import { Component, OnInit } from '@angular/core';
import 'rxjs/add/observable/of';
import 'rxjs/add/operator/map';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import 'rxjs/add/operator/switchMap';

@Component({
  selector: 'room-detail',
  templateUrl: './room-detail.component.html',
  styleUrls: ['./room-detail.component.css']
})
export class RoomDetailComponent implements OnInit {

  constructor(
    private route: ActivatedRoute,
    private router: Router) 
    { 
    console.log("CTOR: room detail view.")
    this.route.params.subscribe( params => console.log(params) );
  }

  ngOnInit() {
    // this.room$ = this.route.paramMap
    // .switchMap((params: ParamMap) =>
    //   this.service.getHero(params.get('id')));
  }

  gotoRooms(){
    this.router.navigate(['/rooms']);
  }

}

@Injectable()
export class HeroService {
  getHeroes() { return Observable.of(RoomDetailComponent); }
}
