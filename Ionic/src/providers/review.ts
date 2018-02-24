import { Injectable } from '@angular/core';
import { Http, Headers, Response } from '@angular/http';
import 'rxjs/add/operator/map';
import {Review} from "../Models/review";

@Injectable()
export class ReviewProvider {
  ip: string = 'http://cloud-app.ddns.net/api/review/';

  private headers: Headers = new  Headers({
    'Accept': 'application/json'
  });
  constructor(public http: Http) {

  }
  addReviewToDriver(driverReview: Review){
    console.log(driverReview);
    return this.http.post(this.ip + 'driver', driverReview)
      .map((response: Response) => {
        console.log(response);
        return response.json();
      });
  }
  getDriverReviews(email: string){
    return this.http.get(this.ip + email,)
      .map((response: Response) => {
        return response.json();
      });
  }
  addReviewToCustomer(customerReview: Review){
    return this.http.post(this.ip + 'driver', customerReview, {headers: this.headers })
      .map((response: Response) => {
        return response.json();
      });
  }
}
