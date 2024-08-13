// tour-map.component.ts
import { Component, OnInit, Input } from '@angular/core';
import * as L from 'leaflet';
import { TourService } from '../services/tour.service';

@Component({
  selector: 'app-tour-map',
  templateUrl: './tour-map.component.html',
  styleUrls: ['./tour-map.component.css']
})
export class TourMapComponent implements OnInit {
  @Input() tourId: number | null = null;
  map: any;
  keyPoints: any[] = [];

  constructor(private tourService: TourService) {}

  ngOnInit(): void {
    if (this.tourId !== null) {
      this.tourService.getKeyPoints(this.tourId).subscribe(keyPoints => {
        this.keyPoints = keyPoints;
        this.loadMap();
      });
    }
  }

  initMap(): void {
    this.map = L.map('map').setView([51.505, -0.09], 13); // Podesite početne koordinate i zoom nivo

    L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
      maxZoom: 19,
    }).addTo(this.map);
  }

  loadMap(): void {
    if (!this.map) {
      this.initMap();
    }

    this.keyPoints.forEach(point => {
      L.marker([point.latitude, point.longitude])
        .bindPopup(`<b>${point.title}</b><br>${point.description}`)
        .addTo(this.map);
    });
  }
}
