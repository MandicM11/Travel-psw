import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AuthorProblemsComponent } from './author-problems.component';

describe('AuthorProblemsComponent', () => {
  let component: AuthorProblemsComponent;
  let fixture: ComponentFixture<AuthorProblemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [AuthorProblemsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AuthorProblemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
