import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatList, MatListItem, MatListModule, MatSidenavModule, MatCheckboxModule } from '@angular/material'


import { AppComponent } from './app.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';
import { LocationSidebarComponent } from './components/location-sidebar/location-sidebar.component';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';


@NgModule({
  declarations: [
    AppComponent,
    CalendarViewComponent,
    LocationSidebarComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    MatListModule,
    MatSidenavModule,
    MatCheckboxModule,
    BrowserAnimationsModule
  ],
  exports: [
    MatCheckboxModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
