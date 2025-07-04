import {Component, OnInit, ViewChild, ElementRef, HostListener, OnDestroy} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {Book, BookPolicy, Chapter, PaginationBook, PolicyReadBook} from '../book/models/book.model';
import {ReaderServices} from '../book/services/reader.service';
import {PublicBookService} from '../book/services/public-book.service';
import {ToastService} from '../../shared/components/toast/toast.service';
import {OrderService} from '../admin/order/services/order.service';
import { environment } from '../../../environments/environment';
import {NgIf, NgFor} from '@angular/common';
import { DomSanitizer, SafeHtml } from '@angular/platform-browser';

@Component({
  selector: 'app-reader-book',
  imports: [
    NgIf,
    NgFor
  ],
  standalone: true,
  templateUrl: './reader-book.component.html',
  styleUrl: './reader-book.component.css'
})
export class ReaderBookComponent implements OnInit, OnDestroy {
  bookSlug!: string;
  bookId: string| undefined = undefined;
  book!: Book;
  isLoadContent = true;
  chapterSlug: string | null = null;
  selectedChapter: Chapter | undefined = undefined;
  minVersion: number = 1;
  maxVersion: number = 2;

  // UI state
  sidebarOpen = false;
  fontSize = 100; // Percentage for font size
  isScrolled = false;

  // ViewChild references
  @ViewChild('contentArea') contentArea!: ElementRef;
  @ViewChild('chapterList') chapterList!: ElementRef;

  constructor(
    private activeRouter: ActivatedRoute,
    private router: Router,
    private readerService: ReaderServices,
    private toasService: ToastService,
    private orderService: OrderService,
    private publicBookService: PublicBookService,
    private sanitizer: DomSanitizer
  ) {}

  ngOnInit(): void {
    this.chapterSlug = decodeURIComponent(this.activeRouter.snapshot.params['chapterSlug'] || '');
    // Load chapter for book
    if(history.state.book){
      this.book = history.state.book;
      this.loadChapterForBook(this.book.id, this.chapterSlug);
    } else {
      this.bookId = this.activeRouter.snapshot.params['id']
      console.log(this.activeRouter.snapshot.params)
      console.log(this.bookId);
      if(this.bookId){
        this.bookId = this.activeRouter.snapshot.params['id']
        if(this.bookId){
          this.loadBookById(this.bookId, this.chapterSlug);
        }
        else{
          this.router.navigate(['error/not-found']);
        }
      }
      else{
        this.bookSlug  = decodeURIComponent(this.activeRouter.snapshot.params['bookSlug']);
        if(this.bookSlug) {
          this.loadBookBySlug(this.bookSlug, this.chapterSlug);
        }
        else{
          this.router.navigate(['error/not-found']);
        }
      }
    }

    // Auto open sidebar initially
    this.sidebarOpen = true;
  }

  // Listen for window scroll events (for mobile compatibility)
  @HostListener('window:scroll', ['$event'])
  onWindowScroll(event: any) {
    this.handleWindowScroll();
  }

