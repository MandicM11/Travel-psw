import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RegisterComponent } from './register/register.component';  // Proveri putanju

const routes: Routes = [
  { path: 'register', component: RegisterComponent },
  // Dodaj druge rute ovde
  { path: '', redirectTo: '/register', pathMatch: 'full' } // Podrazumevana ruta
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
