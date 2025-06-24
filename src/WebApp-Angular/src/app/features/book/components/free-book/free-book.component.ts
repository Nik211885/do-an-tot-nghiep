import {Component, OnInit} from '@angular/core';
import {PublicBookService} from '../../services/public-book.service';
import {BookPolicy, PaginationBook} from '../../models/book.model';
import {BookCardComponent} from '../book-card/book-card.component';
import {NgForOf, NgIf} from '@angular/common';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-free-book',
  imports: [
    BookCardComponent,
    NgForOf,
    NgIf,
    PaginationComponent
  ],
  standalone: true,
  templateUrl: './free-book.component.html',
  styleUrl: './free-book.component.css'
})
export class FreeBookComponent implements OnInit {
  currentPage: number = 1;
  pageSize: number = 10;
  paginationBook!: PaginationBook;
  constructor(private publicBookService: PublicBookService) { }

  ngOnInit(): void {
    this.loadData();
  }
  loadData(){
    this.publicBookService.
    getBookByPolicy(BookPolicy.Free, this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if(result) {
            this.paginationBook = result;
            this.publicBookService.paginationBookAggregate(this.paginationBook);
            console.log(this.paginationBook);
          }
        },
        error: err => {
          console.error(err);
        }
      })
  }

  changeCurrentPage($event: number) {
    this.currentPage = $event;
    this.loadData();
  }
}
