import { Component, OnInit, Input } from '@angular/core';
import {MatGridListModule} from '@angular/material/grid-list';
import { MatList, MatListItem } from '@angular/material';
import { Http, Response, Headers } from '@angular/http';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'schedule-view',
  templateUrl: './schedule-view.component.html',
  styleUrls: ['./schedule-view.component.css']
})


export class ScheduleViewComponent implements OnInit {
  startDate:Date = null;
  host = 'http://localhost';
  roomAddress = '';

  timeSlots:Array<String>=new Array<String>();
  startHour = 6;
  endHour = 18;
  week = null;

  constructor(private _http: Http, private route: ActivatedRoute ) {
    this.route.params.subscribe( params => {
      console.log(params)
      this.roomAddress = params.id;
     });
    if (!this.startDate)
      this.startDate = new Date();

for (let h = this.startHour; h <= this.endHour; h++)
    {
    this.timeSlots.push(h+":00");
    }

    this.week = new Week(this.startDate, "day");
  }
  
  ngOnInit() {
  }

  getRoomSchedule(roomAddress){
    var roomInfo;
    this._http.get(this.host + '/RestServer/api/rooms?id=' + roomAddress)
                .map((res: Response) => res.json())
                .subscribe(data => {
                  roomInfo = data;
                  console.log(roomInfo);
                })
  }

}

class Meeting{
  name:String;
  start:Date;
  end:Date;

  constructor(name, start, end){
    this.name = name;
    this.start = start;
    this.end = end;
  }
}

class TimeSlot{
  date:Date;
  meeting:Meeting;

  constructor (date:Date, meeting:Meeting=null){
    this.date=date;
    this.meeting=meeting;
  }
  isFree():Boolean{
    return this.meeting==null;
  }
}

class Day{
  date:Date;
  start:Date;
  end:Date;
  meetings:Array<Meeting>;
  timeSlots:Array<TimeSlot>;
  static daysOfWeek:Array<String> = ["Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday"];
  constructor(d){
    this.date = d;
    this.date.setHours(0,0,0,0);
    this.start = new Date(d);
    this.start.setHours(6);
    this.end = new Date(d);
    this.end.setHours(20);
    this.timeSlots = this.GetTimeSlots();
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
    console.log("Create day");
    let timeSlots = new Array<TimeSlot>();
    console.log(this.start);
    console.log(this.end);

    for (let d = this.start; d <= this.end; d.setMinutes(d.getMinutes() + 15))
    {
    timeSlots.push(new TimeSlot(this.date));
    }
    return timeSlots;
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
