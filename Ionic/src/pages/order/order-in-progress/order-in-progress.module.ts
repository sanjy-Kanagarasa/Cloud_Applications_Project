import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderInProgressPage } from './order-in-progress';
import { LocalNotifications } from '@ionic-native/local-notifications';

@NgModule({
  declarations: [
    OrderInProgressPage,
  ],
  imports: [
    IonicPageModule.forChild(OrderInProgressPage),
  ],
  providers: [ LocalNotifications ]
})
export class OrderInProgressPageModule {}
