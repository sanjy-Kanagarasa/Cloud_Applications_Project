import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { OrderConfirmPage } from './order-confirm';
import {StarRatingModule} from "angular-star-rating";

@NgModule({
  declarations: [
    OrderConfirmPage,
  ],
  imports: [
    IonicPageModule.forChild(OrderConfirmPage),
    StarRatingModule.forRoot()
  ],
})
export class OrderConfirmPageModule {}
