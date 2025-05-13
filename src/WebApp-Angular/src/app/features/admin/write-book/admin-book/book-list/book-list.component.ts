import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { Book } from "../../models/book.model";
import { BookService } from '../../services/book.service';
import { RouterLink } from '@angular/router';
import { ToastService } from '../../../../../shared/components/toast/toast.service';

@Component({
  selector: 'app-book-list',
  imports: [CommonModule, RouterLink],
  standalone: true,
  templateUrl: './book-list.component.html',
  styleUrl: './book-list.component.css'
})
export class BookListComponent implements OnInit {
  books: Book[] = [];
  constructor(
    private bookService: BookService,
    private toastService: ToastService
  ) {}
   ngOnInit(): void {
    this.loadBooks();
  }
  loadBooks(): void {
    this.bookService.getBooks().subscribe({
      next: (books) => {
        this.books = books;
      },
      error: (error) => {
        console.error('Error loading books:', error);
        this.toastService.error('Failed to load books.');
      }
    });
  }

}
