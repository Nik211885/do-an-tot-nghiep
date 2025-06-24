import { CommonModule } from '@angular/common';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  standalone: true,
  selector: 'app-rating-form',
  imports: [CommonModule, FormsModule],
  templateUrl: './rating-form.component.html',
  styleUrl: './rating-form.component.css'
})
export class RatingFormComponent implements OnInit {
  ngOnInit(): void {
      console.log(this.rating);
  }
  @Input() bookTitle: string = '';
  @Input() bookId: string = '';

  @Output() ratingSubmitted = new EventEmitter<{rating: number, comment: string}>();

  @Input() rating: number = 0;
  hoverRating: number = 0;
  comment: string = '';
  errorMessage: string = '';

  setRating(value: number): void {
    this.rating = value;
    this.errorMessage = '';
  }

  submitRating(): boolean {
    if (this.rating === 0) {
      this.errorMessage = 'Vui lòng chọn số sao đánh giá';
      return false;
    }

    this.ratingSubmitted.emit({
      rating: this.rating,
      comment: this.comment
    });

    return true;
  }

  resetForm(): void {
    this.rating = 0;
    this.comment = '';
    this.errorMessage = '';
  }
}
