import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AfterViewInit, Component, ElementRef, Inject, OnDestroy, OnInit, PLATFORM_ID, Renderer2, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { Book, Chapter } from '../../models/book.model';
import {applyDelta,applyDeltaJson,getDelta,undoDiff, undoDiffJson} from "../../../../../core/utils/diff-content.until"
import { Subscription } from 'rxjs';
import { BookService } from '../../services/book.service';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { ChapterVersion, ChapterVersionComponent } from "./chapter-version/chapter-version.component";
import { ChapterVersionService } from '../../services/chapter.service';
import { ChapterDiffComponent } from './chapter-diff/chapter-diff.component';
import {DialogService, InputDialogOptions} from '../../../../../shared/components/dialog/dialog.component.service';

@Component({
  selector: 'app-chapter-editor',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, CKEditorModule,
     RouterModule, ChapterVersionComponent,
    ChapterDiffComponent],
  templateUrl: './chapter-editor.component.html',
  styleUrl: './chapter-editor.component.css'
})
export class ChapterEditorComponent implements OnInit,
OnDestroy, AfterViewInit {
  Editor = signal<any>(null);
  editorConfig = {
    toolbar: {
      items: [
        'heading',
        '|',
        'bold',
        'italic',
        'underline',
        'strikethrough',
        'subscript',
        'superscript',
        'highlight',
        'fontColor',
        'fontBackgroundColor',
        'fontSize',
        'fontFamily',
        '|',
        'link',
        'linkDecorator',
        'bulletedList',
        'numberedList',
        'outdent',
        'indent',
        'alignment',
        '|',
        'blockQuote',
        'code',
        'codeBlock',
        'horizontalLine',
        'specialCharacters',
        '|',
        'insertTable',
        'tableColumn',
        'tableRow',
        'mergeTableCells',
        '|',
        'imageInsert',
        'imageUpload',
        'mediaEmbed',
        '|',
        'removeFormat',
        'undo',
        'redo',
        '|',
        'pasteFromOffice',
        'autoformat'
      ]
    },
    image: {
      toolbar: [
        'imageTextAlternative',
        'imageStyle:full',
        'imageStyle:side'
      ]
    },
    table: {
      contentToolbar: [
        'tableColumn',
        'tableRow',
        'mergeTableCells',
        'tableProperties',
        'tableCellProperties'
      ]
    },
    placeholder: 'Bắt đầu viết những chương sách của bạn...'
  } as any;


  book: Book | null = null;
  chapter: Chapter | null = null;
  chapterForm: FormGroup | null = null;
  isLoading = true;
  isSaving = false;
  isVersionPanelClosed = false;
  chapterVersion: ChapterVersion[] = [];
  private renderer!: Renderer2;
  private el!: ElementRef;
  isBrowser: boolean;
  isEditing = false;
  chapterId: string | null = null;
  private subscription: Subscription | null = null;

  currentChapterDiff: ChapterVersion | null = null;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private bookService: BookService,
    private dialogServies: DialogService,
    private chapterVersionService: ChapterVersionService,
    private toastService: ToastService,
    @Inject(PLATFORM_ID) private platformId: Object
  ) {
     this.isBrowser = isPlatformBrowser(this.platformId);
  }

  async ngOnInit() {
    if(this.isBrowser){
      const CKEditor = await import('@ckeditor/ckeditor5-build-classic');
      this.Editor.set(CKEditor.default);
      console.log("ckeditor" + this.Editor());
    }
    const bookId = this.route.snapshot.paramMap.get('bookId');
    this.chapterId = this.route.snapshot.paramMap.get('chapterId');

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

        if (this.chapterId) {
          this.isEditing = true;
          this.loadChapter(this.chapterId);
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
    if(this.chapterId){
      this.chapterVersion = this.chapterVersionService.getChapterVersionByChaoterId(this.chapterId);
    }
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
  getChapterVersionByBookId(){
    if(this.chapterId){
      this.chapterVersion = this.chapterVersionService.getChapterVersionByChaoterId(this.chapterId);
    }
  }
  toggleVersionPanel(): void {
    this.isVersionPanelClosed = !this.isVersionPanelClosed;

    // Thêm class vào body để animate khi panel thay đổi trạng thái
    if (this.isVersionPanelClosed) {
      document.body.classList.add('panel-closing');
      setTimeout(() => {
        document.body.classList.remove('panel-closing');
      }, 300); // Thời gian bằng với duration của animation
    } else {
      document.body.classList.add('panel-opening');
      setTimeout(() => {
        document.body.classList.remove('panel-opening');
      }, 300);
    }
  }

   private applyCKEditorWordWrapping(editor: any) {
    if (!this.isBrowser || !editor) return;

    // Get the editing view
    const editorElement = editor.ui.getEditableElement();
    if (!editorElement) return;

    // Add a custom style element with high specificity
    const styleElement = document.createElement('style');
    styleElement.innerHTML = `
      .ck.ck-editor__editable,
      .ck.ck-editor__editable > *,
      .ck-content,
      .ck-content > * {
        word-break: normal !important;
        overflow-wrap: break-word !important;
        word-wrap: break-word !important;
        white-space: pre-wrap !important;
        max-width: 100% !important;
      }
    `;

    // Add the style element to document head
    document.head.appendChild(styleElement);

    // Directly apply inline styles to the editor for maximum effectiveness
    this.renderer.setStyle(editorElement, 'word-break', 'normal');
    this.renderer.setStyle(editorElement, 'overflow-wrap', 'break-word');
    this.renderer.setStyle(editorElement, 'white-space', 'pre-wrap');

    // Set up a mutation observer to ensure styles are maintained
    // This helps if CKEditor overwrites styles after initialization
    if (window.MutationObserver) {
      const observer = new MutationObserver(() => {
        const paragraphs = editorElement.querySelectorAll('p, div, li, h1, h2, h3, h4, h5, h6');
        paragraphs.forEach((element: HTMLElement) => {
          this.renderer.setStyle(element, 'word-break', 'normal');
          this.renderer.setStyle(element, 'overflow-wrap', 'break-word');
          this.renderer.setStyle(element, 'white-space', 'pre-wrap');
          this.renderer.setStyle(element, 'max-width', '100%');
        });
      });

      observer.observe(editorElement, {
        childList: true,
        subtree: true,
        characterData: true,
        attributes: true
      });
    }
  }

  ngAfterViewInit() {
    // Wait for DOM to be ready and apply styles for the editor
    if (this.isBrowser) {
      setTimeout(() => {
        const editorElements = document.querySelectorAll('.ck-editor__editable');
        editorElements.forEach((el) => {
          this.renderer.setStyle(el, 'word-break', 'normal');
          this.renderer.setStyle(el, 'overflow-wrap', 'break-word');
          this.renderer.setStyle(el, 'white-space', 'pre-wrap');
        });
      }, 500);
    }
  }
  onChapterVersionClicked(chapterVersion: ChapterVersion) {
    this.currentChapterDiff = (chapterVersion);
  }
  closeDiffContent(){
    this.currentChapterDiff = (null);
  }
  async renameChapterVersion(chapterVersion: ChapterVersion){
    const options: InputDialogOptions = {
      title: 'Đặt tên phiên bản chương',
      inputs:[
        {
          name: 'name',
          label: 'Tên phiên bản',
          type: 'text',
          value: chapterVersion.name,
          required: true,
          validators: [
            {
              type: 'required',
              message: 'Tên phiên bản không được để trống'
            },
          ]
        }
      ]
    }
    const renameChapterVersionDialog = await this.dialogServies.openInputDialog(options);
    if(renameChapterVersionDialog.isSuccess){
      this.dialogServies.close();
      this.toastService.success("Bạn đã đổi tên version thành công");
      console.log(renameChapterVersionDialog.data);
    }
  }
  rollBackChapterVersion(chapterVersionId: string) {
    this.toastService.success("Bạn đã khôi phục phiên bản thành công");
    console.log(chapterVersionId);
  }
}
