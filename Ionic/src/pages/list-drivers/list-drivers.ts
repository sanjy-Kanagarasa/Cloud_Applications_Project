import { Driver } from './../../Models/driver';
import { Component, OnInit, ViewChild, ElementRef, ChangeDetectorRef } from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import { UserProvider } from "../../providers/user";
import { Geolocation } from "@ionic-native/geolocation";
import { Storage } from "@ionic/storage"
import { OrderProvider } from "../../providers/order";
import { isNumber } from "ionic-angular/util/util";
import { OnDestroy } from '@angular/core/src/metadata/lifecycle_hooks';
import { Location } from '../../Models/driver';

declare var google;

@IonicPage()
@Component({
  selector: 'page-list-drivers',
  templateUrl: 'list-drivers.html',
})
export class ListDriversPage implements OnInit, OnDestroy {
  // MAP
  lat: number = null;//51.2304878;
  lng: number = null; //4.477407;
  distance: number = 5;
  zoom: number = 11;
  orderId: number = null;
  @ViewChild('map') mapElement: ElementRef;
  map: any;
  driversMarkers: any[] = [];
  ratingsObject: any;

  // APP
  title: string = 'Uber voor ijskarren';

  // DRIVERS
  drivers: Driver[] = [];
  driversInZone: Driver[] = [];

  //orderCancel
  isOrderCanceled = false;

  //rating


  constructor(public navCtrl: NavController,
    private navParams: NavParams,
    private storage: Storage,
    private userProvider: UserProvider,
    private orderProvider: OrderProvider,
    private geolocation: Geolocation,
    private ref: ChangeDetectorRef) {
    this.isOrderCanceled = this.navParams.get("cancellation");
    this.storage.get("cancellation").then(
      data => {
        this.isOrderCanceled = data;
      });
  }

  ngOnInit() {
    // Remove price label from drivers list if order has been cancelled
    if (this.isOrderCanceled) {
      this.removeOrder();
    }

    // Check if current position has been saved in the localstorage
    if (JSON.parse(localStorage.getItem('coords')) != null) {
      // if it has, check if lat and lng are not zero's
      this.lat = JSON.parse(localStorage.getItem('coords')).lat;
      this.lng = JSON.parse(localStorage.getItem('coords')).lng;
      // If they are zero's, get users current position
      if (this.lat == 0 || this.lng == 0) {
        this.getUserPosition();
        // if they have been set, display the map and get drivers info
      } else {
        this.initMap();
        this.getDriversInfo();
        this.ref.detectChanges();
      }
      // if current position does not exist in the localstorage, get users current location
    } else {
      this.getUserPosition();
    }
  }

  // Delete session from SignalR database if the user exited the screen
  ngOnDestroy() {
    //   this.userProvider.stopSignalRSession();
  }


  getDriversInfo() {
    this.userProvider.getCurrentUser().then(email => {
      this.userProvider.updateCurrentLocation(email, new Location(this.lat, this.lng)).subscribe(result => {
        console.log(result);
      });
    });
    
    // Check if an order has been placed
    // if it has, display the list of drivers with their prices
    this.storage.ready().then(() => {
      this.storage.get('orderId').then(orderId => {
        if (isNumber(orderId)) {
          this.loadDriversWithTotalPrice(orderId);
        } else {
          // if no order has been placed, display the list of drivers without their prices
          this.userProvider.getDrivers().subscribe(
            data => {
              this.drivers = data;
              this.checkRangeOfDrivers();
            },
            err => {
              console.log(err);
            });
        }

      });
    });
  }

  initMap() {
    let mapOptions = {
      center: new google.maps.LatLng(this.lat, this.lng),
      zoom: this.zoom,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      scrollwheel: false,
      draggable: false
    };
    this.map = new google.maps.Map(this.mapElement.nativeElement, mapOptions);
    this.addUserMarker();
  }


  onChooseDriver(driver: Driver) {
    if (!driver.totalPrice) {
      // console.log("You have to place order first!");
      this.navCtrl.push("ReviewPage", { driver: driver });
    } else {
      this.navCtrl.push("OrderConfirmPage", { driver: driver });
    }
  }

