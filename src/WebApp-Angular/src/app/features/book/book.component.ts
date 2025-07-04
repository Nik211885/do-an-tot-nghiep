import {
  AfterViewInit,
  Component,
  DestroyRef,
  ElementRef,
  HostListener,
  OnDestroy,
  OnInit,
  ViewChild
} from '@angular/core';
import {BookCardComponent} from "./components/book-card/book-card.component";
import {CommonModule} from '@angular/common';
import {PublicBookService} from './services/public-book.service';
import {Book, BookPolicy, Genre, PaginationBook} from './models/book.model';
import {UserProfileService} from '../admin/user-profile/services/user-profile.service';
import {RouterLink} from '@angular/router';

export interface BookTopForGenre{
  top: number;
  genres: Genre;
  books: Book[];
}

@Component({
  selector: 'app-home',
  imports: [BookCardComponent, CommonModule, RouterLink],
  templateUrl: './book.component.html',
  styleUrl: './book.component.css'
})
export class HomeComponent implements OnInit, AfterViewInit, OnDestroy {
  // Sample data for the book
  currentPage: number = 1;
  pageSize: number = 5;
  bookNew: Book[] = [];
  bookFree: Book[] = [];
  bookPaid: Book[] = [];
  booksTopView: Book[] = [];
  yourBook: Book[] =[];
  favoriteBooks: Book[] = [];
  currentTop: number = 0;
  markEndTopGenres : boolean = false;
  ///top
  // genres
  // book
  bookTopForGenres: BookTopForGenre[] = [];
  isLoadingFavorite: boolean = false;
  isLoadingNew: boolean = false;
  isLoadingFree: boolean = false;
  isLoadingPaid: boolean = false;
  isLoadingTopView: boolean = false;
  isLoadingYourBook: boolean = false;
  isLoadingGenre: boolean = false;
  isLoadingMore: boolean = false;

  // Scroll and lazy loading
  showScrollTop: boolean = false;
  sectionsLoaded: Set<string> = new Set();

  // Slideshow properties
  currentSlide: number = 0;
  private autoSlideInterval: any;
  private isAutoPlaying: boolean = true;
  slideInterval: any;
  heroSlides = [
    {
      image: 'https://i.pinimg.com/736x/5b/44/1b/5b441b1de06ad9adfd30cd3411aec2d7.jpg',
      title: 'Khám Phá Thế Giới Sách',
      description: 'Hàng nghìn cuốn sách hay đang chờ bạn khám phá',
      category: 'Nổi bật'
    },
    {
      image: 'https://i.pinimg.com/736x/98/33/49/983349d01872469b434b896eda4f79da.jpg',
      title: 'Đọc Sách Miễn Phí',
      description: 'Trải nghiệm đọc sách không giới hạn với hàng nghìn đầu sách miễn phí',
      category: 'Miễn phí'
    },
    {
      image: 'https://i.pinimg.com/736x/2b/89/60/2b89608c8f9f7cef7f19d507e917156a.jpg',
      title: 'Sách Mới Nhất',
      description: 'Cập nhật những cuốn sách mới nhất từ các tác giả nổi tiếng',
      category: 'Mới nhất'
    }
  ];
  @ViewChild('favoriteSection', { static: false }) favoriteSection!: ElementRef;
  @ViewChild('newBookSection', { static: false }) newBookSection!: ElementRef;
  @ViewChild('freeBookSection', { static: false }) freeBookSection!: ElementRef;
  @ViewChild('paidBookSection', { static: false }) paidBookSection!: ElementRef;
  @ViewChild('topViewSection', { static: false }) topViewSection!: ElementRef;
  @ViewChild('yourBookSection', { static: false }) yourBookSection!: ElementRef;

  private intersectionObserver!: IntersectionObserver;
  constructor(private publicBookService: PublicBookService,
              private userProfileService: UserProfileService) {
  }
  ngOnInit() {
    this.initializeSlideshow();
    this.setupIntersectionObserver();
    this.loadFavoriteBook();
    /*this.startAutoSlide();*/// Load initial content
  }

