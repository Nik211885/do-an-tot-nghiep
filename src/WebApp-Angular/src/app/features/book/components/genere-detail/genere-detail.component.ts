import { Component, OnInit } from '@angular/core';
import { Genere } from '../../models/genere.model';
import {Book, PaginationBook} from '../../models/book.model';
import { ActivatedRoute, Router } from '@angular/router';
import { GenereService } from '../../services/genere.service';
import {PublicBookService} from '../../services/public-book.service';
import {BookCardComponent} from '../book-card/book-card.component';
import {NgForOf, NgIf} from '@angular/common';
import {BookReviewModel} from '../../models/book-review.model';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';

@Component({
  selector: 'app-genere-detail',
  imports: [
    BookCardComponent,
    NgForOf,
    PaginationComponent,
    NgIf
  ],
  templateUrl: './genere-detail.component.html',
  styleUrl: './genere-detail.component.css'
})
export class GenereDetailComponent implements OnInit {
  genre: Genere | undefined;
  currentPage: number = 1;
  pageSize: number = 1;
  paginationBook: PaginationBook | null = null;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private publicBookService: PublicBookService,
    private genereService: GenereService
  ) {}
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      console.log(slug)
      if (slug) {
        this.loadGenreBySlug(slug);
      }
    });
  }
  loadGenreBySlug(slug: string) {
    this.genereService.getGenereBySlug(slug).subscribe(genere=>{
      if(genere){
        this.genre = genere;
        this.loadBooksByGenreSlug();
      }
    });
  }
  loadBooksByGenreSlug() {
    console.log(this.genre);
    if(this.genre) {
      this.publicBookService.getBookByGenreId(this.genre.slug, this.currentPage, this.pageSize)
        .subscribe({
          next: (result) => {
            if (result) {
              this.paginationBook = result;
              if (this.paginationBook) {
                this.publicBookService. paginationBookAggregate(this.paginationBook);
              }
            }
          },
          error: err => {
            console.error(err);
          }
        })
    }
  }

  changeCurrentPage(currentPage: number) {
    console.log(currentPage);
    this.currentPage = currentPage;
    this.loadBooksByGenreSlug();
  }
}
