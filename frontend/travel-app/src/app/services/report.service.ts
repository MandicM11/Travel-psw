import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'http://localhost:5249/api/problem'; // API URL for reporting

  constructor(private http: HttpClient) {}

  reportProblem(report: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/report`, report);
  }
}
