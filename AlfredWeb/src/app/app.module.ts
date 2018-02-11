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
import { LocationPickerComponent } from './components/location-picker/location-picker.component';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
import { AddMeetingCompComponent } from './components/add-meeting-comp/add-meeting-comp.component';
import { MatInputModule } from '@angular/material';
import { SpinnerService } from './loading-spinner.service';

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
    RoomSelectorComponent,
    AddMeetingCompComponent,
    LocationPickerComponent
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
    MatProgressSpinnerModule,
    BrowserAnimationsModule,
    MatInputModule,
    FlexLayoutModule,
    RouterModule.forRoot(
      appRoutes,
      { enableTracing: false } // <-- debugging purposes only
    )
  ],
  providers: [ SpinnerService ],
  bootstrap: [AppComponent]
})
export class AppModule { }
