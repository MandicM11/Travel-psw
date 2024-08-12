import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class TourService {
  private baseUrl = 'http://localhost:5249/api/tours';

  constructor(private http: HttpClient) { }

  getTour(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/${id}`);
  }

  addKeyPoint(keyPoint: FormData): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/${keyPoint.get('tourId')}/keypoints`, keyPoint);
  }
}
