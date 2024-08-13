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

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {
    this.isBrowser = isPlatformBrowser(this.platformId);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['keyPoints'] && this.keyPoints.length) {
      if (this.isBrowser) {
        this.updateMap();
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
      });
    }
  }

  updateMap(): void {
    if (!this.map) {
      this.initMap();
    } else {
      if (this.isBrowser) {
        import('leaflet').then(L => {
          this.map!.eachLayer((layer: L.Layer) => {
            if (layer instanceof L.Marker) {
              this.map!.removeLayer(layer);
            }
          });

          // Load all images asynchronously and create markers once they are loaded
          const loadImage = (imageFile: File) => {
            return new Promise<string>((resolve, reject) => {
              const reader = new FileReader();
              reader.onload = (e: any) => resolve(e.target.result);
              reader.onerror = reject;
              reader.readAsDataURL(imageFile);
            });
          };

          const imagePromises = this.keyPoints.map(async (kp: any) => {
            if (kp.image instanceof File) {
              // Load the image and return its URL
              const imageUrl = await loadImage(kp.image);
              return { ...kp, imageUrl };
            } else {
              // Return keyPoint as is if no image file is provided
              return kp;
            }
          });

          Promise.all(imagePromises).then(keyPointsWithImages => {
            keyPointsWithImages.forEach(kp => {
              // Define the custom icon with image URL
              const icon = L.icon({
                iconUrl: /*kp.imageUrl ||*/  'assets/markers/marker-icon.png', // Use the image URL from the keyPoint
                iconSize: [38, 38], // Size of the icon
                iconAnchor: [22, 94], // Anchor point of the icon (where the marker points to)
                popupAnchor: [-3, -76] // Anchor point of the popup relative to the icon
              });

              // Create a marker with the custom icon
              //L.marker([kp.latitude, kp.longitude], { icon })
              L.marker([48.8584, 2.2945], { icon })
                .addTo(this.map!)
                .bindPopup(`<b>${kp.title}</b><br>${kp.description}`);
            });
          });
        });
      }
    }
  }

  
}
