import { Component, OnInit } from '@angular/core';
import { Book, BookPolicy, BookReleaseType } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookGenreTagComponent } from './book-genre-tag/book-genre-tag.component';
import { BookChapterListComponent } from './book-chapter-list/book-chapter-list.component';
import { BookPolicyBadgeComponent } from './book-policy-badge/book-policy-badge.component';
import { StarRatingComponent } from './star-rating/star-rating.component';
import { CommentBookSectionComponent } from "./comment-book-section/comment-book-section.component";

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [
    CommonModule,
    BookGenreTagComponent,
    BookPolicyBadgeComponent,
    BookChapterListComponent,
    // BookCommentSectionComponent,
    StarRatingComponent,
    CommentBookSectionComponent
],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit {
  book!: Book;
  BookPolicy = BookPolicy;
  BookReleaseType = BookReleaseType;
  
  isDescriptionExpanded = false;
  isFavorite = false;
  activeTab: 'chapters' | 'comments' = 'chapters';
  constructor(private route: ActivatedRoute,
     private bookService: BookService,
    private router: Router) {

  }
  ngOnInit(): void {
    if(history.state.book){
      this.book = history.state.book;
    }
    else{
      const bookSlug = decodeURIComponent(this.route.snapshot.paramMap.get('slug') || '');
      const bookFinSlug = this.bookService.getBookBySlug(bookSlug);
      if(bookFinSlug){
        this.book = bookFinSlug;
      }
      else{
        this.router.navigate(["/error/not-found"])
      }
    }
  }
  get genreNames(): string {
    return (this.book.genres ?? []).map(g => g.name).join(', ');
  }
  
  /**
   * Toggles the favorite state
   */
  toggleFavorite(e: Event): void {
    e.preventDefault();
    e.stopPropagation();
    this.isFavorite = !this.isFavorite;
  }
  
  /**
   * Toggles the description expanded state
   */
  toggleDescription(): void {
    this.isDescriptionExpanded = !this.isDescriptionExpanded;
  }
  
  /**
   * Sets the active tab
   */
  setActiveTab(tab: 'chapters' | 'comments'): void {
    this.activeTab = tab;
  }
  
  /**
   * Returns the appropriate status label based on book completion and release type
   */
  getStatusLabel(): string {
    if (this.book.isCompeleted) {
      return 'Hoàn thành';
    }
    return this.book.bookReleaseType === BookReleaseType.Serialized ? 'Đang ra' : 'Sắp ra mắt';
  }
  
  /**
   * Returns the appropriate status color class based on book status
   */
  getStatusColor(): string {
    if (this.book.isCompeleted) {
      return 'bg-emerald-100 text-emerald-800';
    }
    return this.book.bookReleaseType === BookReleaseType.Serialized 
      ? 'bg-blue-100 text-blue-800' 
      : 'bg-amber-100 text-amber-800';
  }
  
  /**
   * Returns the appropriate CTA button text based on book policy
   */
  getCtaButtonText(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'Đọc Ngay';
      case BookPolicy.Paid:
        return `Mua ${this.book.policyReadBook.price?.toLocaleString('vi-VN')}đ`;
      case BookPolicy.Subscription:
        return 'Đăng Ký Để Đọc';
      default:
        return 'Đọc Ngay';
    }
  }
  
  /**
   * Returns the appropriate CTA button color based on book policy
   */
  getCtaButtonColor(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'bg-emerald-600 hover:bg-emerald-700';
      case BookPolicy.Paid:
        return 'bg-amber-600 hover:bg-amber-700';
      case BookPolicy.Subscription:
        return 'bg-indigo-600 hover:bg-indigo-700';
      default:
        return 'bg-emerald-600 hover:bg-emerald-700';
    }
  }
  
  /**
   * Returns the policy display text
   */
  getPolicyLabel(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'Miễn phí';
      case BookPolicy.Paid:
        return 'Trả phí';
      case BookPolicy.Subscription:
        return 'Đăng ký';
      default:
        return 'Miễn phí';
    }
  }
  
  /**
   * Returns the appropriate policy text color
   */
  getPolicyTextColor(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'text-emerald-600';
      case BookPolicy.Paid:
        return 'text-amber-600';
      case BookPolicy.Subscription:
        return 'text-indigo-600';
      default:
        return 'text-emerald-600';
    }
  }
  
  /**
   * Returns a random percentage for the rating distribution visualization
   */
  getRandomPercentage(): number {
    return Math.floor(Math.random() * 100);
  }
}
