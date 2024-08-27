import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { UserService } from '../services/user.service';
import { Observable, of } from 'rxjs';
import { map, tap, catchError } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class RoleGuard implements CanActivate {

  constructor(private userService: UserService, private router: Router) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    const expectedRoles = route.data['expectedRole'] as string | string[];
    return this.userService.getUserRole().pipe(
      map(role => {
        if (role === null) {
          return false;  // Ako nije uloga, odbij pristup
        }
        if (Array.isArray(expectedRoles)) {
          return expectedRoles.includes(role);  // Proverava da li je role u nizu očekivanih uloga
        } else {
          return role === expectedRoles;  // Proverava da li je role očekivana uloga
        }
      }),
      catchError(error => {
        console.error('Error checking user role', error);
        this.router.navigate(['/access-denied']);
        return of(false);
      }),
      tap(canActivate => {
        if (!canActivate) {
          console.error('Access denied - Role not allowed');
        }
      })
    );
  }
}
