import { Component } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import 'rxjs/add/operator/map'
import { SpinnerService } from './loading-spinner.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  roomList: any = null;
  allRoomList: any = null;
  host = 'http://alfred-hack.eastus.cloudapp.azure.com';
  selectedLocation:String = "POR";
  locations: any = ["Loading..."];
  activeSpinner: boolean = true;


  constructor(private _http: Http, private spinnerService : SpinnerService){
    this.spinnerService.spinnerActive.subscribe(active => 
    this.toggleSpinner(active));
    console.log("App " + this.locations);    
    this.getRooms(()=>{
      this.roomList = this.GetLocationRooms(this.selectedLocation);
      spinnerService.deactivate();
    });
  }

  toggleSpinner(active){
    console.log("inside toggle spinner");
    this.activeSpinner = active;
  }

  private getRooms(callback){
    console.log("Getting rooms from app level.");
    this.activeSpinner = true;
    return this._http.get(this.host + '/RestServer/api/rooms')
                .map((res: Response) => res.json())
                .subscribe(data => {
                  this.allRoomList = data;
                  console.log(this.allRoomList);
                  this.GetAllLocations();
                  callback();
                  this.activeSpinner = false;
                })
  }

  GetLocationFromRoomName(name):String{
    name = name.replace("\\", "/");
    return name.split("/")[0];
  }

  GetLocationRooms(location){
    return this.allRoomList.filter((room)=>{
      return this.GetLocationFromRoomName (room.Name) == location;
    });
  }

  GetAllLocations(){
    let allLocations = new Set;
    for (let i = 0; i < this.allRoomList.length; i++){
      allLocations.add(this.GetLocationFromRoomName(this.allRoomList[i].Name));
    }
    this.locations = allLocations;
  }

  updateLocationSetting(location){
    console.log("Updating rooms...");
    this.selectedLocation = location;
    this.getRooms(()=>{
      this.roomList = this.GetLocationRooms(this.selectedLocation);
      console.log("Rooms updated.");
    });
  }

}
