import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Problem } from '../reported-problems/reported-problems.component';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = 'http://localhost:5249/api/problem'; // API URL for reporting
  private adminUrl = 'http://localhost:5249/api/admin';

  constructor(private http: HttpClient) {}

  reportProblem(report: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/report`, report);
  }

  getProblems(): Observable<any[]> {
    return this.http.get<Problem[]>(`${this.apiUrl}/problems`);
  }

  updateProblemStatus(problemId: number, status: string): Observable<any> {
    return this.http.put<Problem>(`${this.apiUrl}/problems/${problemId}/status`, { status });
  }
}
