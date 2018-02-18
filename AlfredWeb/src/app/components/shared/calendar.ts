// ********************************************************************************
export class Meeting{
    name:String;
    start:Date;
    end:Date;
    color:String;
    static colorIteration = 0;
    static colors:Array<String> = [
      "#897E7E",
      "#3D831E"
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
      let color:String = Meeting.colors[Meeting.colorIteration];
      Meeting.colorIteration += 1;
      if(Meeting.colorIteration >= Meeting.colors.length){
        Meeting.colorIteration = Meeting.colorIteration - Meeting.colors.length;
      }
      return color;
    }
    PeekColorRand():String{
      return Meeting.colors[Math.floor(Math.random()*Meeting.colors.length)];
    }
    // Get meeting duration in 
    GetDuration():number{
      return (this.end.getTime() - this.start.getTime())/60000;
    }
  }

// ********************************************************************************
export class TimeSlot{
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
  
// ********************************************************************************
export class Day{
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
      console.log("GetTimeSlots");
      this.timeSlots = [];
      let slotDuration = 15; // Minutes
      console.log(this.start);
      console.log(this.end);
      for (let d = new Date(this.start); d <= this.end; d.setMinutes(d.getMinutes() + slotDuration)){
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
      console.log(this.timeSlots);
      return this.timeSlots;
    }
}

// ********************************************************************************
export class Week {
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