import { Component, OnInit, Input } from '@angular/core';
import { MatGridListModule} from '@angular/material/grid-list';
import { MatList, MatListItem } from '@angular/material';
import { Http, Response, Headers } from '@angular/http'
import { ActivatedRoute } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddMeetingCompComponent } from '../add-meeting-comp/add-meeting-comp.component';

import {Meeting, TimeSlot, Day, Week} from '../shared/calendar'

@Component({
  selector: 'schedule-view',
  templateUrl: './schedule-view.component.html',
  styleUrls: ['./schedule-view.component.css']
})


export class ScheduleViewComponent implements OnInit {
  roomAddress: String;
  roomInfo;

  startDate:Date = null;
  host = 'http://alfred-hack.eastus.cloudapp.azure.com';

  timeSlotsLabels:Array<String>=new Array<String>();
  startHour = 6;
  endHour = 21;
  week = null;

  isLoaded:Boolean = false;
  isAddMeeting = {value:false};
  selectedTimeSlot:TimeSlot = null;
    
  constructor(private _http: Http, private route: ActivatedRoute) {
    if (!this.startDate){
      this.startDate = new Date();
    }

    for (let h = this.startHour; h < this.endHour; h++){
      let label:String = "";
      if (h<12){
        label = h+"AM";
      }
      if(h==12){
        label = h+"PM";
      }
      if(h>12){
        label = (h-12) + "PM";
      }
    this.timeSlotsLabels.push(label);
    }

    this.route.params.subscribe( params => {
      console.log(params)
      this.roomAddress = params.id;
      this.getRoomSchedule(this.roomAddress, ()=>{
        this.MakeWeek();
        this.isLoaded = true;
      });
     });
  }
  
  ngOnInit() {
    
  }
  
  getRoomSchedule(roomAddress, callback){
    this._http.get(this.host + '/RestServer/api/rooms?id=' + roomAddress)
                .map((res: Response) => res.json())
                .subscribe(data => {
                  this.roomInfo = data;
                  console.log("Room info");
                  console.log(this.roomInfo);
                  callback()
                })
  }

  MakeWeek(){
    this.week = new Week(this.startDate, "day");
    for (let i = 0; i < this.roomInfo.Events.length; i++){
      this.week.days[0].AddMeeting(new Meeting(
        this.roomInfo.Events[i].Subject,
        new Date(this.roomInfo.Events[i].Start),
        new Date(this.roomInfo.Events[i].End)
      ));
    }
    this.week.days[0].GetTimeSlots();
  }

  AddMeeting(slot:TimeSlot):void{
    this.isAddMeeting.value = true;
    this.selectedTimeSlot = slot;
  }
  OnAddMeeting(meeting:Meeting){
    this.week.day[0].AddMeeting(meeting);
    this.week.days[0].GetTimeSlots();
  }
}
