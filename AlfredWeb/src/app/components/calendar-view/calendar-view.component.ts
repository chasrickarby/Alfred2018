import { Component, OnInit, Input } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { RoomDetailComponent } from '../room-detail/room-detail.component';

var host = 'http://localhost';

@Component({
  selector: 'calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.css']
})
export class CalendarViewComponent implements OnInit {
  @Input() rooms: any;
  @Input() locations: any;

  cRooms$: Observable<RoomDetailComponent[]>;
  private selectedId = 0;
  private route: ActivatedRoute

  constructor(
    private _http: Http,
    private router: Router,
  ){
    console.log("Getting Rooms");
  }

  ngOnInit() {
  }

  loadRoomComponent(roomAddress){
    this.router.navigate(['rooms', roomAddress]);
  }
}
