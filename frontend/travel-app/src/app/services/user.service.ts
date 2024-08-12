import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private registerUrl = 'http://localhost:5249/api/Auth/register';
  private loginUrl = 'http://localhost:5249/api/Auth/login';

  constructor(private http: HttpClient) {}

  registerUser(user: any): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.registerUrl, user);
  }

  loginUser(credentials: { username: string, password: string }): Observable<any> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    return this.http.post(this.loginUrl, credentials, { headers });
  }

  logout(): void {
    localStorage.removeItem('token');
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
