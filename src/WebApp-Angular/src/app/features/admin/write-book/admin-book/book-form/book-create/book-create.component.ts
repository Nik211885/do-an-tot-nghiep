import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { BookBasicInfoComponent } from '../book-basic-info/book-basic-info.component';
import { Bookv1 } from '../../../models/book.model';
import { BookService } from '../../../services/book.service';
import { Router } from '@angular/router';
import { BookTagsGenresComponent } from '../book-tags-genres/book-tags-genres.component';
import { BookPricingInfoComponent } from '../book-pricing-info/book-pricing-info.component';
import { BookStatusInfoComponent } from '../book-status-info/book-status-info.component';
import { ToastService } from '../../../../../../shared/components/toast/toast.service';

enum FormStep {
  BASIC_INFO = 0,
  PRICING_INFO = 1,
  STATUS_INFO = 2,
  TAGS_GENRES = 3
}

@Component({
  selector: 'app-book-create',
  imports: [
    CommonModule,
    BookBasicInfoComponent,
    BookPricingInfoComponent,
    BookStatusInfoComponent,
    BookTagsGenresComponent
  ],
  templateUrl: './book-create.component.html',
  styleUrl: './book-create.component.css'
})
export class BookCreateComponent {
  FormStep = FormStep;
  currentStep = FormStep.BASIC_INFO;
  book: Bookv1 = {
    title: '',
    description: '',
    slug: '',
    isPaid: false,
    requiresRegistration: false,
    isCompleted: false,
    tags: [],
    genres: []
  };

  constructor(
    private bookService: BookService,
    private toastService: ToastService,
    private router: Router
  ) {}

  onBasicInfoNext(basicInfo: Partial<Bookv1>): void {
    this.book = { ...this.book, ...basicInfo };
    this.currentStep = FormStep.PRICING_INFO;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onPricingInfoNext(pricingInfo: Partial<Bookv1>): void {
    this.book = { ...this.book, ...pricingInfo };
    this.currentStep = FormStep.STATUS_INFO;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onStatusInfoNext(statusInfo: Partial<Bookv1>): void {
    this.book = { ...this.book, ...statusInfo };
    this.currentStep = FormStep.TAGS_GENRES;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onPreviousStep(): void {
    this.currentStep--;
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  onBookSubmit(tagsGenres: Partial<Bookv1>): void {
    this.book = { ...this.book, ...tagsGenres };

    this.bookService.createBook(this.book).subscribe({
      next: (createdBook) => {
        this.toastService.success('Tạo mới sách thành công');
        this.router.navigate(['/write-book/books', createdBook.slug]);
      },
      error: (error) => {
        console.error('Error creating book:', error);
        this.toastService.error(error['error']);
      }
    });
  }
}
