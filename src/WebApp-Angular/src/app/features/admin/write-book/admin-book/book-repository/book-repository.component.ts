import {Component, OnInit} from '@angular/core';
import {Router, RouterLink} from '@angular/router';
import {BookService} from '../../services/book.service';
import {DialogService} from '../../../../../shared/components/dialog/dialog.component.service';
import {Bookv1, PaginationBook} from '../../models/book.model';
import {ToastService} from '../../../../../shared/components/toast/toast.service';
import {NgForOf, NgIf} from '@angular/common';
import {BookReleaseType} from '../../models/create-book.model';

@Component({
  selector: 'app-book-repository',
  imports: [
    RouterLink,
    NgForOf,
    NgIf
  ],
  standalone: true,
  templateUrl: './book-repository.component.html',
  styleUrl: './book-repository.component.css'
})
export class BookRepositoryComponent implements OnInit {
  paginationBooks: PaginationBook | null = null;
  currentPage = 1;
  pageSize = 5;

  constructor(private readonly bookService: BookService,
              private readonly toastService: ToastService,
              private readonly router: Router,
              private readonly dialogService: DialogService) {
  }

  ngOnInit(): void {
    this.loadBooks();
  }

  loadBooks(): void {
    this.bookService.getBookWithPagination(this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if(result){
            this.paginationBooks = result;
          }
          else{
            this.toastService.error("không tìm thấy sách của bạn trong kho lưu trữ");
          }
        },
        error: error => {
          console.error(error);
        }
      });
  }

  editBook(b: Bookv1){
    this.router.navigate(['/write-book/books', b.slug]);
  }

  async deleteBook(b: Bookv1){
    const result = await this.dialogService.open('Xác nhận xóa sách', 'Bạn có chắc chắn muốn xóa quyển sách này');
    if(result.isSuccess){
      if(b.id){
        this.bookService.deleteBook(b.id)
          .subscribe({
            next: result => {
                if(result){
                  this.toastService.success("Xóa sách thành công");
                  this.loadBooks();
                }
            },
            error: error => {console.error(error);}
          })
      }
    }
  }

  getGenres(book: Bookv1): string{
    return book.genres.map(x=>x.name).join(", ") ?? "";
  }

  getTag(book: Bookv1): string{
    return book.tags.map(x=>x).join(", ") ?? "";
  }

  range(n: number): number[] {
    return Array.from({ length: n }, (_, i) => i + 1);
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= (this.paginationBooks?.totalPages ?? 1) && page !== this.currentPage) {
      this.currentPage = page;
      this.loadBooks();
    }
  }

  nextPage(): void {
    if (this.currentPage < (this.paginationBooks?.totalPages ?? 1)) {
      this.currentPage++;
      this.loadBooks();
    }
  }

  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadBooks();
    }
  }

  getVisiblePages(): (number | string)[] {
    const totalPages = this.paginationBooks?.totalPages ?? 1;
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

  protected readonly Number = Number;
}
