import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: any = {
    id: null,
    items: [],
    totalPrice: 0 // Dodano za ukupnu cenu
  };
  user: any = {
    id: null,
    username: '',
    email: ''
  };
  successMessage: string | null = null;
  errorMessage: string | null = null;
  apiUrl = 'http://localhost:5249/api/cart'; // Prilagodite prema vašem API

  constructor(private http: HttpClient, private userService: UserService) { }

  ngOnInit(): void {
    this.loadCart();
    this.loadUser();
  }

  loadCart(): void {
    const userId = this.userService.getUserIdFromToken();
    if (userId !== null) {
      this.http.get<{ id: number, items: any[], totalPrice: number }>(`${this.apiUrl}/user/${userId}`).subscribe(
        data => {
          console.log('Cart data:', data);
          this.cart.id = data.id;
          this.cart.items = data.items.map(item => ({
            id: item.id,
            quantity: item.quantity,
            tour: {
              id: item.tour.id,
              title: item.tour.title,
              price: item.tour.price
            }
          }));
          this.cart.totalPrice = data.totalPrice; // Postavite ukupnu cenu
        },
        error => {
          console.error('Error loading cart', error);
          this.cart.items = [];
          this.cart.totalPrice = 0; // Postavite ukupnu cenu na 0 u slučaju greške
        }
      );
    } else {
      console.error('User ID is null or undefined');
    }
  }

  loadUser(): void {
    const userId = this.userService.getUserIdFromToken();
    if (userId !== null) {
      this.http.get<{ id: number, username: string, email: string }>(`http://localhost:5249/api/auth/user/${userId}`).subscribe(
        data => {
          this.user = {
            id: data.id,
            username: data.username,
            email: data.email
          };
        },
        error => {
          console.error('Error loading user', error);
          this.user = {
            id: null,
            username: '',
            email: ''
          };
        }
      );
    } else {
      console.error('User ID is null or undefined');
    }
  }

  removeFromCart(tourId: number): void {
    const cartId = this.cart.id;
    if (!cartId || !tourId) {
      console.error('Cart ID or Tour ID not found');
      return;
    }

    this.userService.removeItemFromCart(cartId, tourId).subscribe(
      () => {
        this.successMessage = 'Item removed from cart!';
        this.loadCart();
        setTimeout(() => this.successMessage = null, 3000);
      },
      error => {
        console.error('Error removing item from cart:', error);
        this.errorMessage = 'Error removing item from cart.';
        setTimeout(() => this.errorMessage = null, 3000);
      }
    );
  }

  confirmPurchase(): void {
    const cartId = this.cart.id;
    if (!cartId) {
      console.error('Cart ID not found');
      return;
    }

    this.userService.confirmPurchase(cartId).subscribe(
      () => {
        this.successMessage = 'Purchase confirmed! A confirmation email will be sent.';
        this.cart.items = [];
        this.cart.totalPrice = 0; // Očistite ukupnu cenu nakon kupovine
        setTimeout(() => this.successMessage = null, 3000);
      },
      error => {
        this.errorMessage = 'Error confirming purchase. Please try again.';
        setTimeout(() => this.errorMessage = null, 3000);
      }
    );
  }
}
