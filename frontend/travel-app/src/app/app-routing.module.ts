import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';
import { LoginComponent } from './login/login.component'; 
import { TourMapComponent } from './tour-map/tour-map.component';
import { UserManagementComponent} from './user-management/user-management.component';
import { ReportProblemsComponent } from './report-problems/report-problems.component';
import { ReportedProblemsComponent } from './reported-problems/reported-problems.component';

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  { path: 'login', component: LoginComponent }, 
  { path: 'map', component: TourMapComponent },
  { path: '', redirectTo: '/register', pathMatch: 'full' },
  { path: 'user-management', component: UserManagementComponent },
  { path: 'report-issues', component: ReportProblemsComponent },
  { path: 'reported-problems', component: ReportedProblemsComponent } 
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
