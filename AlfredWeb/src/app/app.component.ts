import { Component } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import 'rxjs/add/operator/map'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  roomList: any = null;
  host = 'http://alfred-hack.eastus.cloudapp.azure.com';


  constructor(private _http: Http){
    console.log("Constructor");
    this.getRooms();
  }

  private getRooms(){
    console.log("Getting rooms from app level.");
    return this._http.get(this.host + '/RestServer/api/rooms')
                .map((res: Response) => res.json())
                .subscribe(data => {
                  this.roomList = data;
                  console.log(this.roomList);
                })
  }
}