  addUserMarker() {
    // Define user marker
    let userMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: this.map.getCenter(),
      label: 'U'
    });

    // Get users address based on his coordinates and put it into the infowindow
    let geocoder = new google.maps.Geocoder();
    geocoder.geocode({ location: new google.maps.LatLng(this.lat, this.lng) }, function (resp, status) {
      let content = "<p>" + resp[0].formatted_address + "</p>";
      let infoWindow = new google.maps.InfoWindow({
        content: content
      });

      // display the infowindow when clicking on the marker
      google.maps.event.addListener(userMarker, 'click', () => {
        infoWindow.open(this.map, userMarker);
      });
    });

    // display the marker on the map
    userMarker.setMap(this.map);
  }

  addDriversMarkers(driver: Driver) {
    // Drivers location
    let driversLocation = new google.maps.LatLng(driver.location.latitude, driver.location.longitude)

    // Drivers marker
    let driverMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: driversLocation
    });

    // Translate drivers coordinates to address and display it on the infowindow
    let content =
      "<p><b>Naam: </b>" + driver.firstName + " " + driver.lastName +
      "<p><b>Afstand: </b>" + (driver.distance / 1000).toFixed(2) + " km</p>" +
      "<p><b>Duur: </b>" + (driver.duration / 60).toFixed(1) + " min</p>" +
      "<p><b>Locatie: </b>" + driver.locationAddress + "</p>"
    let infoWindow = new google.maps.InfoWindow({
      content: content
    });

    // display the infowindow when clicking on the marker
    google.maps.event.addListener(driverMarker, 'click', () => {
      infoWindow.open(this.map, driverMarker);
    });

    // display the driver marker on the map
    this.driversMarkers.push(driverMarker);
  }


  // Does not work with live reload
  getUserPosition() {
    let options = {
      enableHighAccuracy: true
    };

    // Try to get users location using geolocator
    this.geolocation.getCurrentPosition(options).then(position => {
      let geocoder = new google.maps.Geocoder();
      // Translate current coordinates to the address and ask the user whether the address is correct
      geocoder.geocode({ location: new google.maps.LatLng(position.coords.latitude, position.coords.longitude) }, function (resp, status) {
        let isAddressCorrect = confirm('Uw locatie is: ' + resp[0].formatted_address + '. Is deze correct bepaald?');
        // if the address is correct, save the coordinates and show the map and drivers in range
        if (isAddressCorrect) {
          this.lat = position.coords.latitude;
          this.lng = position.coords.longitude;
          localStorage.setItem("coords", JSON.stringify(new google.maps.LatLng(this.lat, this.lng)));
          this.initMap();
          this.getDriversInfo();
          this.ref.detectChanges();
          // if the address is not correct, ask the user to provide his address and translate it to coordinates, then show the map with drivers in range
        } else {
          let address = prompt('Geef hier uw adres in');
          geocoder.geocode({ address: address }, function (resp, status) {
            this.lat = resp[0].geometry.location.lat();
            this.lng = resp[0].geometry.location.lng();
            localStorage.setItem("coords", JSON.stringify(new google.maps.LatLng(this.lat, this.lng)));
            //console.log(this.lat);
            //console.log(this.lng);
            this.initMap();
            this.getDriversInfo();
            this.ref.detectChanges();
            // this.initMap();
          }.bind(this));
        }
      }.bind(this));
    }).catch(err => {
      console.log('Position error: ' + err.message);
    });
  }

  loadDriversWithTotalPrice(orderId: number) {
    this.orderProvider.getDriversWithTotalPrice(orderId).subscribe(
      data => {
        //console.log(data);
        this.drivers = data;
        this.checkRangeOfDrivers();
      },
      err => {
        console.log(err);
      });
  }

  // if the user changes the range, recalculate the drivers whithin that range
  onRangeChange(event: any) {
    this.zoom = event.value;
    this.map.setZoom(this.zoom);
    switch (event.value) {
      case 11:
        this.distance = 5;
        break;
      case 12:
        this.distance = 4;
        break;
      case 13:
        this.distance = 3;
        break;
      case 14:
        this.distance = 2;
        break;
      case 15:
        this.distance = 1;
        break;
      default:
        this.distance = 0;
    }
    this.checkRangeOfDrivers();
  }

  onLogout() {
    this.userProvider.stopSignalRSession();
    this.navCtrl.setRoot('SigninPage');
  }

  placeOrder() {
    this.navCtrl.push('OrderPage');
  }


  checkRangeOfDrivers() {

    // Clear driversInZone array
    while (this.driversInZone.length > 0) {
      this.driversInZone.pop();
    }

    // Clear driver markers
    for (let driverMarker of this.driversMarkers) {
      driverMarker.setMap(null);
    }

    // Wait until the users position has been determined
    if (this.lat != null && this.lng != null) {
      // Distance calculator
      let origin = new google.maps.LatLng(this.lat, this.lng);
      let distanceCalculator: any = new google.maps.DistanceMatrixService();
      //console.log("Range: " + this.distance + "km");

      // calculate distance between origin and destination for each driver
      for (let driver of this.drivers) {
        distanceCalculator.getDistanceMatrix({
          origins: [origin],
          destinations: [new google.maps.LatLng(driver.location.latitude, driver.location.longitude)],
          travelMode: google.maps.TravelMode.DRIVING,
          drivingOptions: {
            departureTime: new Date(Date.now())
          }
          // Callback function when the calculation has been finished
        }, function (resp, status) {
          if (status == "OK") {
            // Save driver's info to drivers object
            driver.locationAddress = resp.destinationAddresses;
            driver.distance = resp.rows[0].elements[0].distance.value;
            driver.duration = resp.rows[0].elements[0].duration.value;
            this.ref.detectChanges();
            // If the driver is in the range zone, add him to the temporary array and create a marker
            if ((driver.distance / 1000) < this.distance) {
              this.driversInZone.push(driver);
              this.addDriversMarkers(driver);
            }
          } else {
            console.log('Error occured with google maps API. Errro: ' + status);
          }
        }.bind(this)); // Access this scope in callback
      }
    }
  };

  removeOrder() {
    this.storage.set('orderId', null);
    for (let driver of this.drivers) {
      driver.totalPrice = null;
    }
  }

  showDriversInZone() {
    //console.log(this.driversInZone);
  }
}
