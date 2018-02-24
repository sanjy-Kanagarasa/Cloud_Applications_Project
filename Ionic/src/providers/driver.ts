import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import { Storage } from '@ionic/storage'
import 'rxjs/add/operator/map';
import {FlavourUpdate, FlavourUpdateJson} from "../Models/flavour.model";

@Injectable()
export class DriverProvider {
  ip: string = 'http://cloud-app.ddns.net/api/driver/';
  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  constructor(public http: Http, private storage: Storage) {
  }
  postFlavours: FlavourUpdateJson = new FlavourUpdateJson([new FlavourUpdate("", 0)]);

  updateFlavours(flavours: FlavourUpdate[], currentDriver: string){
      return this.http.post(this.ip + currentDriver, flavours, {headers: this.headers })
        .map((response: Response) => {
          return response.json();
        });
  }
  getDriverFlavourPrice(user: string){
    return this.http.get(this.ip + user)
      .map((response: Response) => {
        return response.json();
      });
  }
  getCurrentUser(){
    return this.storage.get("currentUser").then();
  }


}
