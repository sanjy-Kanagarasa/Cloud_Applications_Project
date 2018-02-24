import {ChangeDetectorRef, Component, OnInit} from '@angular/core';
import {AlertController, IonicPage, LoadingController, NavController, NavParams, ViewController} from 'ionic-angular';
import {OrderProvider} from "../../providers/order";
import {Toast} from "@ionic-native/toast";
import {Flavour, Icecream, ShoppingCart} from "../../Models/flavour.model";
import {UserProvider} from "../../providers/user";
import {isNumber} from "ionic-angular/util/util";
import { Storage } from '@ionic/storage';
import { Location } from '../../Models/driver';
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';

@IonicPage()
@Component({
  selector: 'page-order',
  templateUrl: 'order.html',
})
export class OrderPage implements OnInit, OnDestroy {
  flavours: string[] = [];
  fixedFlavours: string[] = [];
  usedFlavours: string[] = [];
  amount: number = 1;
  flavoursToSelect = [];
  selectAlertOpts:any;
  currentLocation: Location =  new Location(JSON.parse(localStorage.getItem('coords')).lat, JSON.parse(localStorage.getItem('coords')).lng);


  // location: Location = new Location(0, 0);
  shoppingCart: ShoppingCart = new ShoppingCart([new Icecream([new Flavour("", 1, 0)])], this.currentLocation);
  addFlavours: Flavour[] = [new Flavour("", 1, 0)];
  constructor(public navCtrl: NavController,
              private loadingCtrl: LoadingController,
              private ref: ChangeDetectorRef,
              private orderProvider: OrderProvider,
              private userProvider: UserProvider,
              private storage: Storage,
              private toast: Toast,
              private alertCtrl: AlertController) {
    this.selectAlertOpts = {
      title: 'Flavour',
      subTitle: 'Select your flavour'
    };
  }

  ngOnInit(){
    this.orderProvider.getFlavours().subscribe(
      data => {
        for(let i = 0; i < data.length; i++){
          this.fixedFlavours.push(data[i].name);
        }
        this.flavours = this.fixedFlavours.slice();
        this.flavoursToSelect.push(this.flavours.slice());
      }
    );
    this.shoppingCart.cart.splice(0);
  }

    // Delete session from SignalR database if the user exited the screen
    ngOnDestroy() {
    //   this.userProvider.stopSignalRSession();
    }


  increaseAmount(index: number){
    if(this.addFlavours[index].amount >= 4)
      this.addFlavours[index].amount = 4;
    else
      this.addFlavours[index].amount++;
    this.ref.detectChanges();
  }
  decreaseAmount(index: number){
    if(this.addFlavours[index].amount <= 1)
      this.addFlavours[index].amount = 1;
    else
      this.addFlavours[index].amount--;
    this.ref.detectChanges();
  }
  onAddFlavour(){
    if(this.addFlavours.length>=4)
      return;
    else
      this.addFlavours.push(new Flavour("", 1, 0));
    this.checkFlavourAlreadySelect();
    this.flavoursToSelect.push(this.flavours.slice());
    this.ref.detectChanges();
  }
  onRemoveFlavour(){
    this.addFlavours.splice(this.addFlavours.length-1, 1)
  }
  onAddToCart(){
    this.shoppingCart.cart.push(new Icecream(this.addFlavours));
    this.addFlavours = [new Flavour("", 1, 0)];
    this.flavoursToSelect.splice(0);
    this.flavours = this.fixedFlavours.slice();
    this.flavoursToSelect.push(this.flavours.slice());
    this.ref.detectChanges();

  }
  onRemoveFromList(ice:Icecream){
    let index = this.shoppingCart.cart.indexOf(ice);
    this.shoppingCart.cart.splice(index, 1);
  }
  backToMap(){
    this.navCtrl.setRoot('ListDriversPage');
  }
  placeOrder(){
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    console.log(JSON.stringify(this.shoppingCart));
    loading.present();

    this.userProvider.getCurrentUser().then(user => {
      this.orderProvider.placeOrder(JSON.parse(JSON.stringify(this.shoppingCart)), user).subscribe(
        data => {
          if(isNumber(data)){
            this.storage.set("orderId", data);
            loading.dismiss();
            this.toast.showLongBottom("Order Placed successfull, Choose a Driver").subscribe(
              toast => {
                console.log(toast);
              }
            );
            this.shoppingCart.cart=[];
            this.backToMap();
          }else{
            loading.dismiss();
            this.errorMessage();
          }
        },
        err => {
          console.log(err);
        }
      );
    });

  }
  private errorMessage(){
    const alert = this.alertCtrl.create({
      title: 'Error',
      message: 'Try Again!',
      buttons: ['Ok']
    });
    alert.present();
  }
  private checkFlavourAlreadySelect(){
    //this.flavours.splice(0, 1);
    for(let i = 0; i < this.addFlavours.length; i++){
      if(this.usedFlavours[i] != "" ){
        let f = this.flavours.indexOf(this.addFlavours[i].name);
        if(f>-1)
          this.flavours.splice(f, 1);
      }
      //this.usedFlavours.push(this.addFlavours[i].name);
    }
  }
}


