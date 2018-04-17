import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http'
import { Meeting } from '../components/shared/calendar';

@Injectable()
export class AlfredApiService {
  host:String = 'http://alfred-hack.eastus.cloudapp.azure.com';

  constructor(private _http: Http) {

  }

  GetRoomInformation(roomAddress:String, callback):void{
    console.log("GetRoomInformation");
    this._http.get(this.host + '/RestServer/api/rooms?id=' + roomAddress)
    .map((res: Response) => res.json())
    .subscribe(roomInfo => {
      console.log("Room info");
      console.log(roomInfo);
      callback(roomInfo);
    })
  }

  AddMeeting(roomAddress:String, meeting:Meeting, callback){
    
    let monthStr=(meeting.start.getMonth()+1).toString()
    if(monthStr.length==1){
      monthStr="0"+monthStr;
    }
    let dateStr = meeting.start.getDate().toString()
    if(dateStr.length==1){
      dateStr="0"+dateStr;
    }

    let strData = meeting.start.getFullYear()+"-"+monthStr+"-"+dateStr+"T"
    let hs = meeting.start.getHours().toString()
    if(hs.length==1){
      hs="0"+hs;
    }
    let ms = meeting.start.getMinutes().toString()
    if(ms.length==1){
      ms="0"+ms;
    }
    let meetingStartStr = strData + hs+":"+ms+":00";

    let he = meeting.end.getHours().toString()
    if(he.length==1){
      he="0"+he;
    }
    let me = meeting.end.getMinutes().toString()
    if(me.length==1){
      me="0"+me;
    }
    let meetingEndStr = strData + he +":"+me+":00";

    this._http.post(this.host + "/RestServer/api/rooms/CreateMeeting?id="+roomAddress.split("@")[0]+
    "&subject="+meeting.name+
    "&start="+meetingStartStr+
    "&end="+meetingEndStr, {})
    .map((res: Response) => res.json())
    .subscribe(data => {
      callback();
    })
  }

  GetRooms(callback){
    return this._http.get(this.host + '/RestServer/api/rooms')
                .map((res: Response) => res.json())
                .subscribe(allRoomList => {
                  console.log(allRoomList);
                  callback(allRoomList);
                })
  }

}
