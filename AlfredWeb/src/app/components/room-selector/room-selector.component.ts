import { Component, OnInit, Input } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

var host = 'http://alfred-hack.eastus.cloudapp.azure.com';

@Component({
  selector: 'room-selector',
  templateUrl: './room-selector.component.html',
  styleUrls: ['./room-selector.component.css']
})
export class RoomSelectorComponent implements OnInit {
  @Input() rooms: any;

  constructor(
    private _http: Http,
    private router: Router) 
    {
      console.log("CTOR Room Selector")
      console.log("Getting Rooms");
    }

  ngOnInit() {
  }

  loadRoomComponent(roomAddress){
    this.router.navigate(['rooms', roomAddress]);
  }

}
