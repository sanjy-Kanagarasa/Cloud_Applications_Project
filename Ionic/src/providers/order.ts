import { Injectable } from '@angular/core';
import { Storage } from '@ionic/storage';
import { Http, Response, Headers } from '@angular/http';
import 'rxjs/add/operator/map';
import {ConfirmOrder} from "../Models/order";

@Injectable()
export class OrderProvider {
  ip: string = 'http://cloud-app.ddns.net/api/order/';

  constructor(public http: Http, private storage: Storage) {
  }
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  getFlavours(){
    return this.http.get(this.ip)
      .map((response: Response) => {
      return response.json();
    });
  }
  getDriversWithTotalPrice( orderId: number){
    return this.http.get(this.ip + orderId)
      .map((response: Response) => {
        return response.json();
      });
  }

  getOrderBackWithTotalPrice( orderId: number, driverEmail: string){
    return this.http.get(this.ip + orderId+ "/"+ driverEmail)
      .map((response: Response) => {
        return response.json();
      });
  }

  getDriverCurrentOrder(driver: string){
    return this.http.get(this.ip + 1+ "/"+ driver)
      .map((response: Response) => {
        return response.json();
      });
  }

  placeOrder(shoppingCart: JSON, currentUser: string){
    return this.http.post(this.ip + currentUser, shoppingCart, {headers: this.headers})
      .map((response: Response) => {
      return response.json();
    });
  }

  confirmOrder(orderRepo: ConfirmOrder){
    console.log(orderRepo);
    return this.http.post(this.ip + "confirm", orderRepo, {headers: this.headers})
      .map((response: Response) => {
      console.log(response);
        return response.json();
      });
  }
  setOrderIdToNull(){
    this.storage.ready().then(()=>{
      this.storage.set('orderId', null);
    });
  }
}
