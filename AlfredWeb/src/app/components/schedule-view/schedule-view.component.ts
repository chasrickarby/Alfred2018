import { Component, ChangeDetectionStrategy } from '@angular/core';
import { CalendarEvent } from 'angular-calendar';

@Component({
  selector: 'schedule-view',
  changeDetection: ChangeDetectionStrategy.OnPush,
  templateUrl: './schedule-view.component.html',
})

export class ScheduleViewComponent {
  view: string = 'day';

  viewDate: Date = new Date();

  events: CalendarEvent[] = [];

}
