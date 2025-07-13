import {Component, HostListener, OnInit} from '@angular/core';
import {CommonModule} from '@angular/common';
import {ModerationService} from '../../services/moderation.service';
import {ApproveStatus, PaginationModeration, ModerationViewModel} from '../../models/moderation.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-list-moderation',
  imports: [CommonModule],
  templateUrl: './list-moderation.component.html',
  styleUrl: './list-moderation.component.css'
})
export class ListModerationComponent implements OnInit{
  currentPage = 1;
  pageSize = 1;
  paginationModeration!: PaginationModeration;
  isLoading = false;
  error: string | null = null;

  constructor(private moderationService: ModerationService,
              private router: Router) {
  }

  ngOnInit(): void {
    this.loadModerationData();
  }

  loadModerationData(): void {
    this.isLoading = true;
    this.error = null;

    this.moderationService.getApprovalChapter(this.currentPage, this.pageSize)
      .subscribe({
        next: data => {
          if(data){
            this.paginationModeration = data;
            this.moderationService.getBookApprovalByIds(this.paginationModeration.items);
            console.log(this.paginationModeration);
          }
          this.isLoading = false;
        },
        error: err => {
          this.error = 'Có lỗi xảy ra khi tải dữ liệu';
          this.isLoading = false;
          console.error('Error loading moderation data:', err);
        }
      });
  }

  onPageChange(page: number): void {
    if (page !== this.currentPage && page >= 1 && page <= this.paginationModeration.totalPages) {
      this.currentPage = page;
      this.loadModerationData();
    }
  }

  getPageNumbers(): number[] {
    if (!this.paginationModeration) return [];

    const totalPages = this.paginationModeration.totalPages;
    const current = this.currentPage;
    const pages: number[] = [];

    // Show pages around current page
    const start = Math.max(1, current - 2);
    const end = Math.min(totalPages, current + 2);

    for (let i = start; i <= end; i++) {
      pages.push(i);
    }

    return pages;
  }

  formatDate(date: Date): string {
    return new Date(date).toLocaleDateString('vi-VN', {
      year: 'numeric',
      month: '2-digit',
      day: '2-digit',
      hour: '2-digit',
      minute: '2-digit'
    });
  }

  truncateContent(content: string, maxLength: number = 150): string {
    if (content.length <= maxLength) return content;
    return content.substring(0, maxLength) + '...';
  }

  protected readonly Math = Math;


  viewDetailModeration(item: ModerationViewModel, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/moderation/detail', item.id]);
  }

  protected readonly Date = Date;
}
