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

  constructor(private http: HttpClient) { }

  ngOnInit(): void {
    this.http.get<any[]>(this.apiUrl).subscribe(data => {
      this.tours = data;
    });
  }
}
