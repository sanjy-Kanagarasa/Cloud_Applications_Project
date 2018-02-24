import { BrowserModule } from '@angular/platform-browser';
import { ErrorHandler, NgModule } from '@angular/core';
import { IonicApp, IonicErrorHandler, IonicModule } from 'ionic-angular';
import { SplashScreen } from '@ionic-native/splash-screen';
import { StatusBar } from '@ionic-native/status-bar';

import { MyApp } from './app.component';
import { UserProvider } from '../providers/user';
import {HttpModule} from "@angular/http";
import {OrderProvider} from '../providers/order';
import {CommonModule} from "@angular/common";
import {Toast} from "@ionic-native/toast";
import { DriverProvider } from '../providers/driver';
import {IonicStorageModule} from "@ionic/storage";
import {StarRatingModule} from "angular-star-rating";
import { ReviewProvider } from '../providers/review';

@NgModule({
  declarations: [
    MyApp
  ],
  imports: [
    BrowserModule,
    HttpModule,
    CommonModule,
    IonicModule.forRoot(MyApp),
    IonicStorageModule.forRoot(),
    StarRatingModule.forRoot()
  ],
  bootstrap: [IonicApp],
  entryComponents: [
    MyApp
  ],
  providers: [
    StatusBar,
    SplashScreen,
    Toast,
    {provide: ErrorHandler, useClass: IonicErrorHandler},
    UserProvider,
    OrderProvider,
    DriverProvider,
    ReviewProvider
  ]
})
export class AppModule {}
