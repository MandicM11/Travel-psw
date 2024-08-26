import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS  } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { RouterModule, Routes } from '@angular/router';  // Import RouterModule
import { AppComponent } from './app.component';
import { RegisterComponent } from './register/register.component';
import { UserService } from './services/user.service';
import { provideHttpClient, withFetch } from '@angular/common/http';
import { NavbarComponent } from './navbar/navbar.component';
import { ReactiveFormsModule } from '@angular/forms';
import { LoginComponent } from './login/login.component';
import { TourDetailComponent } from './tour-detail/tour-detail.component';
import { AddTourComponent } from './add-tour/add-tour.component';
import { AddKeyPointComponent } from './add-keypoint/add-keypoint.component';
import { TourListComponent } from './tour-list/tour-list.component';
import { TourMapComponent } from './tour-map/tour-map.component';
import { CartComponent } from './cart/cart.component';
import { ReportProblemsComponent } from './report-problems/report-problems.component';
import { UserManagementComponent } from './user-management/user-management.component';
import { ReportedProblemsComponent } from './reported-problems/reported-problems.component';



const routes: Routes = [
  
  { path: 'register', component: RegisterComponent },
  { path: 'tours', component: TourListComponent },
  { path: 'login', component: LoginComponent },
  { path: 'tours/:id', component: TourDetailComponent },
  { path: 'add-tour', component: AddTourComponent },
  { path: 'tours/:id/add-keypoint', component: AddKeyPointComponent },
  { path: 'cart', component: CartComponent },
  { path: '', redirectTo: '/login', pathMatch: 'full' },
  { path: 'map', component: TourMapComponent },
  { path: 'user-management', component: UserManagementComponent },
  { path: 'report-issues', component: ReportProblemsComponent },
  { path: 'reported-problems', component: ReportedProblemsComponent }
  
];

@NgModule({
  declarations: [
    AppComponent,
    RegisterComponent,
    NavbarComponent,
    LoginComponent,
    TourDetailComponent,
    AddTourComponent,
    AddKeyPointComponent,
    TourListComponent,
    TourMapComponent,
    CartComponent,
    ReportProblemsComponent,
    UserManagementComponent,
    ReportedProblemsComponent
    
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    ReactiveFormsModule,
    RouterModule.forRoot(routes)  // Dodaj RouterModule ovde
  ],
  providers: [
    UserService,
    provideHttpClient(withFetch()),

  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
