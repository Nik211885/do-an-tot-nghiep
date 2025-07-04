import {Component, OnInit} from '@angular/core';
import {PublicBookService} from '../../services/public-book.service';
import {BookPolicy, PaginationBook} from '../../models/book.model';
import {BookCardComponent} from '../book-card/book-card.component';
import {NgForOf, NgIf} from '@angular/common';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-paid-book',
  imports: [
    BookCardComponent,
    NgForOf,
    NgIf,
    PaginationComponent
  ],
  standalone: true,
  templateUrl: './paid-book.component.html',
  styleUrl: './paid-book.component.css'
})
export class PaidBookComponent implements OnInit {
  currentPage: number = 1;
  pageSize: number = 10;
  paginationBook: PaginationBook | undefined;
  constructor(private publicBookService: PublicBookService) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData(): void {
    this.publicBookService.getBookByPolicy(
      BookPolicy.Paid, this.currentPage, this.pageSize
    )
      .subscribe({
        next: result => {
          if(result){
            this.paginationBook = result;
            this.publicBookService.paginationBookAggregate(this.paginationBook);
          }
        },
        error: error => {
          console.error(error);
        }
      })
  }

  changeCurrentPage($event: number) {
    this.currentPage = $event;
    this.loadData();
  }
}
