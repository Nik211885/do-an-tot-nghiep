import { CommonModule, isPlatformBrowser } from '@angular/common';
import { AfterViewInit, Component, ElementRef, Inject, OnDestroy, OnInit, PLATFORM_ID, Renderer2, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CKEditorModule } from '@ckeditor/ckeditor5-angular';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import {Bookv1, Chapter, ChapterVersion} from '../../models/book.model';
import { Subscription } from 'rxjs';
import { BookService } from '../../services/book.service';
import { ToastService } from '../../../../../shared/components/toast/toast.service';
import { ChapterVersionComponent } from "./chapter-version/chapter-version.component";
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


  book: Bookv1 | null = null;
  chapter= signal<Chapter | null>(null);
  chapterForm: FormGroup | null = null;
  isLoading = true;
  isSaving = false;
  isVersionPanelClosed = true;
  chapterVersion = signal<ChapterVersion[] | null>(null);
  private renderer!: Renderer2;
  private el!: ElementRef;
  isBrowser: boolean;
  isEditing = false;
  chapterSlug?: string = undefined;
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
    const bookSlug = this.route.snapshot.paramMap.get('bookSlug');
    this.chapterSlug = this.route.snapshot.paramMap.get('chapterSlug') ?? undefined;
    if (!bookSlug) {
      this.toastService.error('Không tìm thấy sách của bạn');
      this.router.navigate(['/write-book/books']);
      return;
    }

    this.bookService.getBookBySlug(bookSlug).subscribe({
      next: (book) => {
        if (!book) {
          this.toastService.error('Không tìm thấy sách của bạn');
          this.router.navigate(['/write-book/books']);
          return;
        }

        this.book = book;

        if (this.chapterSlug) {
          this.isEditing = true;
          this.loadChapter(this.chapterSlug);
        } else {
          this.initializeForm();
        }
      },
      error: (error) => {
        console.error('Error loading book:', error);
        this.toastService.error('Không thể tải dữ liệu sách');
        this.router.navigate(['/write-book/books']);
      }
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  loadChapter(chapterSlug: string): void {
    this.bookService.getChapter(chapterSlug).subscribe({
      next: (chapter) => {
        if (!chapter) {
          this.toastService.error('Không tìm thấy chương sách');
          this.router.navigate(['/write-book/books', this.book?.slug]);
          return;
        }

        this.chapter.set(chapter);
        this.initializeForm();
      },
      error: (error) => {
        console.error('Error loading chapter:', error);
        this.toastService.error('Không thể tải chương sách');
        this.router.navigate(['/write-book/books', this.book?.slug]);
      }
    });
  }

  initializeForm(): void {
    this.bookService.getChapters(this.book!.slug!).subscribe(chapters => {
      // If creating a new chapter, suggest the next chapter number
      let suggestedChapterNumber = 1;
      if (!this.isEditing && chapters.length > 0) {
        suggestedChapterNumber = Math.max(...chapters.map(c => c.chapterNumber)) + 1;
      }

      this.chapterForm = this.fb.group({
        title: [this.chapter()?.title || '', Validators.required],
        chapterNumber: [
          this.chapter()?.chapterNumber || suggestedChapterNumber,
          [Validators.required, Validators.min(1)]
        ],
        content: [this.chapter()?.content || '', Validators.required]
      });

      this.isLoading = false;
    });
  }

  submitAndReview() : void{
    const chapter = this.chapter();
    if(chapter && chapter.id){
      this.bookService.submitAndReview(chapter.id).subscribe({
        next: (chapter) => {
          this.chapter.set(chapter);
          this.toastService.success("Gửi chương để xuất bản sách thành công");
        }
      })
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

    if (this.isEditing && this.chapter()?.id) {
      chapterData.id = this.chapter()?.id;
    }

    const saveAction = this.isEditing
      ? this.bookService.updateChapter(chapterData)
      : this.bookService.createChapter(chapterData);

    saveAction.subscribe({
      next: (savedChapter: Chapter) => {
        this.toastService.success('Đã lưu lại thành công');
        this.isSaving = false;
        this.chapterVersion.set(savedChapter.chapterVersion);
        if (!this.isEditing) {
          this.isEditing = true;
          this.chapterSlug = savedChapter.slug;
          this.chapter.set(savedChapter);
        } else {
          this.chapter.set(savedChapter);
        }

        this.chapterForm?.patchValue({
          title: savedChapter.title,
          chapterNumber: savedChapter.chapterNumber,
          content: savedChapter.content
        });
      },
      error: (error) => {
        console.error('Error saving draft:', error);
        this.toastService.error('Có lỗi trong quá trình lưu lại');
        this.isSaving = false;
      }
    });
  }


  onCancel(): void {
    this.router.navigate(['/write-book/books', this.book?.slug]);
  }
  toggleVersionPanel(): void {
    console.log(this.chapterVersion);
    this.isVersionPanelClosed = !this.isVersionPanelClosed;
    if(!this.chapterVersion.length && this.chapter){
      this.chapterVersionService.getChapterVersionByChapterId(this.chapter()?.id!)
        .subscribe({
          next: (version) => {
            this.chapterVersion.set(version);
            console.log(version);
          },
          error: (error) => {
            console.log('Error loading chapter version:', error);
          }
        })
    }

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
      chapterVersion.name = renameChapterVersionDialog.data.name;
      console.log(renameChapterVersionDialog.data);
      this.chapterVersionService.renameChapterVersion(chapterVersion,this.chapter()?.id!)
        .subscribe({
          next: result => {
            if(result === true){
              this.dialogServies.close();
              this.toastService.success("Bạn đã đổi tên version thành công");
            }
            else{
              this.toastService.error(typeof result === 'string' ? result : 'Không thể đổi  tên phiên bản lúc này  vui lòng  thử laị')
            }
          },
          error: (error) => {
            console.error('Error deleting chapter:', error);
            this.toastService.error('Không thể đổi  tên phiên bản lúc này  vui lòng  thử laị');
          }
        })
    }
  }
  rollBackChapterVersion([chapterVersionId, chapterId]: [string, string]) {
    this.bookService.rollbackChapter(chapterVersionId, chapterId).subscribe({
      next: result => {
        this.chapter.set(result);
        this.chapterVersion.set(result.chapterVersion);
        this.chapterForm?.patchValue({
          title: result.title,
          chapterNumber: result.chapterNumber,
          content: result.content
        });
        this.toastService.success("Bạn đã khôi phục phiên bản thành công");
      },
      error: (error) => {
        console.error('Error rollback chapter:', error);
      }
    })
  }
}
