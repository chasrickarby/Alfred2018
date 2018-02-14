import { Component, OnInit, Input } from '@angular/core';
import { MatGridListModule} from '@angular/material/grid-list';
import { MatList, MatListItem } from '@angular/material';
import { Http, Response, Headers } from '@angular/http'
import { ActivatedRoute } from '@angular/router';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddMeetingCompComponent } from '../add-meeting-comp/add-meeting-comp.component';

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
    
  constructor(private _http: Http, private route: ActivatedRoute ) {
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
}

class Meeting{
  name:String;
  start:Date;
  end:Date;
  color:String;
  static colors:Array<String> = [
    "#2196F3",
    "#4CAF50",
    "#FFC107",
    "#FF9800",
    "#8BC34A",
    "#29B6F6",
    "#7CB342",
    "#F9A825",
    "#F48FB1",
    "#673AB7"
  ];

  constructor(name, start, end){
    this.name = name;
    this.start = start;
    this.end = end;
    this.color = this.PeekColor();
  }
  isInMeeting(d:Date):Boolean{
    return d >= this.start && d < this.end;
  }
  PeekColor():String{
    return Meeting.colors[Math.floor(Math.random()*Meeting.colors.length)];
  }
  // Get meeting duration in 
  GetDuration():number{
    return (this.end.getTime() - this.start.getTime())/60000;
  }
}

class TimeSlot{
  date:Date;
  private meeting:Meeting;
  private cssClasses = ["slot15min", "slot30min", "slot45min", "slot60min"];

  constructor (date:Date, meeting:Meeting=null){
    this.date=date;
    this.meeting=meeting;
  }
  isFree():Boolean{
    return this.meeting==null;
  }
  GetDuration():number{
    if(this.meeting){
      return this.meeting.GetDuration();
    }
    return 15;
  }
  GetColor():String{
    if(this.meeting){
      return this.meeting.color;
    }
    return "";
  }
  GetLabel():String{
    if(this.meeting){
      return this.meeting.name;
    }
    return "";
  }
  GetCSSClass():String{
    if(this.meeting){
      return "meeting"
    }else{
      return this.cssClasses[this.date.getMinutes()/15];
    }
  }
}

class Day{
  date:Date;
  start:Date;
  end:Date;
  public meetings:Array<Meeting>
  public timeSlots:Array<TimeSlot>;

  static daysOfWeek:Array<String> = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
  constructor(d){
    this.date = d;
    this.date.setHours(0,0,0,0);
    this.start = new Date(d);
    this.start.setHours(6);
    this.end = new Date(d);
    this.end.setHours(21);
    this.timeSlots = new Array<TimeSlot>();
    this.meetings = new Array<Meeting>();
  }

  GetDay():String{
    return Day.daysOfWeek[this.date.getDay()];
  }
  GetDate():String{
    return this.date.getDate().toString();
  }
  AddMeeting(meeting:Meeting):void{
    this.meetings.push(meeting);
  }

  GetTimeSlots(){
    let slotDuration = 15; // Minutes
    for (let d = this.start; d <= this.end; d.setMinutes(d.getMinutes() + slotDuration)){
      let meeting = null;
      slotDuration = 15;
      for(let i = 0; i < this.meetings.length; i++){
        if(this.meetings[i].isInMeeting(d)){
          meeting = this.meetings[i];
          slotDuration = meeting.GetDuration();
          break;
        }
      }
      this.timeSlots.push(new TimeSlot(new Date(d), meeting));
    }
    return this.timeSlots;
  }
}

class Week {
  firstDay:Date;
  days:Array<Day>;

  constructor(date, type){
    if (!date)
      date = new Date();

    var weekLength = 1;
    this.firstDay = date;
    if (type=="week"){
      var weekLength = 7;
      this.firstDay = this.getFirstDayOfWeek(date);
    }
    
    this.days = new Array<Day>();
    for(let i = 0; i < weekLength; i++){
      let iDay = this.getNextDay(this.firstDay, i);
      this.days.push (new Day(iDay));
    }
  }

  getFirstDayOfWeek (d):Date{
    let result = new Date();
    result.setDate(d.getDate()-d.getDay()+1);
    return result;
  }
  getNextDay(d, i):Date{
    let result = new Date();
    result.setDate(d.getDate()+i);
    return result;
  }
}
