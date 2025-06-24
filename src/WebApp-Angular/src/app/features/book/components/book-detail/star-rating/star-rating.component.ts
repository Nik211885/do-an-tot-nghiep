import { CommonModule } from '@angular/common';
import {Component, Input, OnInit} from '@angular/core';

@Component({
  selector: 'app-star-rating',
  imports: [CommonModule],
  templateUrl: './star-rating.component.html',
  styleUrl: './star-rating.component.css'
})
export class StarRatingComponent implements OnInit{
  @Input() set rating(value: number) {
    this._rating = value;
    this.calculateStars();
  }

  get rating(): number {
    return this._rating;
  }

  private _rating = 0;
  stars: ('full' | 'half' | 'empty')[] = [];

  /**
   * Calculate star types based on rating
   */
  private calculateStars(): void {
    this.stars = [];
    const fullStars = Math.floor(this.rating);
    const hasHalfStar = this.rating % 1 >= 0.5;

    // Add full stars
    for (let i = 0; i < fullStars; i++) {
      this.stars.push('full');
    }

    // Add half star if needed
    if (hasHalfStar) {
      this.stars.push('half');
    }

    // Add empty stars
    while (this.stars.length < 5) {
      this.stars.push('empty');
    }
  }

  // Initialize stars on component init
  ngOnInit(): void {
    this.calculateStars();
  }
}
