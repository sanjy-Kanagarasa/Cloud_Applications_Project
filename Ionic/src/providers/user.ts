import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import { Storage } from '@ionic/storage';
import { HubConnection } from '@aspnet/signalr-client';
import { App } from 'ionic-angular';
import { OrderInProgressPage } from '../pages/order/order-in-progress/order-in-progress';
import { Location } from '../Models/driver';

@Injectable()
export class UserProvider {
  private hubConnection;
  ip: string = 'http://cloud-app.ddns.net/api/users/';

  constructor(public http: Http, private storage: Storage, private app: App) {

  }
  private headers: Headers = new Headers({
    'Accept': 'application/json'
  });
  getAllUsers() {
    return this.http.get(this.ip);
  }
  createAccount(user: JSON) {
    console.log(user);
    return this.http.post(this.ip, user);
  }

  updateCurrentLocation(email: string, location: Location) {
    console.log(email);
    console.log(location);
    return this.http.post(this.ip + 'location/' + email, location).map(resp => {
      return resp.json();
    });
  }

  login(email: string, user: JSON) {

    return this.http.post(this.ip + email, user, { headers: this.headers })
      .map((response: Response) => {
        if (response.json() == 'CUSTOMER' || response.json() == 'DRIVER') {
          this.storage.ready().then(() => {
            this.storage.set('currentUser', email);
          });
        }
        return response.json();
      });
  }
  getUser(email: string) {
    return this.http.get(this.ip + email)
      .map((response: Response) => {
        return response.json();
      });
  }
  getCurrentUser() {
    return this.storage.get('currentUser').then();
  }
  getDrivers() {
    return this.http.get(this.ip + 'drivers')
      .map((response: Response) => {
        return response.json();
      });
  }

  startSignalRSession(email: string) {
     this.hubConnection = new HubConnection('http://cloud-app.ddns.net/orderhub?email=' + email);
      // Method used to debug server info
      this.hubConnection.on('Debug', (data: any) => {
        console.log(data);
      });

      this.hubConnection.on('OrderNotification', (data: any) => {
        this.app.getRootNavs()[0].setRoot('CurrentOrdersPage', { 'data' : data.value });
      });

      this.hubConnection.on('CustomerNotification', (data: any) => {
        console.log(data);
        this.app.getRootNavs()[0].setRoot('OrderInProgressPage', { 'driver' : data });
      });

      this.hubConnection.start()
        .then(() => {
          console.log('Hub connection started');
          console.log(this.hubConnection);
          localStorage.setItem('hubConnection', this.hubConnection);
        })
        .catch(err => {
          console.log('Error while establishing connection')
        });
  }

  askForDriverLocation(email: string) {
    return this.http.get('http://cloud-app.ddns.net/api/order/driver/location/' + email)
      .map(resp => {
        return resp.json();
      })
  }

  stopSignalRSession() {
    if (this.hubConnection != null) this.hubConnection.stop();
  }
}
