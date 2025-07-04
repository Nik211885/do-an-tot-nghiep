import {Component, OnInit} from '@angular/core';
import {UserProfileService} from '../../services/user-profile.service';
import {PaginationFavoriteBookViewModel} from '../../models/favorite-book.model';
import {PublicBookService} from '../../../../book/services/public-book.service';
import {Book, Genre} from '../../../../book/models/book.model';
import {AuthService} from '../../../../../core/auth/auth.service';
import {UserModel} from '../../../../../core/models/user.model';
import {NgForOf, NgIf} from '@angular/common';
import {range} from 'rxjs';
import { Router } from '@angular/router';

@Component({
  selector: 'app-book-favorite',
  standalone: true,
  imports: [
    NgForOf,
    NgIf
  ],
  templateUrl: './book-favorite.component.html',
  styleUrl: './book-favorite.component.css'
})
export class BookFavoriteComponent  implements OnInit {
  userModel!: UserModel;
  currentPage = 1;
  pageSize = 5;
  paginationFavoriteBook!: PaginationFavoriteBookViewModel;
  bookInFavorite!: Book[];
  constructor(private userProfileService: UserProfileService,
              private publicBookService: PublicBookService,
              private router: Router,
              private authService: AuthService) {
  }
    ngOnInit(): void {
      this.loadUserModel();
      this.loadFavoriteBook();
    }
    loadUserModel(){
      this.authService.getCurrentUser().subscribe({
        next: (value)=>{
          if(value){
            this.userModel = value;
          }
        },
        error: (err)=>{
          console.error(err)
        }
      })
    }
    loadFavoriteBook(){
      this.userProfileService
        .getFavoriteBook(this.currentPage,this.pageSize)
        .subscribe({
          next: (value)=>{
            this.paginationFavoriteBook = value;
            const ids = this.paginationFavoriteBook
              .items.map(x=>x.favoriteBookId)
            this.loadBookFavoriteBook(ids);
          },
          error:(err)=>{
            console.error(err);
          }
        })
    }
    loadBookFavoriteBook(ids: string[]){
      this.publicBookService.getBookInIds(ids).subscribe({
        next: (value)=>{
          this.bookInFavorite = value;
          const ids = this.paginationFavoriteBook
            .items.map(x=>x.favoriteBookId);
          this.publicBookService.getMyRatingForBookIds(ids)
            .subscribe({
              next: (value)=>{
                if(value){
                  this.bookInFavorite.forEach(x=>{
                    const item = value.find(y=>y.bookId === x.id);
                    console.log(x.id);
                    console.log(value);
                    if(item){
                      x.myRating = item.star
                    }
                  })
                }
              }
            })
        },
        error: (err)=>{
          console.error(err)
        }
      })
    }
    getGenreNames(genres: Genre[]): string {
      return genres.map(x => x.name).join('.') || '';
    }
    previousPage(): void {
      if (this.currentPage > 1) {
        this.currentPage--;
        this.loadFavoriteBook();
      }
  }
  getVisiblePages(): (number | string)[] {
    const totalPages = this.paginationFavoriteBook?.totalPages ?? 1;
    const current = this.currentPage;
    const pages: (number | string)[] = [];

    if (totalPages <= 7) {
      // If total pages <= 7, show all pages
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Always show first page
      pages.push(1);

      if (current <= 4) {
        // Current page is near the beginning
        for (let i = 2; i <= 5; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(totalPages);
      } else if (current >= totalPages - 3) {
        // Current page is near the end
        pages.push('...');
        for (let i = totalPages - 4; i <= totalPages; i++) {
          pages.push(i);
        }
      } else {
        // Current page is in the middle
        pages.push('...');
        for (let i = current - 1; i <= current + 1; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(totalPages);
      }
    }

    return pages;
  }
  isCurrentPage(page: number | string): boolean {
    return typeof page === 'number' && page === this.currentPage;
  }

  isClickablePage(page: number | string): boolean {
    return typeof page === 'number';
  }
  nextPage(): void {
    if (this.currentPage < (this.paginationFavoriteBook?.totalPages ?? 1)) {
      this.currentPage++;
      this.loadFavoriteBook();
    }
  }
  goToPage(page: number): void {
    if (page >= 1 && page <= (this.paginationFavoriteBook?.totalPages ?? 1) && page !== this.currentPage) {
      this.currentPage = page;
      this.loadFavoriteBook();
    }
  }

  protected readonly Number = Number;
  protected readonly range = range;

  viewDetail(b: Book) {
    this.router.navigate(["books", b.slug])
  }
}
