import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { OrderProvider } from "../../../providers/order";
import { Customer } from "../../../Models/customer";
import { Storage } from "@ionic/storage"
import { ShoppingCart } from "../../../Models/flavour.model";
import { Geolocation } from "@ionic-native/geolocation";

declare var google;
@IonicPage()
@Component({
  selector: 'page-current-orders',
  templateUrl: 'current-orders.html',
})
export class CurrentOrdersPage implements OnInit {

  customer: Customer;
  shoppingCart: ShoppingCart;
  totalPrice: number;
  address: string;
  data: any;
  orderId: number;

  constructor(public navCtrl: NavController, public navParams: NavParams,
    private orderProvider: OrderProvider,
    private storage: Storage,
    private ref: ChangeDetectorRef,
    private geolocation: Geolocation) {
  }

  ngOnInit() {
    this.data = this.navParams.get("data");
    this.shoppingCart = this.data.shoppingCart;
    this.customer = this.data.customer;
    this.totalPrice = this.data.totalPrice;
    let geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: new google.maps.LatLng(this.customer.location.latitude, this.customer.location.longitude) }, function (resp, status) {
      console.log(this.customer);
      console.log(resp);
      this.address = resp[1].formatted_address;
      this.ref.detectChanges();
    }.bind(this));

    // Initialize current drivers location
    // this.updateCurrentPosition();
    // Update drivers locations every 20 sec and send it to the server
    // setInterval(() => {
    //   this.updateCurrentPosition();
    // }, 20000);
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
