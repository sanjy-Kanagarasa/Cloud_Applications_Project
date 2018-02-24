import {Component, OnInit} from '@angular/core';
import {AlertController, IonicPage, Loading, LoadingController, NavController, NavParams} from 'ionic-angular';
import {NgForm} from "@angular/forms";
import {UserProvider} from "../../providers/user";
import {ListDriversPage} from "../list-drivers/list-drivers";
import {OrderProvider} from "../../providers/order";

@IonicPage()
@Component({
  selector: 'page-signin',
  templateUrl: 'signin.html',
})
export class SigninPage implements OnInit{


  constructor(private navCtrl: NavController,
              private alertCtrl: AlertController,
              private loadingCtrl: LoadingController,
              private userProvider: UserProvider,
              private orderProvider: OrderProvider,
              ) { }
  ngOnInit(){
    this.orderProvider.setOrderIdToNull();
  }
  onCreateAccount(){
    this.navCtrl.push('SignupPage');
  }

  onLogin(form:NgForm){
    const loading = this.loadingCtrl.create({
      content: 'Please wait...'
    });
    loading.present();
    this.userProvider.login(form.value.email, form.value).subscribe(
      data => {
        console.log(data);
        if(data == "CUSTOMER"){
          loading.dismiss();
          this.userProvider.startSignalRSession(form.value.email);
          this.navCtrl.setRoot('ListDriversPage');
        } else if(data == "DRIVER") {
          loading.dismiss();
          this.userProvider.startSignalRSession(form.value.email);
          this.navCtrl.setRoot('DriverDashboardPage');
        }
        else if(data == false) {
          loading.dismiss();
          this.passwordIncorrectError();
        }else{
          loading.dismiss();
          this.accountDoesntExistError();
        }
      },
      err => {
        console.log(err);
      }
    );
  }

  // Show alert if email is not correct
  accountDoesntExistError() {
    let alert = this.alertCtrl.create({
      title: 'Account werd niet gevonden',
      message: 'Een nieuwe account registreren?',
      buttons: [
        {
          text: 'Cancel',
          role: 'cancel',
          handler: () => {
            console.log('Cancel clicked');
          }
        },
        {
          text: 'Ja',
          handler: () => {
            this.navCtrl.push('SignupPage');
          }
        }
      ]
      }
    );
    alert.present();
  }

  passwordIncorrectError(){
    const alert = this.alertCtrl.create({
      title: 'Inloggen mislukt',
      message: 'Wachtwoord klopt niet, probeer het opnieuw',
      buttons: ['Ok']
    });
    alert.present();
  }
}
