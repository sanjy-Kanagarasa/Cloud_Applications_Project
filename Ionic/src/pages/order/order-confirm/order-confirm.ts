import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {Storage} from '@ionic/storage';
import {AlertController, IonicPage, LoadingController, NavController, NavParams} from 'ionic-angular';
import {Driver, DriverFlavour} from "../../../Models/driver";
import {OrderProvider} from "../../../providers/order";
import {ShoppingCart} from "../../../Models/flavour.model";
import {ConfirmOrder} from "../../../Models/order";
import {UserProvider} from "../../../providers/user";
import {Toast} from "@ionic-native/toast";
import {ReviewProvider} from "../../../providers/review";
import {Review} from "../../../Models/review";

@IonicPage()
@Component({
  selector: 'page-order-confirm',
  templateUrl: 'order-confirm.html',
})
export class OrderConfirmPage implements OnInit{
  driverWithPrice: DriverFlavour;
  shoppingCart: ShoppingCart;
  chosenDiver: Driver;
  confirmed: boolean = false;
  driverReviews: Review[] = [];
  confirmOrderRepo: ConfirmOrder = {
    orderID: null,
    customerEmail: null,
    driverEmail: null,
    totalPrice: null
  };
  constructor(private navCtrl: NavController,
              private navParams: NavParams,
              private ref: ChangeDetectorRef,
              private orderProvider: OrderProvider,
              private storage: Storage,
              private alertCtrl: AlertController, private toast: Toast,
              private userProvider: UserProvider, private loadingCtrl: LoadingController,
              private reviewProvider: ReviewProvider) {
  }

  ngOnInit(){
    this.chosenDiver = this.navParams.get('driver');
    this.storage.ready().then(
      ()=>{
        this.storage.get('orderId').then(
          id => {
            if(id != null){
              this.orderProvider.getOrderBackWithTotalPrice(id, this.chosenDiver.email).subscribe(
                data => {
                  this.shoppingCart = data.shoppingCart;
                  this.confirmOrderRepo.orderID = id;
                }
              );
            }
          }
        );
      });
    this.reviewProvider.getDriverReviews(this.chosenDiver.email).subscribe(
      (data: Review[]) => {
        this.driverReviews = data;
        this.ref.detectChanges();
      }
    );
  }

  onCancelOrder(){
    let confirm = this.alertCtrl.create({
      title: 'Order cancellation ',
      message: 'Do you want remove this order?',
      buttons: [
        {
          text: 'No',
          handler: () => {
            console.log('Disagree clicked');
          }
        },
        {
          text: 'Yes',
          handler: () => {
            this.navCtrl.setRoot('ListDriversPage', {cancellation: true});
          }
        }
      ]
    });
    confirm.present();
  }

  onConfirmOrder() {
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    this.confirmOrderRepo.totalPrice = this.chosenDiver.totalPrice;
    this.confirmOrderRepo.driverEmail = this.chosenDiver.email;
    this.userProvider.getCurrentUser().then(
      email => {
        loading.present();
        this.confirmOrderRepo.customerEmail = email;
        this.orderProvider.confirmOrder(this.confirmOrderRepo).subscribe(
          data => {
            loading.dismiss();
            if(data == true){
              this.confirmed = data;
              this.storage.set('cancellation', true);
              this.toast.showLongBottom("Order Placed success!").subscribe(
                toast => {
                  console.log(toast);
                }
              );
              //  this.navCtrl.setRoot('ListDriversPage', {cancellation: true});
            }else{
              this.errorMessage();
            }
          }
        );
      }
    );
  }
  private errorMessage(){
    const alert = this.alertCtrl.create({
      title: 'Error',
      message: 'Try Again!',
      buttons: ['Ok']
    });
    alert.present();
  }
}
