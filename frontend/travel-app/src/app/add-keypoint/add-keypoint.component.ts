import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-add-keypoint',
  templateUrl: './add-keypoint.component.html',
  styleUrls: ['./add-keypoint.component.css']
})
export class AddKeyPointComponent {
  keyPoint: any = {
    title: '',
    description: '',
    latitude: '',
    longitude: '',
    imageUrl: ''
  };
  apiUrl = 'http://localhost:5249/api/tours';

  constructor(private http: HttpClient, private route: ActivatedRoute, private router: Router) { }

  addKeyPoint(): void {
    const tourId = this.route.snapshot.paramMap.get('id');
    this.http.post(`${this.apiUrl}/${tourId}/keypoints`, this.keyPoint).subscribe(() => {
      this.router.navigate([`/tours/${tourId}`]);
    });
  }
}
