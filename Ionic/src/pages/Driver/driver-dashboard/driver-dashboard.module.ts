import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { DriverDashboardPage } from './driver-dashboard';
import { Geolocation } from '@ionic-native/geolocation';

@NgModule({
  declarations: [
    DriverDashboardPage,
  ],
  imports: [
    IonicPageModule.forChild(DriverDashboardPage),
  ],
  providers: [ Geolocation ]
})
export class DriverDashboardPageModule {}
