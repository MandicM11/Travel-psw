import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReportedProblemsComponent } from './reported-problems.component';

describe('ReportedProblemsComponent', () => {
  let component: ReportedProblemsComponent;
  let fixture: ComponentFixture<ReportedProblemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ReportedProblemsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReportedProblemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
