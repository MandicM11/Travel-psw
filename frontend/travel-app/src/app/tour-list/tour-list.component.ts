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
    const userId = this.userService.getUserIdFromToken(); // Prilagodite na UserService
    if (userId !== null) {
      this.userService.getCartByUserId(userId).subscribe(
        cart => {
          if (cart && cart.length > 0) { // Proverite da li `cart` ima očekivanu strukturu
            this.cartId = cart[0].id; // Pretpostavlja se da je `cart` niz i da ima `id`
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
    this.http.get<any>(this.apiUrl).subscribe(data => {
      if (data && Array.isArray(data.$values)) {
        this.tours = data.$values; // Ekstrahuj niz
      } else {
        console.error('Expected an array for tours but received:', data);
        this.tours = []; // Podesi na prazno ako je greška
      }
    }, error => {
      console.error('Error loading tours', error);
      this.tours = []; // Podesi na prazno ako je greška
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
      this.loadTours(); // Ponovo učitaj ture nakon arhiviranja
      setTimeout(() => this.successMessage = null, 3000); // Resetuj poruku nakon 3 sekunde
    }, error => {
      this.errorMessage = 'Error archiving tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000); // Resetuj poruku nakon 3 sekunde
      console.error('Error archiving tour', error);
    });
  }

  publishTour(tourId: number): void {
    this.http.put(`${this.apiUrl}/${tourId}/publish`, {}).subscribe(() => {
      this.successMessage = 'Tour successfully published!';
      this.errorMessage = null;
      this.loadTours(); // Ponovo učitaj ture nakon objavljivanja
      setTimeout(() => this.successMessage = null, 3000); // Resetuj poruku nakon 3 sekunde
    }, error => {
      this.errorMessage = 'Error publishing tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000); // Resetuj poruku nakon 3 sekunde
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
