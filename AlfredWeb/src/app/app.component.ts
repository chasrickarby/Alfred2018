import { Component } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import 'rxjs/add/operator/map'
import { SpinnerService } from './loading-spinner.service';
import { AlfredApiService } from './services/alfred-api.service'

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  roomList: any = null;
  allRoomList: any = null;
  selectedLocation:String = "POR";
  locations: any = ["Loading..."];
  activeSpinner: boolean = true;
  cacheDuration: number = 24*60*60*1000;


  constructor(private api: AlfredApiService, private spinnerService : SpinnerService){
    this.spinnerService.spinnerActive.subscribe(active => 
    this.toggleSpinner(active));
    console.log("App " + this.locations);
    this.getRooms((allRoomList)=>{
      this.allRoomList = allRoomList;
      this.GetAllLocations();
      this.roomList = this.GetLocationRooms(this.selectedLocation);
      spinnerService.deactivate();
      this.activeSpinner = false;
    });
  }

  toggleSpinner(active){
    console.log("inside toggle spinner");
    this.activeSpinner = active;
  }

  private GetRoomsFromLocalStorage(cacheDuration){
    let allRoomListLocal = localStorage.getItem('allRoomList');
    let allRoomListDateLocal = localStorage.getItem('allRoomListDate');
    if (allRoomListLocal && allRoomListDateLocal){
        let nowDate = new Date();
        let cacheData = new Date(allRoomListDateLocal);
        if (nowDate.getTime() - cacheData.getTime() < cacheDuration){
          return JSON.parse(allRoomListLocal);
        }
    }
    return null;
  }

  private getRooms(callback){
    console.log("Getting rooms from app level.");
    // First try to get rooms list from local storage
    let allRoomListLocal = this.GetRoomsFromLocalStorage(this.cacheDuration);
    if(allRoomListLocal){
      console.log("Get rooms from the local storage.")
      callback(allRoomListLocal);
      return true;
    }

    // If local storage is empty or cache is expired, request the list from server
    this.activeSpinner = true;
    return this.api.GetRooms((allRoomList)=>{
                  console.log(allRoomList);
                  localStorage.setItem('allRoomListDate', (new Date().toString()));
                  localStorage.setItem('allRoomList', JSON.stringify(allRoomList));
                  console.log("Get rooms from the server.")
                  callback(allRoomList);
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
    this.roomList = this.GetLocationRooms(this.selectedLocation);
    console.log("Rooms updated.");
  }

}
