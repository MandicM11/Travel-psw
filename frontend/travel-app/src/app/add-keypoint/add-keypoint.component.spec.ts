import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AddKeypointComponent } from './add-keypoint.component';

describe('AddKeypointComponent', () => {
  let component: AddKeypointComponent;
  let fixture: ComponentFixture<AddKeypointComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AddKeypointComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AddKeypointComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
