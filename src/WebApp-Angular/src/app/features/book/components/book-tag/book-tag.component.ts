import {Component, OnInit} from '@angular/core';
import {PublicBookService} from '../../services/public-book.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Book, PaginationBook} from '../../models/book.model';
import {BookCardComponent} from '../book-card/book-card.component';
import {NgForOf, NgIf} from '@angular/common';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-book-tag',
  imports: [
    BookCardComponent,
    NgForOf,
    NgIf,
    PaginationComponent
  ],
  standalone: true,
  templateUrl: './book-tag.component.html',
  styleUrl: './book-tag.component.css'
})
export class BookTagComponent implements OnInit {
  currentPage= 1;
  pageSize  = 1;
  tag!: string;
  paginationBook!: PaginationBook;
  constructor(private publicBookService: PublicBookService,
              private router: Router,
              private activeRouter: ActivatedRoute) {
  }
    ngOnInit(): void {
        this.tag = this.activeRouter.snapshot.params['tag'];
        if( this.tag){
          this.loadBook();
        }
        else{
          this.router.navigate(["/error/not-found"]);
        }
    }
    loadBook(){
      this.publicBookService.getBookByTagNameWithPagination(this.tag, this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if(result){
            this.publicBookService.paginationBookAggregate(result);
            this.paginationBook = result;
          }
          else{
            this.router.navigate(["/error/not-found"]);
          }
        },
        error: error => {
          console.error(error);
        }
      })
    }

  changeCurrentPage(currentPage: number) {
    console.log(currentPage);
    this.currentPage = currentPage;
    this.loadBook();
  }
  trackByBookId(index: number, book: Book): any {
    return book.id;
  }

  protected readonly Math = Math;
}
