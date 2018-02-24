import {Component, OnInit} from '@angular/core';
import {IonicPage, LoadingController, NavController, NavParams, ViewController} from 'ionic-angular';
import {Flavour, FlavourUpdate} from "../../../Models/flavour.model";
import {OrderProvider} from "../../../providers/order";
import {CurrencyPipe} from "@angular/common"
import {DriverProvider} from "../../../providers/driver";
import {DriverFlavour} from "../../../Models/driver";

@IonicPage()
@Component({
  selector: 'page-update-flavours',
  templateUrl: 'update-flavours.html',
})
export class UpdateFlavoursPage implements OnInit{
  flavours: FlavourUpdate[] = [];
  driverFlavours: DriverFlavour;
  constructor(public navCtrl: NavController, private viewCtrl: ViewController,
              private orderProvider: OrderProvider,
              private driverProvider: DriverProvider,
              private loadingCtrl: LoadingController) {
  }
  ngOnInit(){
    /*this.orderProvider.getFlavours().subscribe(
      (data: FlavourUpdate[]) => {
        for(let i = 0; i < data.length; i++){
          this.flavours.push(new FlavourUpdate(data[i].name, data[i].price));
        }
      }
    );*/
    this.driverProvider.getCurrentUser().then(user => {
      this.driverProvider.getDriverFlavourPrice(user).subscribe(
        (data: DriverFlavour) => {
          this.driverFlavours = data;
          for(let i = 0; i < this.driverFlavours.flavours.length; i++){
            this.flavours.push(new FlavourUpdate(this.driverFlavours.flavours[i].name, this.driverFlavours.flavours[i].price));
          }
          //console.log(this.flavours[1].price);
        }
      );
    });

  }

  onUpdateFlavours(){
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    loading.present();
    this.driverProvider.getCurrentUser().then(user => {
      this.driverProvider.updateFlavours(this.flavours, user).subscribe(
        data => {
          if(data){
            loading.dismiss();
            this.viewCtrl.dismiss();
          }
      });
    });
  }
  dismiss() {
    this.viewCtrl.dismiss();
  }

}
