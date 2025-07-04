import {Component, OnInit, ElementRef, ViewChild, OnDestroy} from '@angular/core';
import {PublicBookService} from '../../services/public-book.service';
import {ActivatedRoute, Router} from '@angular/router';
import {Book, PaginationBook} from '../../models/book.model';
import {NgForOf, NgIf} from '@angular/common';
import {PaginationComponent} from '../../../../shared/components/pagination/pagination.component';
import {BookCardComponent} from '../book-card/book-card.component';
import {FormsModule} from '@angular/forms';
import {debounceTime, distinctUntilChanged, filter, map, Subject, switchMap, takeUntil} from 'rxjs';

@Component({
  selector: 'app-book-search',
  imports: [
    NgIf,
    NgForOf,
    PaginationComponent,
    BookCardComponent,
    FormsModule
  ],
  standalone: true,
  templateUrl: './book-search.component.html',
  styleUrl: './book-search.component.css'
})
export class BookSearchComponent implements OnInit, OnDestroy {
  @ViewChild('searchInput') searchInput!: ElementRef;

  currentPage = 1;
  searchTerm = '';
  pageSize = 12;
  stringSuggestions: string[] = [];
  paginationBook!: PaginationBook;
  isLoading = false;
  hasSearched = false;
  showSuggestions = false;
  selectedSuggestionIndex = -1;

  // Popular search tags
/*
  popularTags = [
    'Văn học', 'Khoa học', 'Lịch sử', 'Tâm lý', 'Triết học',
    'Kinh tế', 'Công nghệ', 'Tiểu thuyết', 'Sách thiếu nhi', 'Ngoại ngữ'
  ];
*/

  private searchSubject = new Subject<string>();
  private destroy$ = new Subject<void>();

  constructor(
    private publicBookService: PublicBookService,
    private router: Router,
    private activatedRoute: ActivatedRoute
  ) {
    this.initializeSearchSubscription();
    this.initializeSuggestionSubscription();
  }

  ngOnInit(): void {
    this.activatedRoute.params.subscribe(params => {
      const term = params['term'] || '';
      this.searchTerm = term;
      if (term) {
        this.hasSearched = true;
        this.loadBookByTerm();
      }
    });

    // Close suggestions when clicking outside
    document.addEventListener('click', this.onDocumentClick.bind(this));
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
    document.removeEventListener('click', this.onDocumentClick.bind(this));
  }

  private initializeSearchSubscription(): void {
    this.searchSubject.pipe(
      debounceTime(300),
      distinctUntilChanged(),
      takeUntil(this.destroy$)
    ).subscribe(searchTerm => {
      if (searchTerm.trim()) {
        this.loadSuggestions(searchTerm.trim());
      } else {
        this.clearSuggestions();
      }
    });
  }

