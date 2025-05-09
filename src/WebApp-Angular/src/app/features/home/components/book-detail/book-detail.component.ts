import { Component, Input, OnInit } from '@angular/core';
import { Book, BookPolicy } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { BookCardComponent } from "../book-card/book-card.component";

@Component({
  selector: 'app-book-detail',
  imports: [CommonModule],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit {
  book!: Book;
  userRating: number = 0;
  BookPolicy = BookPolicy; // Expose the enum to the template
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

}
