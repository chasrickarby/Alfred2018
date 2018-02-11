import { Component, OnInit, Input } from '@angular/core';
import {MatSelectModule} from '@angular/material/select';
import {MatButtonModule} from '@angular/material/button';
import {MatInputModule} from '@angular/material/input';
import { Http, Response, Headers } from '@angular/http';

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
    let meetingStartStr = meetingStart.getFullYear()+"-"+(meetingStart.getMonth()+1)+"-"+meetingStart.getDate()+"T"+meetingStart.getHours()+":"+meetingStart.getMinutes()+":00";

    let meetingEnd = new Date(this.startDate);
    meetingEnd.setHours(this.selectedEndTime.hour, this.selectedEndTime.minute,0,0);
    let meetingEndStr = meetingEnd.getFullYear()+"-"+(meetingEnd.getMonth()+1)+"-"+meetingEnd.getDate()+"T"+meetingEnd.getHours()+":"+meetingEnd.getMinutes()+":00";
    console.log(this.roomAddress);
    console.log(meetingStartStr);
    //console.log(meetingEndStr);
    
    //2018-02-06T00:00:00

    
    this._http.post(this.host + "/RestServer/api/rooms/CreateMeeting?roomAddress="+this.roomAddress+
    "&subject="+this.meetingSubject+
    "&start="+meetingStart.toISOString()+
    "&end="+meetingEnd.toISOString(), {})
    .map((res: Response) => res.json())
    .subscribe(data => {
      this.Close();
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
