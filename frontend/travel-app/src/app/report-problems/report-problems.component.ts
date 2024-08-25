import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ReportService } from '../services/report.service'; // Proverite putanju do vašeg servisa

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

  constructor(private reportService: ReportService) { }

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
