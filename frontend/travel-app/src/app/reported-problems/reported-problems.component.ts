import { Component, OnInit } from '@angular/core';
import { ReportService } from '../services/report.service';

@Component({
  selector: 'app-reported-problems',
  templateUrl: './reported-problems.component.html',
  styleUrls: ['./reported-problems.component.css']
})
export class ReportedProblemsComponent implements OnInit {
  problems: Problem[] = [];;

  constructor(private reportService: ReportService) { }

  ngOnInit(): void {
    this.loadProblems();
  }

  loadProblems(): void {
    this.reportService.getProblems().subscribe(
      (data: any[]) => {
        this.problems = data;
      },
      error => {
        console.error('Error loading problems:', error);
      }
    );
  }

  changeStatus(problemId: number, newStatus: string): void {
    this.reportService.updateProblemStatus(problemId, newStatus).subscribe(
      () => {
        this.loadProblems(); // Reload problems after status change
      },
      error => {
        console.error('Error updating problem status:', error);
      }
    );
  }
  
}

export interface Problem {
  id: number;
  title: string;
  description: string;
  status: string; 
  createdAt: Date;
  resolvedAt?: Date;
  tourId: number;
  touristId: number;
}