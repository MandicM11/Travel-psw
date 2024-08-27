import { Component, OnInit } from '@angular/core';
import { UserService } from '../services/user.service'; // Servis koji koristi backend API
import { ReportService } from '../services/report.service'; // Servis za rad sa problemima

@Component({
  selector: 'app-author-problems',
  templateUrl: './author-problems.component.html',
  styleUrls: ['./author-problems.component.css']
})
export class AuthorProblemsComponent implements OnInit {
  problems: any[] = [];

  constructor(private reportService: ReportService, private userService: UserService) {}

  ngOnInit(): void {
    const authorId = this.userService.getUserIdFromToken(); // Pretpostavimo da je ID autora u tokenu
    if (authorId) {
      this.reportService.getProblemsByAuthorId(authorId).subscribe((problems: any[]) => {
        this.problems = problems;
      });
    }
  }

  fixTour(problemId: number): void {
    // Logika za ispravljanje ture (moguće preusmeravanje na formu za izmenu ture)
    console.log(`Fix tour for problem ID: ${problemId}`);
  }

  discardProblem(problemId: number): void {
    this.reportService.discardProblem(problemId).subscribe(() => {
      this.problems = this.problems.filter(problem => problem.id !== problemId);
    });
  }
}
