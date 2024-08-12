import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-tour-detail',
  templateUrl: './tour-detail.component.html',
  styleUrls: ['./tour-detail.component.css']
})
export class TourDetailComponent implements OnInit {
  tour: any = null;
  showForm = false;
  newKeyPoint: any = {
    title: '',
    description: '',
    latitude: 0,
    longitude: 0,
    image: null, // Add image field
    tourId: null
  };

  constructor(private tourService: TourService, private route: ActivatedRoute) {}

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      this.tourService.getTour(id).subscribe(tour => {
        this.tour = tour;
        this.newKeyPoint.tourId = tour.id; // Set the tourId for the new key point
      });
    });
  }

  showAddKeyPointForm(): void {
    this.showForm = !this.showForm;
  }

  onImageSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.newKeyPoint.image = file;
    }
  }

  addKeyPoint(): void {
    if (this.tour && this.newKeyPoint.title && this.newKeyPoint.description) {
      const formData = new FormData();
      formData.append('title', this.newKeyPoint.title);
      formData.append('description', this.newKeyPoint.description);
      formData.append('latitude', this.newKeyPoint.latitude.toString());
      formData.append('longitude', this.newKeyPoint.longitude.toString());
      if (this.newKeyPoint.image) {
        formData.append('image', this.newKeyPoint.image);
      }
      formData.append('tourId', this.newKeyPoint.tourId.toString());

      this.tourService.addKeyPoint(formData).subscribe(keyPoint => {
        if (this.tour) {
          this.tour.keyPoints.push(keyPoint);
          this.showForm = false;
          this.newKeyPoint = {
            title: '',
            description: '',
            latitude: 0,
            longitude: 0,
            image: null,
            tourId: null
          };
        }
      });
    }
  }
}
