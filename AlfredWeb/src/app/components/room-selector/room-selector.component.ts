import { Component, OnInit, Input } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import { ActivatedRoute, ParamMap, Router } from '@angular/router';

var host = 'http://localhost';

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
