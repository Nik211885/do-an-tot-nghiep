import {Component, OnInit} from '@angular/core';
import {UserProfileService} from '../../services/user-profile.service';
import {PaginationSearchHistoryViewModel, SearchHistoryViewModel} from '../../models/search-history.model';
import {DatePipe, NgForOf, NgIf} from '@angular/common';
import {ToastService} from '../../../../../shared/components/toast/toast.service';
import {DialogService} from '../../../../../shared/components/dialog/dialog.component.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-search-history',
  imports: [
    NgForOf,
    NgIf,
    DatePipe
  ],
  standalone: true,
  templateUrl: './search-history.component.html',
  styleUrl: './search-history.component.css'
})
export class SearchHistoryComponent implements OnInit {
  currentPage = 1;
  pageSize = 1;
  paginationSearchHistory!: PaginationSearchHistoryViewModel;
  constructor(private readonly userProfileServices: UserProfileService,
              private readonly toastService: ToastService,
              private router: Router,
              private readonly dialogService: DialogService) {
  }
  ngOnInit(): void {
    this.loadSearchHistory();
  }
  loadSearchHistory(): void {
    this.userProfileServices.getHistorySearch(this.currentPage,this.pageSize)
      .subscribe({
        next: result => {
          this.paginationSearchHistory = result;
          console.log(this.paginationSearchHistory);
        },
        error: error => {
          console.error(error);
        }
      })
  }
  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadSearchHistory();
    }
  }
  getVisiblePages(): (number | string)[] {
    const totalPages = this.paginationSearchHistory?.totalPages ?? 1;
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
    if (this.currentPage < (this.paginationSearchHistory?.totalPages ?? 1)) {
      this.currentPage++;
      this.loadSearchHistory();
    }
  }
  goToPage(page: number): void {
    if (page >= 1 && page <= (this.paginationSearchHistory?.totalPages ?? 1) && page !== this.currentPage) {
      this.currentPage = page;
      this.loadSearchHistory();
    }
  }
  protected readonly Number = Number;

  deleteSearchHistory(h: SearchHistoryViewModel) {
    this.userProfileServices.deleteSearchHistory(h.id)
      .subscribe({
        next: result => {
          if(result){
            this.toastService.success("Xóa thành công");
            this.loadSearchHistory();
          }
          else{
            this.toastService.error("Có lỗi trong quá trình xử lý")
          }
        },
        error: error => {
          console.error(error);
        }
      })
  }

  async cleanAllHistory() {
    const dialogConfirmCleanHistory
      = await this.dialogService.open('Xác nhận xóa lịch sử tìm kiếm',
      'Bạn có chắc chắn muốn xóa toàn bộ lịch sử tìm kiếm'
      );
    if(dialogConfirmCleanHistory.isSuccess) {
      this.userProfileServices.cleanSearchHistory()
        .subscribe({
          next: result => {
            if(result){
              this.toastService.success("Xóa lịch sử tìm kiếm thành công")
              this.loadSearchHistory();
            }
            else {
              this.toastService.error("Có lỗi trong quá trình xóa, vui lòng thử lại sau")
            }
          },
          error: error => {
            console.error(error);
            this.toastService.error("Có lỗi trong quá trình xóa, vui lòng thử lại sau")
          }
        })
    }
  }

  search(h: SearchHistoryViewModel) {
    this.router.navigate(["search", h.searchTerm])
  }
}
