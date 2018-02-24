import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { UserProvider } from './../../../providers/user';
import { Component, OnInit } from '@angular/core';
import { IonicPage, ModalController, NavController, NavParams } from 'ionic-angular';
import { HubConnection } from '@aspnet/signalr-client';
import { Storage } from '@ionic/storage';
import { Geolocation } from "@ionic-native/geolocation";
declare var google;

@IonicPage()
@Component({
  selector: 'page-driver-dashboard',
  templateUrl: 'driver-dashboard.html',
})
export class DriverDashboardPage implements OnInit, OnDestroy {
  hubConnection: HubConnection;


  constructor(private navCtrl: NavController,
    private userProvider: UserProvider,
    private modalCtrl: ModalController,
    private geolocation: Geolocation) {
  }

  ngOnInit() {
    // Initialize current drivers location
    // this.updateCurrentPosition();
    // Update drivers locations every 20 sec and send it to the server
    // setInterval(() => {
    //   this.updateCurrentPosition();
    // }, 20000);
  }

  ngOnDestroy() {
    this.userProvider.stopSignalRSession();
  }
  onLogout() {
    this.navCtrl.setRoot('SigninPage');
  }
  onUpdateFlavours() {
    let model = this.modalCtrl.create('UpdateFlavoursPage');
    model.present();
  }
  onCurrentOrder() {
    this.navCtrl.push("CurrentOrdersPage");
  }

  updateCurrentPosition() {
    let options = {
      enableHighAccuracy: true,
      timeout: 5000,
      maximumAge: 0
    };

    this.geolocation.getCurrentPosition(options).then(position => {
      console.log(position);
    });
  }
}
