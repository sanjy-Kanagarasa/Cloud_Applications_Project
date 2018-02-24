import { Component, ViewChild, ElementRef, OnInit } from '@angular/core';
declare var google;
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit{
  
  title = 'app';
  lat = 40.748774;
  lng: -73.985763;
  //position = [40.748774, -73.985763];
  @ViewChild('map') mapElement: ElementRef;
  map: any;
  marker: any;
  numDeltas: any = 100;
  delay: any = 10; //milliseconds
  i: any = 0;
  deltaLat: any;
  deltaLng: any;
  
  ngOnInit(): void {
    this.initMap();
  }
  transition(result){
    this.i = 0;
    this.deltaLat = (result[0] - this.lat)/this.numDeltas;
    this.deltaLng = (result[1] - this.lng)/this.numDeltas;
    this.moveMarker();
}

moveMarker() {
  this.lat += this.deltaLat;
  this.lng += this.deltaLng;
    var latlng = new google.maps.LatLng(this.lat, this.lng);
    this.marker.setTitle("Latitude:"+this.lat+" | Longitude:"+this.lng);
    this.marker.setPosition(latlng);
    if(this.i!=this.numDeltas){
      this.i++;
        setTimeout(this.moveMarker, 500);
    }
}
  initMap() {
    let mapOptions = {
      center: new google.maps.LatLng(this.lat, this.lng),
      zoom: 16,
      mapTypeId: google.maps.MapTypeId.ROADMAP,
      draggable: false
    };
    this.map = new google.maps.Map(this.mapElement.nativeElement, mapOptions);
    google.maps.event.addListener(this.map, 'click', function(event) {
      let result = [event.latLng.lat(), event.latLng.lng()];
      //this.transition(result);
      console.log(event.latLng.lat(), event.latLng.lng());
      this.i = 0;
      this.deltaLat = (event.latLng.lat() - event.latLng.lng())/this.numDeltas;
      this.deltaLng = (event.latLng.lng() - event.latLng.lng())/this.numDeltas;
      
      this.position[0] += this.deltaLat;
      this.position[1] += this.deltaLng;
        var latlng = new google.maps.LatLng(this.position[0], this.position[1]);
        this.marker.setTitle("Latitude:"+this.position[0]+" | Longitude:"+this.position[1]);
        this.marker.setPosition(latlng);
        if(this.i!=this.numDeltas){
          this.i++;
            setTimeout(this.moveMarker, 500);
        }
  });
    this.addUserMarker();
  }
  
  addUserMarker() {
    // Define user marker
    let userMarker = new google.maps.Marker({
      map: this.map,
      animation: google.maps.Animation.DROP,
      position: this.map.getCenter(),
      label: 'U'
    });
  }
  
  
  
  
   
  
}
