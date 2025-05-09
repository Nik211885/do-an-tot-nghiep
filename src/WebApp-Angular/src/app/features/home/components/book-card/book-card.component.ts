// book-card.component.ts
import { Component, EventEmitter, Host, HostListener, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Book, BookPolicy } from '../../models/book.model';
import { Router } from '@angular/router';
import { state } from '@angular/animations';

@Component({
  selector: 'app-book-card',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './book-card.component.html',
  styleUrls: ['./book-card.component.css']
})
export class BookCardComponent {
  @Input() book!: Book;
  constructor(private router: Router) {}
  // BookPolicy enum for template access
  BookPolicy = BookPolicy;
  
  // Returns appropriate policy label in Vietnamese
  getPolicyLabel(policy: BookPolicy): string {
    switch(policy) {
      case BookPolicy.Free:
        return 'Miễn phí';
      case BookPolicy.Paid:
        return 'Trả phí';
      case BookPolicy.Subscription:
        return 'Gói thành viên';
      default:
        return '';
    }
  }
  
  // Returns stars array for the rating display
  get ratingStars(): number[] {
    return Array(5).fill(0).map((_, i) => i < Math.round(this.book.rating) ? 1 : 0);
  }
  
  // Truncates description to specific length
  truncateDescription(description: string, maxLength: number = 100): string {
    if (description.length <= maxLength) return description;
    return description.substring(0, maxLength) + '...';
  }
  @HostListener('click', ["$event"])
  onClick(event: Event) {
    console.log("Book card clicked", event);
    const encodedTitle = encodeURIComponent(this.book.title);
    this.router.navigate(['/books', this.book.slug],{
      state: { book: this.book }
    });
  }
  toggleFavorite(id: string){
    console.log("Toggle favorite", id);
  }
}