  private initializeSuggestionSubscription(): void {
    // Separate subscription for real-time suggestions
    this.searchSubject.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      filter(term => term.trim().length > 0),
      switchMap(term => this.publicBookService.getBookBoolPrefix(term.trim())),
      takeUntil(this.destroy$)
    ).subscribe({
      next: (suggestions: string[]) => {
        this.stringSuggestions = suggestions.slice(0, 8); // Limit to 8 suggestions
        this.selectedSuggestionIndex = -1;
      },
      error: (error) => {
        console.error('Error loading suggestions:', error);
        this.stringSuggestions = [];
      }
    });
  }

  onSearchInput(event: any): void {
    const value = event.target.value;
    this.searchTerm = value;
    this.showSuggestions = true;
    this.searchSubject.next(value);
  }

  onInputFocus(): void {
    if (this.searchTerm.trim() && this.stringSuggestions.length > 0) {
      this.showSuggestions = true;
    }
  }

  onInputBlur(): void {
    // Delay hiding suggestions to allow for clicks
    setTimeout(() => {
      this.showSuggestions = false;
    }, 200);
  }

  onKeyDown(event: KeyboardEvent): void {
    if (!this.showSuggestions || this.stringSuggestions.length === 0) {
      return;
    }

    switch (event.key) {
      case 'ArrowDown':
        event.preventDefault();
        this.selectedSuggestionIndex = Math.min(
          this.selectedSuggestionIndex + 1,
          this.stringSuggestions.length - 1
        );
        break;

      case 'ArrowUp':
        event.preventDefault();
        this.selectedSuggestionIndex = Math.max(this.selectedSuggestionIndex - 1, -1);
        break;

      case 'Enter':
        event.preventDefault();
        if (this.selectedSuggestionIndex >= 0) {
          this.selectSuggestion(this.stringSuggestions[this.selectedSuggestionIndex]);
        } else {
          this.onSearchSubmit();
        }
        break;

      case 'Escape':
        this.showSuggestions = false;
        this.selectedSuggestionIndex = -1;
        this.searchInput.nativeElement.blur();
        break;
    }
  }

  selectSuggestion(suggestion: string): void {
    this.searchTerm = suggestion;
    this.showSuggestions = false;
    this.selectedSuggestionIndex = -1;
    this.performSearch(suggestion);
  }

  private loadSuggestions(term: string): void {
    this.publicBookService.getBookBoolPrefix(term)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (suggestions: string[]) => {
          this.stringSuggestions = suggestions.slice(0, 8);
          this.showSuggestions = suggestions.length > 0;
        },
        error: (error) => {
          console.error('Error loading suggestions:', error);
          this.clearSuggestions();
        }
      });
  }

  private clearSuggestions(): void {
    this.stringSuggestions = [];
    this.showSuggestions = false;
    this.selectedSuggestionIndex = -1;
  }

  performSearch(term: string): void {
    if (!term.trim()) return;

    this.currentPage = 1;
    this.hasSearched = true;
    this.showSuggestions = false;

    // Add to search history (optional)
    this.addToSearchHistory(term);

    // Update URL
    this.router.navigate(['/search', term]);
  }

  onSearchSubmit(): void {
    if (this.searchTerm.trim()) {
      this.performSearch(this.searchTerm.trim());
    }
  }

  loadBookByTerm(): void {
    if (!this.searchTerm.trim()) return;

    this.isLoading = true;
    this.clearSuggestions();

    this.publicBookService.getBookWithPaginationForTerm(this.searchTerm, this.currentPage, this.pageSize)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: data => {
          this.isLoading = false;
          if (data) {
            this.publicBookService.paginationBookAggregate(data);
            this.paginationBook = data;
          }
        },
        error: error => {
          this.isLoading = false;
          console.error('Error loading books:', error);
          // You can add user-friendly error handling here
        }
      });
  }

  changeCurrentPage(currentPage: number): void {
    this.currentPage = currentPage;
    this.loadBookByTerm();

    // Smooth scroll to top
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }

  clearSearch(): void {
    this.searchTerm = '';
    this.hasSearched = false;
    this.paginationBook = {} as PaginationBook;
    this.clearSuggestions();
    this.router.navigate(['/search']);

    // Focus back to search input
    setTimeout(() => {
      if (this.searchInput) {
        this.searchInput.nativeElement.focus();
      }
    }, 100);
  }

  startVoiceSearch(): void {
    // Check if browser supports speech recognition
    if ('webkitSpeechRecognition' in window || 'SpeechRecognition' in window) {
      const SpeechRecognition = (window as any).webkitSpeechRecognition || (window as any).SpeechRecognition;
      const recognition = new SpeechRecognition();

      recognition.lang = 'vi-VN';
      recognition.continuous = false;
      recognition.interimResults = false;
      recognition.maxAlternatives = 1;

      recognition.onstart = () => {
        console.log('Voice recognition started');
        // You can add visual feedback here
      };

      recognition.onresult = (event: any) => {
        const transcript = event.results[0][0].transcript;
        this.searchTerm = transcript;
        this.performSearch(transcript);
      };

      recognition.onerror = (event: any) => {
        console.error('Speech recognition error:', event.error);
        // Handle error - show user-friendly message
      };

      recognition.onend = () => {
        console.log('Voice recognition ended');
      };

      recognition.start();
    } else {
      alert('Trình duyệt của bạn không hỗ trợ tìm kiếm bằng giọng nói');
    }
  }

  private addToSearchHistory(term: string): void {
    try {
      const history = JSON.parse(localStorage.getItem('searchHistory') || '[]');
      const updatedHistory = [term, ...history.filter((item: string) => item !== term)].slice(0, 10);
      localStorage.setItem('searchHistory', JSON.stringify(updatedHistory));
    } catch (error) {
      console.error('Error saving search history:', error);
    }
  }

  private getSearchHistory(): string[] {
    try {
      return JSON.parse(localStorage.getItem('searchHistory') || '[]');
    } catch (error) {
      console.error('Error loading search history:', error);
      return [];
    }
  }

  private onDocumentClick(event: Event): void {
    const target = event.target as HTMLElement;
    if (!target.closest('.relative')) {
      this.showSuggestions = false;
    }
  }


}
