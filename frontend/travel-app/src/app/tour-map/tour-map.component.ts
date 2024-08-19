import { Component, Input, OnChanges, SimpleChanges } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';
import { PLATFORM_ID, Inject } from '@angular/core';
import L from 'leaflet';

@Component({
  selector: 'app-tour-map',
  templateUrl: './tour-map.component.html',
  styleUrls: ['./tour-map.component.css']
})
export class TourMapComponent implements OnChanges {
  @Input() keyPoints: any[] = [];
  map: L.Map | undefined;
  isBrowser: boolean;
  baseUrl: string = 'http://localhost:5249/uploads'; // Osnovni URL za slike
  imageUrls: { [key: string]: string } = {}; // Mapiranje za slike

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['keyPoints']) {
      const keyPoints = changes['keyPoints'].currentValue;
      console.log('Updated keyPoints:', keyPoints); 
      if (Array.isArray(keyPoints) && keyPoints.length > 0) {
        console.log('keyPoints:', keyPoints);  // Provera podataka
        if (this.isBrowser) {
          this.updateMap();
        }
      } else {
        console.log('Key points is either undefined, not an array, or empty');
      }
    }
  }

  initMap(): void {
    if (this.isBrowser) {
      import('leaflet').then(L => {
        this.map = L.map('map').setView([51.505, -0.09], 13);

        L.tileLayer('https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png', {
          maxZoom: 19,
        }).addTo(this.map);

        // Generiši URL-ove za slike u `keyPoints`
        this.keyPoints.forEach(kp => {
          if (kp.imageUrl) {
            this.imageUrls[kp.id] = `${this.baseUrl}/${kp.imageUrl}`;
          }
        });

        // Dodaj markere nakon inicijalizacije mape
        this.addMarkers();
      });
    }
  }

  addMarkers(): void {
    if (this.isBrowser && this.map) {
      import('leaflet').then(L => {
        this.map!.eachLayer((layer: L.Layer) => {
          if (layer instanceof L.Marker) {
            this.map!.removeLayer(layer);
          }
        });

        this.keyPoints.forEach(kp => {
          const icon = L.icon({
            iconUrl: this.imageUrls[kp.id] || 'assets/markers/marker-icon.png', // Koristi generisani URL za sliku
            iconSize: [38, 38], // Veličina ikone
            iconAnchor: [22, 94], // Tačka sidrenja ikone
            popupAnchor: [-3, -76] // Tačka sidrenja popup-a u odnosu na ikonu
          });

          L.marker([kp.latitude, kp.longitude], { icon })
            .addTo(this.map!)
            .bindPopup(`<b>${kp.title}</b><br>${kp.description}`);
        });
      });
    }
  }

  updateMap(): void {
    if (!this.map) {
      this.initMap();
    } else {
      this.addMarkers();
    }
  }
}
