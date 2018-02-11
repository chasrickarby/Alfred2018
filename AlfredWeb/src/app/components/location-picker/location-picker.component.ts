import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'location-picker',
  templateUrl: './location-picker.component.html',
  styleUrls: ['./location-picker.component.css']
})
export class LocationPickerComponent implements OnInit {

  _locations: Set<string> = new Set().add("Loading...");
  locationArray: any;

  value: any;
  getLocation: any;

  @Input()
  // locations: any;
  set locations(locationList: any){
    console.log("LocationSidebar: " + locationList)
    this._locations = locationList || '<no locations set>';
    if(this._locations !== undefined){
      
    }
  }

  @Output() myEvent = new EventEmitter<string>();

  constructor() {
    this.locationArray = new Array(); 
    console.log("Location Array: " + this.locationArray.length);
  }

  ngOnInit() {
  }

  printLocation(location){
    this.value = location;
    console.log("Location set to: " + location);
    this.getLocation = location;
    this.myEvent.emit(location);
  }

}
