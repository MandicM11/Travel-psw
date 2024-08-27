import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ReportService } from '../services/report.service';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-report-problems',
  templateUrl: './report-problems.component.html',
  styleUrls: ['./report-problems.component.css']
})
export class ReportProblemsComponent {
  report = {
    touristId: 0,
    tourId: 0,
    title: '',
    description: ''
  };

  constructor(private route: ActivatedRoute, private router: Router,private reportService: ReportService) {
    // Preuzmite parametre iz URL-a
    this.route.queryParams.subscribe(params => {
      this.report.tourId = +params['tourId'];
      this.report.touristId = +params['touristId'];
    });
  }

  onSubmit(form: NgForm): void {
    if (form.valid) {
      this.reportService.reportProblem(this.report).subscribe(
        response => {
          console.log('Report submitted:', response);
          // Prikazivanje poruke o uspehu ili preusmeravanje
        },
        error => {
          console.error('Error submitting report:', error);
          // Obrada greške
        }
      );
    }
  }
}
