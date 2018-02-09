import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'location-sidebar',
  templateUrl: './location-sidebar.component.html',
  styleUrls: ['./location-sidebar.component.css']
})
export class LocationSidebarComponent implements OnInit {
  @Input() roomList: any;

  constructor() { }

  ngOnInit() {
  }
}
