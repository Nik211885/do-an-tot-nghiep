import { Component, ElementRef, HostListener, Input, OnInit, Renderer2, ViewChild } from '@angular/core';
import { HeaderComponent } from '../../header.component';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-public-header',
  imports: [HeaderComponent, CommonModule],
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css'
})
export class PublicHeaderComponent implements OnInit {
  @ViewChild('searchInput') searchInput!: ElementRef;
  
  isMobileMenuOpen: boolean = false;
  isSearchOverlayOpen: boolean = false;
  searchResults: any[] = [];
  searchHistory: string[] = ['Harry Potter', 'Đắc Nhân Tâm', 'Sherlock Holmes'];
  searchQuery: string = '';
  
  @Input() variant: 'public' | 'authenticated' = 'public';

  constructor(
    private renderer: Renderer2,
    private router: Router
  ) {}

  ngOnInit(): void {
    // Thêm event listener để đóng menu khi click vào overlay
    this.renderer.listen('document', 'click', (event: Event) => {
      const target = event.target as HTMLElement;
      // Kiểm tra nếu click vào overlay (không phải menu hay các phần tử trong header)
      if (this.isMobileMenuOpen && 
          !target.closest('.fixed.bottom-0') && 
          !target.closest('button')) {
        this.isMobileMenuOpen = false;
      }
    });

    // Thêm event listener cho sự kiện vuốt lên để đóng mobile menu
    this.setupSwipeDetection();
  }

  // Thiết lập phát hiện cử chỉ vuốt để tăng UX cho mobile
  setupSwipeDetection(): void {
    let touchStartY: number;
    let touchEndY: number;
    
    this.renderer.listen('document', 'touchstart', (event: TouchEvent) => {
      touchStartY = event.touches[0].clientY;
    });
    
    this.renderer.listen('document', 'touchend', (event: TouchEvent) => {
      touchEndY = event.changedTouches[0].clientY;
      
      // Phát hiện vuốt xuống để đóng menu
      if (this.isMobileMenuOpen && touchEndY - touchStartY > 70) {
        this.isMobileMenuOpen = false;
      }
    });
  }

  toggleMobileMenu(): void {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
    
    // Đóng search overlay nếu đang mở
    if (this.isSearchOverlayOpen && this.isMobileMenuOpen) {
      this.isSearchOverlayOpen = false;
    }
    
    // Ngăn chặn cuộn trang khi menu mobile mở
    if (this.isMobileMenuOpen) {
      this.renderer.addClass(document.body, 'overflow-hidden');
    } else {
      this.renderer.removeClass(document.body, 'overflow-hidden');
    }
  }

  toggleSearchOverlay(): void {
    this.isSearchOverlayOpen = !this.isSearchOverlayOpen;
    
    // Đóng mobile menu nếu đang mở
    if (this.isMobileMenuOpen && this.isSearchOverlayOpen) {
      this.isMobileMenuOpen = false;
    }
    
    // Focus vào ô tìm kiếm khi mở overlay
    if (this.isSearchOverlayOpen) {
      setTimeout(() => {
        this.searchInput?.nativeElement.focus();
      }, 300);
    }
  }

  onSearchInput(event: Event): void {
    const query = (event.target as HTMLInputElement).value;
    this.searchQuery = query;
    
    if (query.length >= 2) {
      // Giả lập kết quả tìm kiếm - trong thực tế bạn sẽ gọi API
      this.searchResults = [
        { id: 1, title: 'Harry Potter và Hòn Đá Phù Thủy', author: 'J.K. Rowling' },
        { id: 2, title: 'Đắc Nhân Tâm', author: 'Dale Carnegie' },
        { id: 3, title: 'Sherlock Holmes: Vụ Án Baskervilles', author: 'Arthur Conan Doyle' }
      ].filter(book => 
        book.title.toLowerCase().includes(query.toLowerCase()) ||
        book.author.toLowerCase().includes(query.toLowerCase())
      );
    } else {
      this.searchResults = [];
    }
  }

  // Đóng overlay search khi nhấn ESC
  @HostListener('document:keydown.escape')
  closeSearchOverlay(): void {
    if (this.isSearchOverlayOpen) {
      this.isSearchOverlayOpen = false;
    }
    
    if (this.isMobileMenuOpen) {
      this.isMobileMenuOpen = false;
    }
  }

  // Giả lập chọn một kết quả tìm kiếm
  selectSearchResult(result: any): void {
    // Lưu vào lịch sử tìm kiếm
    if (!this.searchHistory.includes(this.searchQuery) && this.searchQuery) {
      this.searchHistory.unshift(this.searchQuery);
      // Giữ tối đa 5 lịch sử tìm kiếm
      if (this.searchHistory.length > 5) {
        this.searchHistory.pop();
      }
    }
    
    // Đóng overlay
    this.isSearchOverlayOpen = false;
    
    // Điều hướng đến trang chi tiết sách
    this.router.navigate(['/books', result.id]);
  }

  // Xóa một mục khỏi lịch sử tìm kiếm
  removeHistoryItem(index: number, event: Event): void {
    event.stopPropagation();
    this.searchHistory.splice(index, 1);
  }

  // Xóa tất cả lịch sử tìm kiếm
  clearSearchHistory(): void {
    this.searchHistory = [];
  }
}
