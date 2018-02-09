import { Component, OnInit } from '@angular/core';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/map'
import { Observable } from 'rxjs/Observable'
import { Http, Response, Headers } from '@angular/http'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { RoomDetailComponent } from '../room-detail/room-detail.component';

var host = 'http://localhost';

@Component({
  selector: 'calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.css']
})
export class CalendarViewComponent implements OnInit {
  rooms: any = null;

  cRooms$: Observable<RoomDetailComponent[]>;
  private selectedId = 0;
  private route: ActivatedRoute

  constructor(
    private _http: Http,
    private router: Router
  ){
    console.log("Getting Rooms");
    this.getRooms();
  }

  ngOnInit() {
  }

  private getRooms(){
    return this._http.get(host + '/RestServer/api/rooms')
                .map((res: Response) => res.json())
                .subscribe(data => {
                  this.rooms = data;
                  console.log(this.rooms);
                })
  }

  loadRoomComponent(roomAddress){
    this.router.navigate(['rooms', roomAddress]);
  }
}
