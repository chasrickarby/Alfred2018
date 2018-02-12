import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddMeetingCompComponent } from './add-meeting-comp.component';

describe('AddMeetingCompComponent', () => {
  let component: AddMeetingCompComponent;
  let fixture: ComponentFixture<AddMeetingCompComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddMeetingCompComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddMeetingCompComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
