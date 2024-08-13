import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-tour-list',
  templateUrl: './tour-list.component.html',
  styleUrls: ['./tour-list.component.css']
})
export class TourListComponent implements OnInit {
  tours: any[] = [];
  apiUrl = 'http://localhost:5249/api/tours';
  successMessage: string | null = null;
  errorMessage: string | null = null;
  selectedStatus: string | null = null;

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.loadTours();
  }

  loadTours(): void {
    let url = this.apiUrl;
    if (this.selectedStatus) {
      url = `${this.apiUrl}?status=${this.selectedStatus}`;
    }

    this.http.get<any[]>(url).subscribe(data => {
      this.tours = data;
    }, error => {
      console.error('Error loading tours', error);
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
      this.loadTours();  // Reload tours after archiving
      setTimeout(() => this.successMessage = null, 3000);  // Reset message after 3 seconds
    }, error => {
      this.errorMessage = 'Error archiving tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000);  // Reset message after 3 seconds
      console.error('Error archiving tour', error);
    });
  }

  publishTour(tourId: number): void {
    this.http.put(`${this.apiUrl}/${tourId}/publish`, {}).subscribe(() => {
      this.successMessage = 'Tour successfully published!';
      this.errorMessage = null;
      this.loadTours();  // Reload tours after publishing
      setTimeout(() => this.successMessage = null, 3000);  // Reset message after 3 seconds
    }, error => {
      this.errorMessage = 'Error publishing tour. Please try again.';
      this.successMessage = null;
      setTimeout(() => this.errorMessage = null, 3000);  // Reset message after 3 seconds
      console.error('Error publishing tour', error);
    });
  }
}
