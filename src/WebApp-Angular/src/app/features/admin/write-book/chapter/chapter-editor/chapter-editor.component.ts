import { CommonModule, isPlatformBrowser } from '@angular/common';
import { Component, Inject, OnDestroy, OnInit, PLATFORM_ID, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import * as ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Book, Chapter } from '../../models/book.model';
import { Subscription } from 'rxjs';
import { BookService } from '../../services/book.service';
import { ToastService } from '../../../../../shared/components/toast/toast.service';

@Component({
  selector: 'app-chapter-editor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, CKEditorModule, RouterModule],
  templateUrl: './chapter-editor.component.html',
  styleUrl: './chapter-editor.component.css'
})
export class ChapterEditorComponent implements OnInit, OnDestroy {
  Editor = signal<any>(null); 
  public editorConfig = {
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'link',
        'bulletedList',
        'numberedList',
        '|',
        'indent',
        'outdent',
        '|',
        'blockQuote',
        'insertTable',
        'undo',
        'redo'
      ]
    },
    placeholder: 'Start writing your chapter content here...'
  };

  book: Book | null = null;
  chapter: Chapter | null = null;
  chapterForm: FormGroup | null = null;
  isLoading = true;
  isSaving = false;
  isBrowser: boolean;
  isEditing = false;
  private subscription: Subscription | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private toastService: ToastService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
     this.isBrowser = isPlatformBrowser(this.platformId);
  }

  async ngOnInit() {
    if(this.isBrowser){
      const CKEditor = await import('@ckeditor/ckeditor5-build-classic');
      this.Editor.set(CKEditor.default);
      console.log(this.Editor());
    }
    const bookId = this.route.snapshot.paramMap.get('bookId');
    const chapterId = this.route.snapshot.paramMap.get('chapterId');
    
    if (!bookId) {
      this.toastService.error('Không tìm thấy sách của bạn');
      this.router.navigate(['/books']);
      return;
    }

    this.bookService.getBook(bookId).subscribe({
      next: (book) => {
        if (!book) {
          this.toastService.error('Không tìm thấy sách của bạn');
          this.router.navigate(['/books']);
          return;
        }

        this.book = book;
        
        if (chapterId) {
          this.isEditing = true;
          this.loadChapter(chapterId);
        } else {
          this.initializeForm();
        }
      },
      error: (error) => {
        console.error('Error loading book:', error);
        this.toastService.error('Không thể tải dữ liệu sách');
        this.router.navigate(['/books']);
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  loadChapter(chapterId: string): void {
    this.bookService.getChapter(chapterId).subscribe({
      next: (chapter) => {
        if (!chapter) {
          this.toastService.error('Không tìm thấy chương sách');
          this.router.navigate(['/books', this.book?.id]);
          return;
        }

        this.chapter = chapter;
        this.initializeForm();
      },
      error: (error) => {
        console.error('Error loading chapter:', error);
        this.toastService.error('Không thể tải chương sách');
        this.router.navigate(['/books', this.book?.id]);
      }
    });
  }

  initializeForm(): void {
    this.bookService.getChapters(this.book!.id!).subscribe(chapters => {
      // If creating a new chapter, suggest the next chapter number
      let suggestedChapterNumber = 1;
      if (!this.isEditing && chapters.length > 0) {
        suggestedChapterNumber = Math.max(...chapters.map(c => c.chapterNumber)) + 1;
      }

      this.chapterForm = this.fb.group({
        title: [this.chapter?.title || '', Validators.required],
        chapterNumber: [
          this.chapter?.chapterNumber || suggestedChapterNumber, 
          [Validators.required, Validators.min(1)]
        ],
        content: [this.chapter?.content || '', Validators.required]
      });

      this.isLoading = false;
    });
  }

  onSubmit(): void {
    if (this.chapterForm?.invalid || !this.book?.id) {
      return;
    }

    this.isSaving = true;
    const chapterData: Chapter = {
      ...(this.chapterForm ? this.chapterForm.value : {}),
      bookId: this.book.id
    };

    if (this.isEditing && this.chapter?.id) {
      chapterData.id = this.chapter.id;
      this.bookService.updateChapter(chapterData).subscribe({
        next: () => {
          this.toastService.success('Cập nhật thành công');
          this.router.navigate(['/write-book/books', this.book?.id]);
        },
        error: (error) => {
          console.error('Error updating chapter:', error);
          this.toastService.error('Không thể cập nhập chương sách');
          this.isSaving = false;
        }
      });
    } else {
      this.bookService.createChapter(chapterData).subscribe({
        next: () => {
          this.toastService.success('Chương được xuất bản thành công');
          this.router.navigate(['/write-book/books', this.book?.id]);
        },
        error: (error) => {
          console.error('Error creating chapter:', error);
          this.toastService.error('Chương xuất bản thất bại');
          this.isSaving = false;
        }
      });
    }
  }

  onSaveDraft(): void {
    if (!this.book?.id || !this.chapterForm) {
      return;
    }

    this.isSaving = true;
    const chapterData: Chapter = {
      ...this.chapterForm.value,
      bookId: this.book.id
    };

    if (this.isEditing && this.chapter?.id) {
      chapterData.id = this.chapter.id;
    }

    // For a draft we're just saving locally without validating 
    // In a real app, you would probably have a draft status field
    this.bookService[this.isEditing ? 'updateChapter' : 'createChapter'](chapterData).subscribe({
      next: () => {
        this.toastService.success('Đã lưu lại thành công');
        this.isSaving = false;
      },
      error: (error) => {
        console.error('Error saving draft:', error);
        this.toastService.error('Có lỗi trong quá trình lưu lại');
        this.isSaving = false;
      }
    });
  }

  onCancel(): void {
    this.router.navigate(['/write-book/books', this.book?.id]);
  }
}
