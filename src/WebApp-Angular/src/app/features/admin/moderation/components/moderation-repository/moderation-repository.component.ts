import {Component, OnInit} from '@angular/core';
import {ModerationService} from '../../services/moderation.service';
import {ModerationForBookGroup, PaginationModerationForBookGroup} from '../../models/moderation.model';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {CommentBookService} from '../../../../book/services/comment-book.service';
import {UserModel} from '../../../../../core/models/user.model';
import {Router} from '@angular/router';
import {GenreViewModel} from '../../../../resources/models/genres.model';

@Component({
  selector: 'app-moderation-repository',
  imports: [CommonModule, FormsModule],
  templateUrl: './moderation-repository.component.html',
  styleUrl: './moderation-repository.component.css'
})
export class ModerationRepositoryComponent implements OnInit{
  moderationForBook: ModerationForBookGroup[] = [];
  currentPage: number = 1;
  pageSize: number = 1;
  totalPages: number = 0;
  totalCount: number = 0;
  search: string = '';
  hasPreviousPage: boolean = false;
  hasNextPage: boolean = false;

  constructor(private moderationService: ModerationService,
              private router: Router,
              private commentService: CommentBookService) {
  }

  ngOnInit(): void {
    this.loadModerationData();
  }

  loadModerationData(): void {
    this.moderationService.getModerationForBookGroup(this.currentPage, this.pageSize, this.search)
      .subscribe({
        next: (result: PaginationModerationForBookGroup) => {
          if(result) {
            this.moderationForBook = result.items;
            this.currentPage = result.pageNumber;
            this.totalPages = result.totalPages;
            this.totalCount = result.totalCount;
            this.hasPreviousPage = result.hasPreviousPage;
            this.hasNextPage = result.hasNextPage;
            const userId = this.moderationForBook
              .map(x=>x.authorId);
            if(userId){
              this.commentService.getUserByIds(userId)
                .subscribe({
                  next: (results) => {
                    if(results){
                      this.moderationForBook.forEach((moderation)=>{
                        const userData = results.find(x => x.id === moderation.authorId);
                        if (userData) {
                          const userModel = new UserModel(userData); // now getFullName is available
                          moderation.authorName = userModel.getFullName();
                        }
                      })
                    }
                  }
                })
            }
          }
        },
        error: (error) => {
          console.error('Error loading moderation data:', error);
        }
      });
  }

  // Pagination methods
  previousPage(): void {
    if (this.hasPreviousPage) {
      this.currentPage--;
      this.loadModerationData();
    }
  }

  nextPage(): void {
    if (this.hasNextPage) {
      this.currentPage++;
      this.loadModerationData();
    }
  }

  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.loadModerationData();
    }
  }

  viewDetails(bookId: string): void {
    this.router.navigate([`/moderation/all-chapter/in-book/`, bookId]);
  }

  // Helper method to get page numbers for pagination
  getPageNumbers(): number[] {
    const pages: number[] = [];
    const startPage = Math.max(1, this.currentPage - 2);
    const endPage = Math.min(this.totalPages, this.currentPage + 2);

    for (let i = startPage; i <= endPage; i++) {
      pages.push(i);
    }

    return pages;
  }

  // Handle page size change
  onPageSizeChange(): void {
    this.currentPage = 1; // Reset to first page when changing page size
    this.loadModerationData();
  }

  // Expose Math to template
  Math = Math;

  searchBook() {
    console.log(this.search);
    if(this.search) {
      this.currentPage = 1;
      this.loadModerationData();
    }
  }

  active(book: ModerationForBookGroup) {
    book.isActive = !book.isActive;
    if(book.isActive){
      this.moderationService.activeForBook(book.bookId)
        .subscribe({
          next: data => {
            if(data){
              return;
            }
            else{
              book.isActive = !book.isActive;
              return;
            }
          },
          error: err => {
            book.isActive = ! book.isActive;
            return;
          }
        })
    }
    else{
      this.moderationService.unActiveForBook(book.bookId)
        .subscribe({
          next: data => {
            if(data){
              return;
            }
            else{
              book.isActive = ! book.isActive;
              return;
            }
          },
          error: err => {
            book.isActive = ! book.isActive;
            return;
          }
        })
    }
  }
}
