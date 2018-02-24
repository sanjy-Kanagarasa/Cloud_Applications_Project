import { Component } from '@angular/core';
import {IonicPage, LoadingController, NavController, NavParams} from 'ionic-angular';
import {Customer} from "../../Models/customer";
import {NgForm} from "@angular/forms";
import {UserProvider} from "../../providers/user";

@IonicPage()
@Component({
  selector: 'page-signup',
  templateUrl: 'signup.html',
})
export class SignupPage {
  user: string = 'Customer';
  customer: Customer = {
    firstName: '',
    lastName: '',
    phoneNumber: '',
    email:'',
    password:'',
    userRoleType: 0,
    address: {
      streetName: '',
      streetNumber: '',
      zipCode: null
    },
    location: null
  };
  constructor(public navCtrl: NavController,
              public loadingCtrl: LoadingController,
              private userProvider: UserProvider) {
  }
  onRegister(form: NgForm, type: string){
    let user= form.value;//JSON.parse(form.value);
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    loading.present();
    console.log(type);
    if(type == 'Customer'){
      user['userRoleType'] = 0;
    }else {
      user['userRoleType'] = 1;
    }
    this.userProvider.createAccount(user).subscribe(
      data => {
        console.log(data);
        if(data){
          loading.dismiss();
          this.navCtrl.setRoot('SigninPage');
        }
      },
      err => {
        console.log(err.message);
      }
    );
    //console.log(this.user);
  }

}