  ngOnDestroy() {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
    }
    if (this.intersectionObserver) {
      this.intersectionObserver.disconnect();
    }
  }

  ngAfterViewInit() {
    this.observeSections();
  }

  // Slideshow Methods
  initializeSlideshow() {
    this.slideInterval = setInterval(() => {
      this.nextSlide();
    }, 5000);
  }

  nextSlide() {
    this.currentSlide = (this.currentSlide + 1) % this.heroSlides.length;
  }

  previousSlide() {
    this.currentSlide = this.currentSlide === 0
      ? this.heroSlides.length - 1
      : this.currentSlide - 1;
  }

  goToSlide(index: number) {
    this.currentSlide = index;
  }
  /*private startAutoSlide() {
    if (this.isAutoPlaying) {
      this.autoSlideInterval = setInterval(() => {
        this.nextSlide();
      }, 5000); // Chuyển slide mỗi 5 giây
    }
  }*/
  /*private stopAutoSlide() {
    if (this.autoSlideInterval) {
      clearInterval(this.autoSlideInterval);
      this.autoSlideInterval = null;
    }
  }*/
  /*toggleAutoPlay() {
    this.isAutoPlaying = !this.isAutoPlaying;
    if (this.isAutoPlaying) {
      this.startAutoSlide();
    } else {
      this.stopAutoSlide();
    }
  }*/

  // Intersection Observer Setup
  setupIntersectionObserver() {
    const options = {
      root: null,
      rootMargin: '100px',
      threshold: 0.1
    };

    this.intersectionObserver = new IntersectionObserver((entries) => {
      entries.forEach(entry => {
        if (entry.isIntersecting) {
          const sectionId = entry.target.getAttribute('data-section-id');
          if (sectionId && !this.sectionsLoaded.has(sectionId)) {
            this.loadSectionData(sectionId);
            this.sectionsLoaded.add(sectionId);
          }
        }
      });
    }, options);
  }

  observeSections() {
    setTimeout(() => {
      const sections = [
        { element: this.favoriteSection?.nativeElement, id: 'favorite' },
        { element: this.newBookSection?.nativeElement, id: 'new' },
        { element: this.freeBookSection?.nativeElement, id: 'free' },
        { element: this.paidBookSection?.nativeElement, id: 'paid' },
        { element: this.topViewSection?.nativeElement, id: 'topview' },
        { element: this.yourBookSection?.nativeElement, id: 'yourbook' }
      ];

      sections.forEach(section => {
        if (section.element) {
          section.element.setAttribute('data-section-id', section.id);
          this.intersectionObserver.observe(section.element);
        }
      });
    }, 100);
  }

  loadSectionData(sectionId: string) {
    switch (sectionId) {
      case 'new':
        if (this.bookNew.length === 0) {
          this.loadNewBook();
        }
        break;
      case 'free':
        if (this.bookFree.length === 0) {
          this.loadFreeBook();
        }
        break;
      case 'paid':
        if (this.bookPaid.length === 0) {
          this.loadPaidBook();
        }
        break;
      case 'topview':
        if (this.booksTopView.length === 0) {
          this.loadBooksTopView();
        }
        break;
      case 'yourbook':
        if (this.yourBook.length === 0) {
          this.loadYourBook();
        }
        break;
    }
  }

  // Enhanced loading methods with loading states
  loadNewBook() {
    if (this.isLoadingNew) return;

    this.isLoadingNew = true;
    this.publicBookService.getAllBookWithPagination(this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if (result) {
            this.publicBookService.paginationBookAggregate(result);
            this.bookNew = result.items;
          }
          this.isLoadingNew = false;
        },
        error: () => {
          this.isLoadingNew = false;
        }
      });
  }

  loadFreeBook() {
    if (this.isLoadingFree) return;

    this.isLoadingFree = true;
    this.publicBookService.getBookByPolicy(BookPolicy.Free, this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if (result) {
            this.publicBookService.paginationBookAggregate(result);
            this.bookFree = result.items;
          }
          this.isLoadingFree = false;
        },
        error: () => {
          this.isLoadingFree = false;
        }
      });
  }

  loadPaidBook() {
    if (this.isLoadingPaid) return;

    this.isLoadingPaid = true;
    this.publicBookService.getBookByPolicy(BookPolicy.Paid, this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if (result) {
            this.publicBookService.paginationBookAggregate(result);
            this.bookPaid = result.items;
          }
          this.isLoadingPaid = false;
        },
        error: () => {
          this.isLoadingPaid = false;
        }
      });
  }

  loadBooksTopView() {
    if (this.isLoadingTopView) return;

    this.isLoadingTopView = true;
    this.publicBookService.getBookReviewHasTopView(this.pageSize).subscribe({
      next: result => {
        if (result) {
          var ids = result.map(x => x.bookId);
          this.publicBookService.getBookInIds(ids)
            .subscribe({
              next: value => {
                if (value) {
                  const pagination = {
                    items: value,
                  } as PaginationBook;
                  this.publicBookService.paginationBookAggregate(pagination);
                  this.booksTopView = pagination.items;
                }
                this.isLoadingTopView = false;
              },
              error: () => {
                this.isLoadingTopView = false;
              }
            });
        } else {
          this.isLoadingTopView = false;
        }
      },
      error: () => {
        this.isLoadingTopView = false;
      }
    });
  }

  loadYourBook() {
    if (this.isLoadingYourBook) return;

    this.isLoadingYourBook = true;
    this.publicBookService.getMyBook(this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if (result) {
            this.publicBookService.paginationBookAggregate(result);
            this.yourBook = result.items;
          }
          this.isLoadingYourBook = false;
        },
        error: () => {
          this.isLoadingYourBook = false;
        }
      });
  }

  loadFavoriteBook() {
    if (this.isLoadingFavorite) return;

    this.isLoadingFavorite = true;
    this.userProfileService.getFavoriteBook(this.currentPage, this.pageSize)
      .subscribe({
        next: result => {
          if (result) {
            var bookItems = result.items.map(x => x.favoriteBookId);
            this.publicBookService.getBookInIds(bookItems)
              .subscribe({
                next: result => {
                  if (result) {
                    const pagination = {
                      items: result
                    } as PaginationBook;
                    this.publicBookService.paginationBookAggregate(pagination);
                    this.favoriteBooks = pagination.items;
                  }
                  this.isLoadingFavorite = false;
                },
                error: () => {
                  this.isLoadingFavorite = false;
                }
              });
          } else {
            this.isLoadingFavorite = false;
          }
        },
        error: () => {
          this.isLoadingFavorite = false;
        }
      });
  }

  loadBookTopForGenre() {
    if (this.isLoadingGenre || this.markEndTopGenres) return;

    this.isLoadingGenre = true;
    this.currentTop += 1;

    this.publicBookService.getTopGenres(this.currentTop - 1)
      .subscribe({
        next: result => {
          if (result) {
            const genres = result;
            this.publicBookService.getBookByGenreId(genres.slug, this.currentPage, this.pageSize)
              .subscribe({
                next: result => {
                  if (result) {
                    this.publicBookService.paginationBookAggregate(result);
                    const book = result.items;
                    this.bookTopForGenres.push({
                      top: this.currentTop,
                      genres: genres,
                      books: book
                    });
                  }
                  this.isLoadingGenre = false;
                },
                error: () => {
                  this.isLoadingGenre = false;
                }
              });
          } else {
            this.markEndTopGenres = true;
            this.isLoadingGenre = false;
          }
        },
        error: () => {
          this.isLoadingGenre = false;
        }
      });
  }

  // Scroll handling
  @HostListener('window:scroll', ['$event'])
  onWindowScroll() {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;
    this.showScrollTop = scrollTop > 500;

    // Load more genre sections when near bottom
    const scrollHeight = document.documentElement.scrollHeight;
    const clientHeight = document.documentElement.clientHeight;
    const scrolledToBottom = scrollTop + clientHeight >= scrollHeight - 1000;

    if (scrolledToBottom && !this.isLoadingGenre && !this.markEndTopGenres) {
      this.loadBookTopForGenre();
    }
  }

  scrollToTop() {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  // TrackBy functions for better performance
  trackByBookId(index: number, book: Book): any {
    return book.id;
  }

  trackByGenreId(index: number, genreSection: BookTopForGenre): any {
    return genreSection.genres.id;
  }

  // Mouse events for slideshow
  onSlideMouseEnter() {
    if (this.slideInterval) {
      clearInterval(this.slideInterval);
    }
  }

  onSlideMouseLeave() {
    this.initializeSlideshow();
  }
}
