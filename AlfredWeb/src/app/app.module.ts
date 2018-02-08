import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatList, MatListItem, MatListModule } from '@angular/material'


import { AppComponent } from './app.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';


@NgModule({
  declarations: [
    AppComponent,
    CalendarViewComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    MatListModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
