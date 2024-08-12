import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-add-tour',
  templateUrl: './add-tour.component.html',
  styleUrls: ['./add-tour.component.css']
})
export class AddTourComponent {
  tour: any = {
    title: '',
    description: '',
    difficulty: '',
    category: '',
    price: '',
    status: 'draft'
  };
  apiUrl = 'http://localhost:5249/api/tours';

  constructor(private http: HttpClient, private router: Router) { }

  addTour(): void {
    this.http.post(this.apiUrl, this.tour).subscribe((response: any) => {
      this.router.navigate([`/tours/${response.id}`]);
    });
  }
}
