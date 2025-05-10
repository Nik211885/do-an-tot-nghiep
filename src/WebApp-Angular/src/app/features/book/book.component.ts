import { Component, OnInit } from '@angular/core';
import { Book, BookPolicy } from './models/book.model';
import { BookCardComponent } from "./components/book-card/book-card.component";
import { CommonModule } from '@angular/common';
import { BookService } from './services/book.service';

@Component({
  selector: 'app-home',
  imports: [BookCardComponent, CommonModule],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class HomeComponent implements OnInit {
  books: Book[] = [];
  // Sample data for the book
  constructor(private bookService: BookService){}
  ngOnInit() {
    this.books = this.bookService.getBoooks();
  }
}
