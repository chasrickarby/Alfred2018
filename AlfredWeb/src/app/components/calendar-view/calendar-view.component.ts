import { Component, OnInit } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import { MatList, MatListItem } from '@angular/material';
import 'rxjs/add/operator/map'

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
    return this._http.get('http://localhost/RestServer/api/rooms')
                .map((res: Response) => res.json())
                .subscribe(data => {
                  this.rooms = data;
                  console.log(this.rooms);
                })

  }
}
