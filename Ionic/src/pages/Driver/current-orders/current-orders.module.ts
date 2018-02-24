import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { CurrentOrdersPage } from './current-orders';
import { Geolocation } from '@ionic-native/geolocation';

@NgModule({
  declarations: [
    CurrentOrdersPage,
  ],
  imports: [
    IonicPageModule.forChild(CurrentOrdersPage),
  ],
  providers: [ Geolocation ]
})
export class CurrentOrdersPageModule {}
