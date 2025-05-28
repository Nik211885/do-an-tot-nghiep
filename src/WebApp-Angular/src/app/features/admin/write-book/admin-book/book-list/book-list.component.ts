import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

import { BookService } from '../../services/book.service';
import { Bookv1 } from '../../models/book.model';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { AuthService } from '../../../../../core/auth/auth.service';

@Component({
  selector: 'app-book-list',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.css']
})
export class BookListComponent implements OnInit {
  books: Bookv1[] = [];
  isLoading = true;

  constructor(
    private bookService: BookService,
    private authService: AuthService,
    private toastService: ToastService
  ) {}

  ngOnInit(): void {
    this.authService.initialize().subscribe({
      next: (initialized) => {
        if (initialized) {
          this.loadBooks();
        } else {
          this.toastService.error('Không thể khởi tạo xác thực');
          this.isLoading = false;
        }
      },
      error: () => {
        this.toastService.error('Lỗi khởi tạo xác thực');
        this.isLoading = false;
      }
    });
  }

  loadBooks(): void {
    this.isLoading = true;
    this.bookService.getBooks().subscribe({
      next: (books) => {
        this.books = books;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.toastService.error('Không tải được danh sách sách.');
        this.isLoading = false;
      }
    });
  }
}
