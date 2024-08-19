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
    image: null,
    tourId: null
  };

  constructor(private tourService: TourService, private route: ActivatedRoute) {}

  formatDecimal(value: number): string {
    return value.toString().replace('.', ',');
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const id = +params['id'];
      this.tourService.getTour(id).subscribe(tour => {
        this.tour = tour;
        if (this.tour) {
          console.log('Tour received:', this.tour);
          if (Array.isArray(this.tour.keyPoints)) {
            console.log('Key Points:', this.tour.keyPoints);
          } else {
            console.log('No keyPoints in tour or not an array');
          }
          this.newKeyPoint.tourId = tour.id;
        }
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
      const formattedLatitude = this.formatDecimal(this.newKeyPoint.latitude);
      const formattedLongitude = this.formatDecimal(this.newKeyPoint.longitude);

      const formData = new FormData();
      formData.append('title', this.newKeyPoint.title);
      formData.append('description', this.newKeyPoint.description);
      formData.append('latitude', formattedLatitude);
      formData.append('longitude', formattedLongitude);
      if (this.newKeyPoint.image) {
        formData.append('image', this.newKeyPoint.image);
      }
      formData.append('tourId', this.newKeyPoint.tourId.toString());

      if (this.newKeyPoint.tourId !== null && this.newKeyPoint.tourId !== undefined) {
        formData.append('tourId', this.newKeyPoint.tourId.toString());
      } else {
        console.error('Tour ID is missing');
        return;
      }

      this.tourService.addKeyPoint(formData).subscribe(keyPoint => {
        if (this.tour) {
          if (!this.tour.keyPoints) {
            this.tour.keyPoints = [];
          }
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

  getImageUrl(imageUrl: string): string {
    return `http://localhost:5249/uploads/${imageUrl}`;
  }
}
