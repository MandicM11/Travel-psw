import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service'; 

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cart: any; // Definišite tip ako imate konkretan interfejs za cart
  successMessage: string | null = null;
  errorMessage: string | null = null;
  apiUrl = 'http://localhost:5249/api/cart'; // Prilagodite prema vašem API

  constructor(private http: HttpClient, private userService: UserService) { }
  

  ngOnInit(): void {
    this.loadCart();
  }

  loadCart(): void {
    this.http.get<any>(`${this.apiUrl}/2`).subscribe(data => {
      // Check if items is an object with $values property
      if (data && data.items && Array.isArray(data.items.$values)) {
        this.cart = data;
        this.cart.items = data.items.$values; // Extract the array
      } else {
        console.error('Expected an array for cart items but received:', data.items);
        this.cart.items = []; // Default to empty array
      }
    }, error => {
      console.error('Error loading cart', error);
      this.cart.items = []; // Set to empty array on error
    });
  }

  removeFromCart(tourId: number): void {
    if (!this.cart || !this.cart.id) {
      console.error('Cart ID not found');
      return;
    }

    this.userService.removeItemFromCart(this.cart.id, tourId).subscribe(
      () => {
        this.successMessage = 'Item removed from cart!';
        this.loadCart(); // Ako imate ovu metodu za ponovno učitavanje korpe
        setTimeout(() => this.successMessage = null, 3000);
      },
      error => {
        this.errorMessage = 'Error removing item from cart. Please try again.';
        setTimeout(() => this.errorMessage = null, 3000);
      }
    );
  }

  confirmPurchase(): void {
    if (!this.cart || !this.cart.id) {
      console.error('Cart ID not found');
      return;
    }

    this.userService.confirmPurchase(this.cart.id).subscribe(
      () => {
        this.successMessage = 'Purchase confirmed! A confirmation email will be sent.';
        if (this.cart) this.cart.items = [];
        setTimeout(() => this.successMessage = null, 3000);
      },
      error => {
        this.errorMessage = 'Error confirming purchase. Please try again.';
        setTimeout(() => this.errorMessage = null, 3000);
      }
    );
  }
}
