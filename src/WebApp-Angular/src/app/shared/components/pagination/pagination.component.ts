import {Component, EventEmitter, Input, OnChanges, OnInit, Output, SimpleChanges} from '@angular/core';
import {NgForOf, NgIf} from "@angular/common";

export interface Pagination<T> {
  items: T[],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  pageSize: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}

@Component({
  selector: 'app-pagination',
  standalone: true,
  imports: [
    NgForOf,
    NgIf
  ],
  templateUrl: './pagination.component.html',
  styleUrl: './pagination.component.css'
})
export class PaginationComponent<T> {
    @Input() currentPage!: number;
    @Input() pageSize!: number;
    @Output() changeCurrentPage =
      new EventEmitter<number>();
    @Input() pagination: Pagination<T> | null = null;
    previousPage(): void {
      if (this.currentPage > 1) {
        this.currentPage--;
        this.changeCurrentPage.emit(this.currentPage);
      }
    }
    getVisiblePages(): (number | string)[] {
      const totalPages = this.pagination?.totalPages ?? 1;
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
      if (this.currentPage < (this.pagination?.totalPages ?? 1)) {
        this.currentPage++;
        this.changeCurrentPage.emit(this.currentPage);
      }
    }
    goToPage(page: number): void {
      if (page >= 1 && page <= (this.pagination?.totalPages ?? 1) && page !== this.currentPage) {
        this.currentPage = page;
        this.changeCurrentPage.emit(this.currentPage);
      }
    }
    protected readonly Number = Number;
}
