import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-tour-list',
  templateUrl: './tour-list.component.html',
  styleUrls: ['./tour-list.component.css']
})
export class TourListComponent implements OnInit {
  tours: any[] = [];
  apiUrl = 'http://localhost:5249/api/tours';
  cartId: number | null = null;
  successMessage: string | null = null;
  errorMessage: string | null = null;
  selectedStatus: string | null = null;

  constructor(private http: HttpClient, private userService: UserService) { }

  ngOnInit(): void {
    this.loadTours();
    this.loadCartId();
  }

  loadCartId(): void {
    const userId = this.userService.getUserIdFromToken();
    if (userId !== null) {
      console.log('User ID:', userId);
      this.userService.getCartByUserId(userId).subscribe(
        (cart: any) => {
          console.log('API response:', cart);
          // Pristupanje stavkama unutar korpe
          if (cart && cart.id) {
            this.cartId = cart.id; // Postavljanje ID-a korpe
          } else {
            console.error('No cart found for user');
          }
        },
        error => {
          console.error('Error loading cart', error);
        }
      );
    } else {
      console.error('User ID is null or undefined');
    }
  }

  loadTours(): void {
    let url = this.apiUrl;
  
    if (this.selectedStatus && this.selectedStatus !== '') {
      url += `?status=${this.selectedStatus}`;
    }
  
    this.http.get<any[]>(url).subscribe(data => {
      if (Array.isArray(data)) {
        this.tours = data;
      } else {
        console.error('Expected an array for tours but received:', data);
        this.tours = [];
      }
    }, error => {
      console.error('Error loading tours', error);
      this.tours = [];
    });
  }
  
  

  onStatusChange(event: Event): void {
    const target = event.target as HTMLSelectElement;
    this.selectedStatus = target.value;
    this.loadTours();
  }

  archiveTour(tourId: number): void {
    this.http.put(`${this.apiUrl}/${tourId}/archive`, {}).subscribe(() => {
      this.successMessage = 'Tour successfully archived!';
      this.errorMessage = null;
      this.loadTours();
      setTimeout(() => this.successMessage = null, 3000);
    }, error => {
      this.errorMessage = 'Error archiving tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000);
      console.error('Error archiving tour', error);
    });
  }

  publishTour(tourId: number): void {
    this.http.put(`${this.apiUrl}/${tourId}/publish`, {}).subscribe(() => {
      this.successMessage = 'Tour successfully published!';
      this.errorMessage = null;
      this.loadTours();
      setTimeout(() => this.successMessage = null, 3000);
    }, error => {
      this.errorMessage = 'Error publishing tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000);
      console.error('Error publishing tour', error);
    });
  }

  addToCart(tourId: number): void {
    if (this.cartId === null) {
      console.error('Cart ID not found');
      return;
    }

    this.userService.addItemToCart(this.cartId, tourId).subscribe(
      () => {
        this.successMessage = 'Tour successfully added to cart!';
        this.errorMessage = null;
        setTimeout(() => this.successMessage = null, 3000);
      },
      error => {
        this.errorMessage = 'Error adding tour to cart. Please try again.';
        this.successMessage = null;
        setTimeout(() => this.errorMessage = null, 3000);
        console.error('Error adding tour to cart', error);
      }
    );
  }
}
