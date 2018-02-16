import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import {MatSelectModule} from '@angular/material/select';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import { Http, Response, Headers } from '@angular/http';

import {Meeting} from '../shared/calendar'

@Component({
  selector: 'add-meeting',
  templateUrl: './add-meeting-comp.component.html',
  styleUrls: ['./add-meeting-comp.component.css']
})
export class AddMeetingCompComponent implements OnInit {
  @Input() endHour;
  @Input() startHour;
  @Input() isAddMeeting;

  startDate:Date;
  @Input() 
  set startDateLink(startDate: Date) {
    console.log(startDate)
    this.startDate = startDate;
    if(this.daySlots){
      this.UpdateSelecters();
    }
  }

  daySlots:DaySlot[];
  endTimeDaySlots:DaySlot[];

  selectedStartTime:DaySlot;
  selectedEndTime:DaySlot;

  @Input() roomAddress;
  meetingSubject="";

  day:Date;

  host = 'http://alfred-hack.eastus.cloudapp.azure.com';

  @Output() OnAddMeeting = new EventEmitter();

  constructor(private _http: Http) {
  }

  ngOnInit() {
    this.daySlots = new Array<DaySlot>();
    for (let h = this.startHour; h <= this.endHour; h++){
      this.daySlots.push(new DaySlot(h, 0));
      this.daySlots.push(new DaySlot(h, 15));
      this.daySlots.push(new DaySlot(h, 30));
      this.daySlots.push(new DaySlot(h, 45));
      }

    this.UpdateSelecters();

    console.log(this.selectedStartTime);
  }

  UpdateSelecters():void{
    this.selectedStartTime = this.daySlots.find((slot) => {
      return this.startDate.getHours() == slot.hour && this.startDate.getMinutes() == slot.minute;
    });

    this.endTimeDaySlots = this.daySlots.filter((slot)=>{
      return slot.hour > this.selectedStartTime.hour || 
      (slot.hour == this.selectedStartTime.hour && slot.minute > this.selectedStartTime.minute);
    });

    this.selectedEndTime = this.endTimeDaySlots.find((slot) => {
      let h = this.startDate.getHours();
      let m = this.startDate.getMinutes() + 15
      if (m==60){
        m = 0;
        h++;
      }
      return h == slot.hour && m == slot.minute;
    });
  }

  AddMeeting(){
    let meetingStart = new Date(this.startDate);
    meetingStart.setHours(this.selectedStartTime.hour, this.selectedStartTime.minute,0,0);

    let meetingEnd = new Date(this.startDate);
    meetingEnd.setHours(this.selectedEndTime.hour, this.selectedEndTime.minute,0,0);

    let monthStr=(meetingStart.getMonth()+1).toString()
    if(monthStr.length==1){
      monthStr="0"+monthStr;
    }
    let dateStr = meetingStart.getDate().toString()
    if(dateStr.length==1){
      dateStr="0"+dateStr;
    }

    let strData = meetingStart.getFullYear()+"-"+monthStr+"-"+dateStr+"T"
    let hs = meetingStart.getHours().toString()
    if(hs.length==1){
      hs="0"+hs;
    }
    let ms = meetingStart.getMinutes().toString()
    if(ms.length==1){
      ms="0"+ms;
    }
    let meetingStartStr = strData + hs+":"+ms+":00";

    let he = meetingEnd.getHours().toString()
    if(he.length==1){
      he="0"+he;
    }
    let me = meetingEnd.getMinutes().toString()
    if(me.length==1){
      me="0"+me;
    }
    let meetingEndStr = strData + he +":"+me+":00";
    
    this._http.post(this.host + "/RestServer/api/rooms/CreateMeeting?id="+this.roomAddress.split("@")[0]+
    "&subject="+this.meetingSubject+
    "&start="+meetingStartStr+
    "&end="+meetingEndStr, {})
    .map((res: Response) => res.json())
    .subscribe(data => {
      this.Close();
      this.OnAddMeeting.emit(new Meeting (this.meetingSubject, meetingStart, meetingEnd));
    })
    
  }

  Close():void{
    this.isAddMeeting.value = false;
  }
}

class DaySlot{
  hour
  minute
  label:String

  constructor(hour:Number, minute:Number){
    this.hour = hour;
    this.minute = minute;
    let h = hour.toString()
    if(h.length == 1){
      h = "0" + h;
    }
    let m = minute.toString()
    if(minute == 0){
      m = "00";
    }
    this.label = h+":"+m;
  }
}
