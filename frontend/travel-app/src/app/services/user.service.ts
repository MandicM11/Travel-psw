  import { Injectable } from '@angular/core';
  import { HttpClient, HttpHeaders } from '@angular/common/http';
  import { Observable, of } from 'rxjs';
  import { map, tap, catchError } from 'rxjs/operators';
  @Injectable({
    providedIn: 'root'
  })
  export class UserService {
    private registerUrl = 'http://localhost:5249/api/Auth/register';
    private loginUrl = 'http://localhost:5249/api/Auth/login';
    private cartUrl = 'http://localhost:5249/api/cart';
    private adminUrl = 'http://localhost:5249/api/admin';
    private userUrl = 'http://localhost:5249/api/users';

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
      localStorage.removeItem('cartId');
    }

    isLoggedIn(): boolean {
      return !!localStorage.getItem('token');
    }

    getUserIdFromToken(): number | null {
      const token = localStorage.getItem('token');
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          console.log('Token payload:', payload); // Ispisuje celokupan payload za debagovanje
          return parseInt(payload.nameid, 10); // Koristi 'nameid' iz payload-a
        } catch (error) {
          console.error('Error decoding token', error);
        }
      }
      return null;
    }
    
    

    createCart(userId: number): Observable<any> {
      return this.http.post<any>(this.cartUrl, { userId });
    }

    public getCartByUserId(userId: number): Observable<any[]> {
      return this.http.get<any[]>(`${this.cartUrl}/user/${ userId }`);
  }
  
    removeItemFromCart(cartId: number, tourId: number): Observable<any> {
      return this.http.delete(`${this.cartUrl}/${cartId}/items/${tourId}`);
    }

    confirmPurchase(cartId: number): Observable<any> {
      return this.http.post(`${this.cartUrl}/${cartId}/confirm`, {});
    }

    addItemToCart(cartId: number, tourId: number): Observable<any> {
      return this.http.post(`${this.cartUrl}/${cartId}/items`, { tourId });
    }

    getUserById(userId: number): Observable<any> {
      return this.http.get<any>(`${this.loginUrl}/user/${userId}`);
    }

    blockUser(userId: number): Observable<any> {
      return this.http.post<any>(`${this.adminUrl}/block-user/${userId}`, {});
    }
  
    unblockUser(userId: number): Observable<any> {
      return this.http.post<any>(`${this.adminUrl}/unblock-user/${userId}`, {});
    }
    getUsers(): Observable<any[]> {
      return this.http.get<any[]>(`${this.userUrl}/allusers`);
    }
    getUserRole(): Observable<string | null> {
      const token = localStorage.getItem('token');
      if (token) {
        try {
          const payload = JSON.parse(atob(token.split('.')[1]));
          console.log('Token payload:', payload); // Ispisuje celokupan payload za debagovanje
          return of(payload.role); // Koristi 'role' iz payload-a
        } catch (error) {
          console.error('Error decoding token', error);
        }
      }
      return of(null); // Ako nema tokena, vratite null
    }

  }
