import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../services/user.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-purchased-tours',
  templateUrl: './purchased-tours.component.html',
  styleUrls: ['./purchased-tours.component.css']
})
export class PurchasedToursComponent implements OnInit {
  purchasedTours: any[] = [];
  userId: number | null = null;

  constructor(private http: HttpClient, private userService: UserService, private router: Router) { }

  ngOnInit(): void {
    this.userId = this.userService.getUserIdFromToken();
    if (this.userId !== null) {
      this.loadPurchasedTours();
    } else {
      console.error('User ID is null');
    }
  }

  loadPurchasedTours(): void {
    if (this.userId !== null) {
      this.http.get<any[]>(`http://localhost:5249/api/tours/user/${this.userId}`).subscribe(
        data => {
          this.purchasedTours = data;
        },
        error => {
          console.error('Error loading purchased tours', error);
        }
      );
    } else {
      console.error('User ID is null');
    }
  }

  reportIssue(tourId: number): void {
    if (this.userId !== null) {
      this.router.navigate(['/report-issues'], { queryParams: { tourId: tourId, touristId: this.userId } });
    } else {
      console.error('User ID is null');
    }
  }
}
