import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Book, Chapter } from '../../models/book.model';
import { BookService } from '../../services/book.service';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { DialogService } from '../../../../../shared/components/dialog/dialog.component.service';

@Component({
  selector: 'app-chapter-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './chapter-list.component.html',
  styleUrl: './chapter-list.component.css'
})
export class ChapterListComponent implements OnInit {
  book: Book | null = null;
  chapters: Chapter[] = [];
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private toastService: ToastService,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    const bookId = this.route.snapshot.paramMap.get('bookId');
    
    if (!bookId) {
      this.toastService.error('Không tìm thấy sách của bạn');
      this.router.navigate(['/books']);
      return;
    }

    this.loadBook(bookId);
  }

  loadBook(bookId: string): void {
    this.bookService.getBook(bookId).subscribe({
      next: (book) => {
        if (!book) {
          this.toastService.error('Không tìm thấy sách của bạn');
          this.router.navigate(['/books']);
          return;
        }

        this.book = book;
        this.loadChapters(bookId);
      },
      error: (error) => {
        console.error('Error loading book:', error);
        this.toastService.error('Có lỗi trong quá trình tải sách của bạn');
        this.router.navigate(['/books']);
      }
    });
  }

  loadChapters(bookId: string): void {
    this.bookService.getChapters(bookId).subscribe({
      next: (chapters) => {
        this.chapters = chapters;
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading chapters:', error);
        this.toastService.error('Có lỗi trong quá trình tải chương');
        this.isLoading = false;
      }
    });
  }

  editChapter(chapter: Chapter): void {
    this.router.navigate(['write-book/books', chapter.bookId, 'chapters', chapter.id, 'edit']);
  }

  async deleteChapter(chapter: Chapter) {
    const dialog = await this.dialogService.open("Xác nhận xóa","Bạn có chắc chắn muốn xóa chương này không");
    if(dialog.isSuccess){
      if(chapter.id){
        this.bookService.deleteChapter(chapter.id).subscribe({
          next: (success)=>{
            if(success){
              this.toastService.success('Chương đã được xóa thành công');
              this.chapters = this.chapters.filter(c => c.id !== chapter.id);
            } 
            else {
            this.toastService.error('Không thể xóa chương này');
            }
          },
          error: (error) => {
          console.error('Error deleting chapter:', error);
          this.toastService.error('Không thể xóa chương này');
        }
        })
      }
    }
  }

  editBook(book: Book): void {
    // In a real app, this would navigate to a book edit page
    console.log("aa")
    this.toastService.info('Book editing is not implemented in this demo');
  }
}
