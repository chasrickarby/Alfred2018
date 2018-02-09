import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpModule } from '@angular/http';
import { MatList, MatListItem, MatListModule, MatButtonModule, MatSelectModule, MatOptionModule, MatSidenavModule } from '@angular/material'
import { FlexLayoutModule } from '@angular/flex-layout'
import { AppComponent } from './app.component';
import { CalendarViewComponent } from './components/calendar-view/calendar-view.component';
import { RoomDetailComponent } from './components/room-detail/room-detail.component';
import { RouterModule, Routes, Router } from '@angular/router';
import { ScheduleViewComponent } from './components/schedule-view/schedule-view.component';
import { LocationSidebarComponent } from './components/location-sidebar/location-sidebar.component';
import { RoomSelectorComponent } from './components/room-selector/room-selector.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';


const appRoutes: Routes = [
  {
    path: '',
    redirectTo: 'rooms',
    pathMatch: 'full'
  },
  {path: 'rooms', component: CalendarViewComponent},
  {path: 'rooms', component: RoomSelectorComponent},
  {path: 'rooms/:id', component: ScheduleViewComponent}
]

@NgModule({
  declarations: [
    AppComponent,
    CalendarViewComponent,
    LocationSidebarComponent,
    RoomDetailComponent,
    ScheduleViewComponent,
    RoomSelectorComponent
  ],
  imports: [
    HttpModule,
    BrowserModule,
    MatListModule,
    MatSidenavModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatOptionModule,
    MatSelectModule,
    BrowserAnimationsModule,
    FlexLayoutModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: true } // <-- debugging purposes only
    )
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
