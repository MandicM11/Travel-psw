import { Injectable, Inject, PLATFORM_ID } from '@angular/core';
import { isPlatformBrowser } from '@angular/common';

@Injectable({
  providedIn: 'root'
})
export class MapService {
  private L: any;

  constructor(@Inject(PLATFORM_ID) private platformId: Object) {}

  loadLeaflet() {
    if (isPlatformBrowser(this.platformId)) {
      if (!this.L) {
        // Dinamički učitaj Leaflet samo u browseru
        this.L = require('leaflet');
      }
      return this.L;
    }
    return null;
  }
}
