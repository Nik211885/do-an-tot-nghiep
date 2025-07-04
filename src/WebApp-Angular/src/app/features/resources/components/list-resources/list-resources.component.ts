import {Component, OnInit} from '@angular/core';
import {GenreService} from '../../services/genre.service';
import {GenreViewModel, PaginationGenreViewModel} from '../../models/genres.model';
import {NgForOf, NgIf, DatePipe} from '@angular/common';
import {Router} from '@angular/router';

@Component({
  selector: 'app-list-resources',
  imports: [
    NgForOf,
    NgIf,
    DatePipe
  ],
  standalone: true,
  templateUrl: './list-resources.component.html',
  styleUrl: './list-resources.component.css'
})
export class ListResourcesComponent implements OnInit {
  currentPage = 1;
  pageSize = 10; // Tăng pageSize lên 10 để hiển thị nhiều hơn
  genrePagination: PaginationGenreViewModel | null = null;

  constructor(private genreService: GenreService,
              private router: Router) {}

  ngOnInit(): void {
    this.loadPaginationGenre();
  }

  loadPaginationGenre(): void {
    this.genreService.getGenresWithPagination(this.currentPage, this.pageSize)
      .subscribe({
        next: data => {
          if (data) {
            this.genrePagination = data;
          } else {
            this.genrePagination = null;
          }
        },
        error: err => {
          console.error('Error loading genres:', err);
          this.genrePagination = null;
        }
      });
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadPaginationGenre();
    }
  }

  nextPage(): void {
    if (this.currentPage < (this.genrePagination?.totalPages ?? 1)) {
      this.currentPage++;
      this.loadPaginationGenre();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= (this.genrePagination?.totalPages ?? 1) && page !== this.currentPage) {
      this.currentPage = page;
      this.loadPaginationGenre();
    }
  }

  getVisiblePages(): (number | string)[] {
    const totalPages = this.genrePagination?.totalPages ?? 1;
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

  editGenre(genre: any): void {
    this.router.navigate(["/resources/genres/detail", genre.id])
  }

  protected readonly Math = Math;
  protected readonly Number = Number;

    goToCreateGenre() {
    this.router.navigate(["/resources/genres/create"]);
  }

  active(genre: GenreViewModel) {
      genre.isActive = ! genre.isActive;
      this.genreService.changeActive(genre.id)
        .subscribe({
          next: data => {
            if(data){
              return;
            }
            else{
              genre.isActive = ! genre.isActive;
              return;
            }
          },
          error: err => {
            genre.isActive = ! genre.isActive;
            return;
          }
        })
  }
}
