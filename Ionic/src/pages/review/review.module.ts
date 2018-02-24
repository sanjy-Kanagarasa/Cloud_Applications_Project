import { NgModule } from '@angular/core';
import { IonicPageModule } from 'ionic-angular';
import { ReviewPage } from './review';
import {StarRatingModule} from "angular-star-rating";

@NgModule({
  declarations: [
    ReviewPage,
  ],
  imports: [
    IonicPageModule.forChild(ReviewPage),
    StarRatingModule.forRoot()
  ],
})
export class ReviewPageModule {}
