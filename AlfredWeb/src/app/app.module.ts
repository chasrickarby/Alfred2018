import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatList, MatListItem, MatListModule } from '@angular/material'
import { CommonModule } from '@angular/common';
import { DemoUtilsModule } from './module'

import { AppComponent } from './app.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';
import { CalendarModule } from 'angular-calendar';
import { ScheduleViewComponent } from './components/schedule-view/schedule-view.component'

@NgModule({
  declarations: [
    AppComponent,
    CalendarViewComponent,
    ScheduleViewComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    CommonModule,
    MatListModule,
    CalendarModule.forRoot(),
    DemoUtilsModule
  ],
  exports: [ScheduleViewComponent],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
