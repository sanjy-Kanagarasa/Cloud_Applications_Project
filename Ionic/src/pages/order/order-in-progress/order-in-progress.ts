import { ReviewPage } from './../../review/review';
import { UserProvider } from '../../../providers/user';
import { Storage } from '@ionic/storage';
import { Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { LocalNotifications } from '@ionic-native/local-notifications';

declare var google;

@IonicPage()
@Component({
  selector: 'page-order-in-progress',
  templateUrl: 'order-in-progress.html',
})

export class OrderInProgressPage implements OnInit {
  driver: any;
  orderLocation: any;
  notificateIfDriverArrivesInLessThan: number = 5;
  notifacationHasAlreadyBeenFired: boolean = false;
  isOrderFinished;

  constructor(
    private navCtrl: NavController, 
    private navParams: NavParams, 
    private userProvider: UserProvider,
    private storage: Storage,
    private ref: ChangeDetectorRef,
    private localNotifications: LocalNotifications
  ) {}

  ngOnInit() {  
    this.GetDataFromLocalStorage();    
    this.GetDriversInfo();
    this.GetDriverLocationAtInterval();
  }

  GetDriverLocationAtInterval() {

      this.isOrderFinished = setInterval(() => { 
        console.log('order in progress page');
        this.GetDriversInfo();
        this.ref.detectChanges(); // update the template when the callback finishes
       }, 2000);
  }

  GetDriversInfo() {
    this.userProvider.askForDriverLocation(this.driver.email).subscribe(data => {
      // this.userProvider.askForDriverLocation('chingiz-driver@uber.be').subscribe(data => {
        this.driver = data;
        this.CalculateDistanceAndDurationToTheDriver();
        console.log(data);
      });
  }

  GetDataFromLocalStorage() {
    this.driver = this.navParams.get('driver');
    this.orderLocation = new google.maps.LatLng(JSON.parse(localStorage.getItem('coords')).lat, JSON.parse(localStorage.getItem('coords')).lng);    
  }

  CalculateDistanceAndDurationToTheDriver() {
    let distanceCalculator: any = new google.maps.DistanceMatrixService();

    distanceCalculator.getDistanceMatrix({
      origins: [this.orderLocation],
      destinations: [new google.maps.LatLng(this.driver.location.latitude, this.driver.location.longitude)],
      travelMode: google.maps.TravelMode.DRIVING,
      drivingOptions: {
        departureTime: new Date(Date.now())
      }
      // Callback function when the calculation has been finished
    }, function (resp, status) {
      if (status == "OK") {
        // Save driver's info to drivers object
        this.driver.distance = resp.rows[0].elements[0].distance.value;
        this.driver.duration = resp.rows[0].elements[0].duration.value;
        console.log(this.driver.distance / 1000 + 'km');
        console.log(this.driver.duration / 60 + 'min');
        this.ref.detectChanges(); // update the template when the callback finishes
        // Notify the user if the driver will arrive in less than 5 min
        if (((this.driver.duration / 60) < this.notificateIfDriverArrivesInLessThan) && this.notifacationHasAlreadyBeenFired == false) {
          this.FireNotification();
        }
      } else {
        console.log('Error occured with google maps API. Errro: ' + status);
      }
    }.bind(this)); // Access this scope in callback
  }

  FireNotification() {
    // For mobile
    this.localNotifications.schedule({
      id: 1,
      text: 'The driver with your icecream will arrive in 5 mins'
    });

    clearInterval(this.isOrderFinished);
    this.navCtrl.setRoot("ReviewPage", {driver : this.driver});

    // For Dev
    console.log('The driver with your icecream will arrive in 5 mins');
    this.notifacationHasAlreadyBeenFired = true;
  }
}
