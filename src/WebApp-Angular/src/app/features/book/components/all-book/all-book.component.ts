import {Component, OnInit} from '@angular/core';
import {PublicBookService} from '../../services/public-book.service';
import {PaginationBook} from '../../models/book.model';
import {BookCardComponent} from '../book-card/book-card.component';
import {NgForOf, NgIf} from '@angular/common';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-all-book',
  imports: [
    BookCardComponent,
    NgForOf,
    NgIf,
    PaginationComponent
  ],
  standalone: true,
  templateUrl: './all-book.component.html',
  styleUrl: './all-book.component.css'
})
export class AllBookComponent implements OnInit {
  currentPage = 1;
  pageSize = 1;
  paginationBook!: PaginationBook;
  constructor(private publicBookService: PublicBookService) {
  }
  ngOnInit(): void {
    this.loadPage();
  }
  loadPage(){
    this.publicBookService.getAllBookWithPagination(this.currentPage, this.pageSize).subscribe(
      {
        next:(result)=>{
          if(result){
            this.paginationBook = result;
            this.publicBookService.paginationBookAggregate(this.paginationBook);
          }
        },
        error:(error)=>{
          console.error(error);
        }
      }
    )
  }

  changeCurrentPage($event: number) {
    this.currentPage = $event;
    this.loadPage();
  }
}