  // Listen for keyboard shortcuts
  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    // ESC to toggle sidebar
    if (event.key === 'Escape') {
      this.toggleSidebar();
    }
    // Arrow keys for navigation
    else if (event.key === 'ArrowLeft' && event.ctrlKey) {
      event.preventDefault();
      this.navigateToPreviousChapter();
    }
    else if (event.key === 'ArrowRight' && event.ctrlKey) {
      event.preventDefault();
      this.navigateToNextChapter();
    }
    // Font size shortcuts
    else if (event.key === '+' && event.ctrlKey) {
      event.preventDefault();
      this.increaseFontSize();
    }
    else if (event.key === '-' && event.ctrlKey) {
      event.preventDefault();
      this.decreaseFontSize();
    }
  }
  loadBookById(id: string, chapterSlug: string | undefined){
    if(id){
      const order = this.orderService
        .getOrderInBookIdsHasSuccess([id])
        .subscribe({
          next: results => {
            if (results) {
              const items = results[0]
                .orderItems.find(x=>x.bookId==this.bookId);
              if(items){
                // find if has public
                const bookPublic = this.publicBookService
                  .getBookInIds([items.bookId])
                  .subscribe({next: results => {
                      if(results){
                        const paginationBook = {
                          items: results
                        } as PaginationBook
                        this.publicBookService.paginationBookAggregate(paginationBook)
                        this.book = paginationBook.items[0]
                        this.loadChapterForBook(this.book.id, this.chapterSlug  ?? undefined);
                      }
                      else {
                        const book = {
                          id: items.bookId,
                          title: items.bookName,
                          policyReadBook: {
                            bookPolicy: BookPolicy.Paid,
                            price: items.price
                          } as PolicyReadBook,
                          isPayemnt: true,
                        } as Book
                        const paginationBook = {
                          items: [book]
                        } as PaginationBook
                        this.publicBookService.paginationBookAggregate(paginationBook)
                        this.book = paginationBook.items[0]
                        this.loadChapterForBook(this.book.id, this.chapterSlug  ?? undefined);
                      }}});
              }
            }
          }
        })
    }
  }
  loadBookBySlug(slug: string, chapterSlug: string | undefined) {
    const book = this.publicBookService
      .getPublicBookBySlug(slug).subscribe({
        next: (book) => {
          if(book){
            this.book = book;
            this.loadChapterForBook(this.book.id, chapterSlug);
          } else {
            this.toasService.error("Không tìm thấy sách");
          }
        },
        error: (err) => {
          this.toasService.error("Không tìm thấy sách")
          console.error(err);
        }
      })
  }

  loadChapterForBook(bookId: string, chapterSlug: string | undefined) {
    this.readerService.getChapterForBook(bookId).subscribe({
      next: (chapter) => {
        if(chapter){
          const { min, max } = chapter.reduce(
            (acc, cur) => {
              return {
                min: cur.version < acc.min ? cur.version : acc.min,
                max: cur.version > acc.max ? cur.version : acc.max,
              };
            },
            {
              min: chapter[0].version,
              max: chapter[0].version,
            }
          );
          this.minVersion = min;
          this.maxVersion = max;
          console.log(chapter);
          this.book.chapters = chapter;
          if(this.chapterSlug) {
            const chapterForSlug = this.book.chapters
              .find(x => x.slug === chapterSlug);
            if (chapterForSlug) {
              this.loadChapterContent(chapterForSlug);
            } else {
              this.toasService.error("Không thể tải chương sách");
            }
          } else {
            this.isLoadContent = false;
          }
        } else {
          this.isLoadContent = false;
          this.toasService.error("Không thể tải chương sách");
        }
      },
      error: (err) => {
        this.isLoadContent = false;
        this.toasService.error("Không thể tải chương sách");
        console.error(err);
      }
    })
  }

  loadChapterContent(chapter: Chapter) {
    this.isLoadContent = true;
    this.readerService.getChapterContent(this.book.id, chapter.slug)
      .subscribe({
        next: (result) => {
          this.isLoadContent = false;
          if(typeof result === "boolean") {
            if (result) {
              this.toasService.error("Không tìm thấy nội dung của chương bạn đã chọn");
            } else {
              // Redirect to page payment gateway
              this.paymentForBook()
            }
          } else {
            if(typeof result === "string"){
              chapter.content = result;
              this.selectedChapter = chapter;
              this.chapterSlug = chapter.slug;

              // Update URL without reloading
              this.router.navigate(['/reader', this.book.slug, chapter.slug], {
                replaceUrl: true
              });

              // Scroll to top when chapter loads - with improved timing
              this.scrollToTopWithDelay();
            } else {
              this.toasService.error("Không tìm thấy nội dung của chương bạn đã chọn");
            }
          }
        },
        error: (err) => {
          this.isLoadContent = false;
          this.toasService.error("Không tìm thấy nội dung của chương bạn đã chọn");
          console.error(err);
        }
      })
  }

  paymentForBook(){
    this.orderService.createOrder(this.book.id)
      .subscribe({
        next:(result)=>{
          if(result){
            this.orderService.paymentForOrder(result.id, `${environment.publicUrl}/reader/${this.book.slug}/${this.chapterSlug}`)
              .subscribe({
                next: (result)=>{
                  if(result){
                    window.location.href = result.paymentUrl
                  } else {
                    this.toasService.error("Có lỗi xảy ra vui lòng thử lại sau");
                  }
                },
                error: (err) => {
                  this.toasService.error("Có lỗi xảy ra vui lòng thử lại sau");
                  console.log(err);
                }
              })
          } else {
            this.toasService.error("Có lỗi xảy ra vui lòng thử lại sau");
          }
        },
        error: (err) => {
          this.toasService.error("Có lỗi xảy ra vui lòng thử lại sau");
          console.error(err);
        }
      })
  }

  // UI Methods
  toggleSidebar() {
    this.sidebarOpen = !this.sidebarOpen;
  }

  selectChapter(chapter: Chapter) {
    if (chapter.content) {
      this.selectedChapter = chapter;
      this.chapterSlug = chapter.slug;

      // Update URL without reloading
      this.router.navigate(['/reader', this.book.slug, chapter.slug], {
        replaceUrl: true
      });

      // Scroll to top with improved timing
      this.scrollToTopWithDelay();

      // Auto-close sidebar on mobile after selection
      if (window.innerWidth < 1024) {
        this.sidebarOpen = false;
      }
    } else {
      this.loadChapterContent(chapter);
    }
  }

  // New navigation methods with guaranteed scroll to top
  navigateToNextChapter() {
    const next = this.getNextChapter();
    if (next) {
      this.selectChapter(next);
    }
  }

  navigateToPreviousChapter() {
    const prev = this.getPreviousChapter();
    if (prev) {
      this.selectChapter(prev);
    }
  }

  increaseFontSize() {
    if (this.fontSize < 150) {
      this.fontSize += 10;
    }
  }

  decreaseFontSize() {
    if (this.fontSize > 80) {
      this.fontSize -= 10;
    }
  }

  sanitizeHtml(html: string): SafeHtml {
    return this.sanitizer.bypassSecurityTrustHtml(html);
  }

  getPreviousChapter(): Chapter | null {
    console.log("c")
    if (!this.book.chapters || !this.selectedChapter) return null;

    const currentIndex = this.book.chapters.findIndex(c => c.slug
      === this.selectedChapter!.slug);
    if (currentIndex > 0) {
      console.log("b")
      return this.book.chapters[currentIndex - 1];
    }
    return null;
  }

  getNextChapter(): Chapter | null {
    if (!this.book.chapters || !this.selectedChapter) return null;

    const currentIndex = this.book.chapters.findIndex(c => c.slug === this.selectedChapter!.slug);
    if (currentIndex < this.book.chapters.length - 1) {
      return this.book.chapters[currentIndex + 1];
    }
    return null;
  }

  getCurrentChapterIndex(): number {
    if (!this.book.chapters || !this.selectedChapter) return 0;
    return this.book.chapters.findIndex(c => c.slug === this.selectedChapter!.slug);
  }

  getReadingProgress(): number {
    if (!this.book.chapters || !this.selectedChapter) return 0;
    const currentIndex = this.getCurrentChapterIndex();
    return ((currentIndex + 1) / this.book.chapters.length) * 100;
  }


  private updateScrollState(): void {
    let scrollTop = 0;

    // Kiểm tra scroll của content area trước
    if (this.contentArea && this.contentArea.nativeElement) {
      scrollTop = this.contentArea.nativeElement.scrollTop;
    }
    // Nếu không có scroll trong content area, kiểm tra window scroll
    else {
      scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    }

    // Cập nhật state với threshold thấp hơn để dễ trigger
    this.isScrolled = scrollTop > 50;

    // Debug log để kiểm tra
    console.log('Scroll position:', scrollTop, 'isScrolled:', this.isScrolled);
  }

  scrollToTop(): void {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  private scrollToTopWithDelay(): void {
    // Immediate scroll
    this.performScrollToTop();

    // Additional scroll attempts with increasing delays
    setTimeout(() => this.performScrollToTop(), 50);
    setTimeout(() => this.performScrollToTop(), 150);
    setTimeout(() => this.performScrollToTop(), 300);
  }
  forceShowScrollButton(): void {
    this.isScrolled = true;
  }

  // Core scroll implementation
  private performScrollToTop(): void {
    if (this.contentArea && this.contentArea.nativeElement) {
      this.contentArea.nativeElement.scrollTo({
        top: 0,
        behavior: 'smooth'
      });
    }

    // Always also scroll window as fallback for mobile
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });

    // For browsers that don't support smooth behavior
    try {
      if (this.contentArea && this.contentArea.nativeElement) {
        this.contentArea.nativeElement.scrollTop = 0;
      }
      window.scrollTo(0, 0);
    } catch (e) {
      // Silent fallback
    }
  }

  // Mobile responsiveness
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    // Auto-close sidebar on mobile when resizing to small screen
    if (window.innerWidth < 1024 && this.sidebarOpen) {
      this.sidebarOpen = false;
    }
  }

  // Touch gesture support for mobile navigation
  private touchStartX: number = 0;
  private touchStartY: number = 0;

  @HostListener('touchstart', ['$event'])
  onTouchStart(event: TouchEvent) {
    this.touchStartX = event.touches[0].clientX;
    this.touchStartY = event.touches[0].clientY;
  }

  @HostListener('touchend', ['$event'])
  onTouchEnd(event: TouchEvent) {
    if (!event.changedTouches.length) return;

    const touchEndX = event.changedTouches[0].clientX;
    const touchEndY = event.changedTouches[0].clientY;
    const deltaX = touchEndX - this.touchStartX;
    const deltaY = touchEndY - this.touchStartY;

    // Only handle horizontal swipes that are more horizontal than vertical
    if (Math.abs(deltaX) > Math.abs(deltaY) && Math.abs(deltaX) > 50) {
      // Swipe right - previous chapter
      if (deltaX > 0 && !this.sidebarOpen) {
        this.navigateToPreviousChapter();
      }
      // Swipe left - next chapter
      else if (deltaX < 0 && !this.sidebarOpen) {
        this.navigateToNextChapter();
      }
    }
  }

  // Auto-save reading position (updated to not use localStorage as per requirements)
  saveReadingPosition(): void {
    if (this.selectedChapter && this.book) {
      const position = {
        bookSlug: this.book.slug,
        chapterSlug: this.selectedChapter.slug,
        scrollPosition: this.contentArea?.nativeElement.scrollTop || 0,
        fontSize: this.fontSize,
        timestamp: new Date().getTime()
      };
    }
  }

  // Call this when component is destroyed to save position
  ngOnDestroy(): void {
    window.removeEventListener('scroll', this.handleWindowScroll.bind(this));
    this.saveReadingPosition();
  }
  protected readonly Math = Math;
  private setupScrollListener(): void {
    // Remove existing listeners if any
    window.removeEventListener('scroll', this.handleWindowScroll.bind(this));

    // Add new listener
    window.addEventListener('scroll', this.handleWindowScroll.bind(this), { passive: true });
  }
  private handleWindowScroll(): void {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop || document.body.scrollTop || 0;
    const wasScrolled = this.isScrolled;
    this.isScrolled = scrollTop > 50; // Giảm threshold xuống 50px

    if (wasScrolled !== this.isScrolled) {
      setTimeout(() => {}, 0);
    }
  }
}
