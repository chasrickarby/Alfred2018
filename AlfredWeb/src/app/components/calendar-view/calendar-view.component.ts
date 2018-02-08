import { Component, OnInit } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import 'rxjs/add/operator/map'

var host = 'http://localhost';

@Component({
  selector: 'calendar-view',
  templateUrl: './calendar-view.component.html',
  styleUrls: ['./calendar-view.component.css']
})
export class CalendarViewComponent implements OnInit {
  rooms: any = null;

  constructor(private _http: Http){
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

  getRoomInfo(roomAddress){
    var roomInfo;
    return this._http.get(host + '/RestServer/api/rooms?id=' + roomAddress)
                .map((res: Response) => res.json())
                .subscribe(data => {
                  roomInfo = data;
                  console.log(roomInfo);
                })
  }
}
