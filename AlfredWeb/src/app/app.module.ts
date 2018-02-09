import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatList, MatListItem, MatListModule } from '@angular/material'
import { FlexLayoutModule } from '@angular/flex-layout'

import { AppComponent } from './app.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';
import { ScheduleViewComponent } from './components/schedule-view/schedule-view.component';


@NgModule({
  declarations: [
    AppComponent,
    CalendarViewComponent,
    ScheduleViewComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    MatListModule,
    FlexLayoutModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
