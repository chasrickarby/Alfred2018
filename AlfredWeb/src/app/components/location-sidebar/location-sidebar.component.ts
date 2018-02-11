import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'location-sidebar',
  templateUrl: './location-sidebar.component.html',
  styleUrls: ['./location-sidebar.component.css']
})
export class LocationSidebarComponent implements OnInit {

  _locations: any = "loading...";

  selectedLocation: any;

  @Output() callToParent = new EventEmitter<string>();

  @Input()
  roomList: any;
  @Input()
  set locations(locationList: any){
    console.log("LocationSidebar: " + locationList)
    this._locations = locationList || '<no locations set>';
    console.log("Sidebar Locations: " + this._locations);
    console.log("locations length: " + this._locations.size);
  }

  constructor() {}

  eventResult(location){
    console.log("Selection: " + location);
    this.callToParent.emit(location);
  }

  ngOnInit() {
  }
}
