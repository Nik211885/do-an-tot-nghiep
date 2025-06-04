import {CommonModule} from '@angular/common';
import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router, RouterModule} from '@angular/router';
import {Bookv1, Chapter} from '../../models/book.model';
import {BookService} from '../../services/book.service';
import {ToastService} from '../../../../../shared/components/toast/toast.service';
import {DialogService} from '../../../../../shared/components/dialog/dialog.component.service';
import {BookReleaseType} from '../../models/create-book.model';

@Component({
  selector: 'app-chapter-list',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './chapter-list.component.html',
  styleUrl: './chapter-list.component.css'
})
export class ChapterListComponent implements OnInit {
  book: Bookv1 | null = null;
  chapters: Chapter[] = [];
  bookSlug?: string;
  isLoading = true;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private toastService: ToastService,
    private dialogService: DialogService
  ) {}

  ngOnInit(): void {
    this.bookSlug = this.route.snapshot.paramMap.get('slug') ?? undefined;
    if (!this.bookSlug) {
      this.toastService.error('Không tìm thấy sách của bạn');
      this.router.navigate(['/books']);
      return;
    }

    this.loadBook(this.bookSlug);
  }

  loadBook(bookSlug: string): void {
    this.bookService.getBookBySlug(bookSlug).subscribe({
      next: (book) => {
        if (!book) {
          this.toastService.error('Không tìm thấy sách của bạn');
          this.router.navigate(['/books']);
          return;
        }

        this.book = book;
        console.log(this.book);
        this.loadChapters(bookSlug);
      },
      error: (error) => {
        console.error('Error loading book:', error);
        this.toastService.error('Có lỗi trong quá trình tải sách của bạn');
        this.router.navigate(['/books']);
      }
    });
  }

  loadChapters(bookSlug: string): void {
    this.bookService.getChapters(bookSlug).subscribe({
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
    if (this.bookSlug) {
      this.router.navigate(['write-book/books', this.bookSlug, 'chapters', chapter.slug, 'edit']);
    }
  }

  async deleteChapter(chapter: Chapter) {
    const dialog = await this.dialogService.open("Xác nhận xóa", "Bạn có chắc chắn muốn xóa chương này không?");

    if (dialog.isSuccess && chapter.id) {
      console.log("Calling API to delete chapter with ID:", chapter.id);

      this.bookService.deleteChapter(chapter.id).subscribe({
        next: (result) => {
          console.log("API result:", result);
          if (result === true) {
            this.toastService.success('Chương đã được xóa thành công');
            this.chapters = this.chapters.filter(c => c.id !== chapter.id);
          } else {
            this.toastService.error(typeof result === 'string' ? result : 'Không thể xóa chương này');
          }
        },
        error: (error) => {
          console.error('Error deleting chapter:', error);
          this.toastService.error('Không thể xóa chương này');
        }
      });
    } else {
      console.log("Không gọi API vì dialog bị hủy hoặc thiếu chapter.id");
    }
  }
  editBook(book: Bookv1): void {
    this.router.navigate(['write-book/books-information', book.id])
  }
  markBook(): void{
    if(this.book){
      this.book.isCompleted = !this.book.isCompleted;
      if(this.book.id) {
        this.bookService.markBookComplete(this.book.id).subscribe({
          next: (result) => {
            this.toastService.success("Sách được đánh dấu thành công")
          },
          error: err => {
            console.log(err);
           if(this.book){
             this.book.isCompleted = !this.book.isCompleted;
           }
          }
        })
      }
    }
  }
}
