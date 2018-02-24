import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import { IonicPage, NavController, NavParams } from 'ionic-angular';
import {OnRatingChangeEven} from "angular-star-rating";
import {Review} from "../../Models/review";
import {Driver} from "../../Models/driver";
import {Customer} from "../../Models/customer";
import {Storage} from "@ionic/storage";
import {ReviewProvider} from "../../providers/review";
import {UserProvider} from "../../providers/user";
import {Toast} from "@ionic-native/toast";

@IonicPage()
@Component({
  selector: 'page-review',
  templateUrl: 'review.html',
})
export class ReviewPage implements OnInit{
  review: Review = {
    review: "",
    rating: 0,
    reviewToEmail: null,
    reviewFromEmail: null,
    reviewerName: null
  };
  reviewTextArray: string[] = ['Very bad ', 'Bad ', 'Average ', 'Good', 'Very good'];
  reviewText = "";
  driver: Driver;
  customerEmail = "";
  constructor(private navCtrl: NavController,
              private navParams: NavParams,
              private ref: ChangeDetectorRef,
              private reviewProvider: ReviewProvider,
              private userProvider: UserProvider,
              private toast: Toast) {
  }
  ngOnInit() {
    this.userProvider.getCurrentUser().then(
      user => {
        this.customerEmail = user;
        this.review.reviewFromEmail = this.customerEmail;
      });
    this.driver = this.navParams.get('driver');
    this.review.reviewToEmail = this.driver.email;

  }
  onRatingChange = ($event:OnRatingChangeEven) => {
    this.review.rating = $event.rating;
    this.reviewText = this.reviewTextArray[$event.rating-1];
    this.ref.detectChanges();
    //console.log(this.review);
  };
  onSubmitReview(){
    this.review.reviewToEmail = this.driver.email;
    console.log("test");
    this.reviewProvider.addReviewToDriver(this.review).subscribe(
      data => {
        if(data == true){
          this.toast.showLongBottom("Thank you for the feedback! ").subscribe(
              toast => {
                console.log(toast);
              }
            );
          this.navCtrl.setRoot('ListDriversPage', {cancellation: true});
        }
      }
    );
  }
  onNotNow(){
    this.navCtrl.setRoot('ListDriversPage');
  }
}
