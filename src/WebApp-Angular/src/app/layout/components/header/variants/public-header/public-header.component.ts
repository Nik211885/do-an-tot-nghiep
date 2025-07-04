import { Component, ElementRef, HostListener, Input, OnInit, Renderer2, signal, ViewChild } from '@angular/core';
import { HeaderComponent } from '../../header.component';
import { Router, RouterLink, RouterModule } from '@angular/router';
import {
  trigger,
  transition,
  style,
  animate
} from '@angular/animations';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../../../../core/auth/auth.service';
import { UserModel } from '../../../../../core/models/user.model';

@Component({
  standalone: true,
  selector: 'app-public-header',
  animations: [
    trigger('dropdownAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'translateY(-10px)' }),
        animate('200ms ease-out', style({ opacity: 1, transform: 'translateY(0)' }))
      ]),
      transition(':leave', [
        animate('150ms ease-in', style({ opacity: 0, transform: 'translateY(-10px)' }))
      ])
    ]),
    trigger('overlayAnimation', [
      // Animation khi element xuất hiện (enter)
      transition(':enter', [
        style({ transform: 'translateY(-100%)', opacity: 0 }), // Bắt đầu từ vị trí trên cùng và opacity bằng 0
        animate('500ms ease-out', style({ transform: 'translateY(0)', opacity: 1 })) // Di chuyển xuống và opacity từ từ tăng lên
      ]),
      // Animation khi element biến mất (leave)
      transition(':leave', [
        animate('500ms ease-in', style({ transform: 'translateY(-100%)', opacity: 0 })) // Trượt lên và giảm opacity
      ])
    ])
  ],
  imports: [HeaderComponent, CommonModule, RouterLink, RouterModule],
  templateUrl: './public-header.component.html',
  styleUrl: './public-header.component.css'
})
export class PublicHeaderComponent implements OnInit {
  @ViewChild('searchInput') searchInput!: ElementRef;
  isAuthenticated = false;
  userModel = signal<UserModel | null>(null);
  isMobileMenuOpen: boolean = false;
  isSearchOverlayOpen: boolean = false;
  searchResults: any[] = [];
 /* searchHistory: string[] = ['Harry Potter', 'Đắc Nhân Tâm', 'Sherlock Holmes'];*/
  searchQuery: string = '';

  @Input() variant: 'public' | 'authenticated' = 'public';

  constructor(
    private renderer: Renderer2,
    private router: Router,
    private authService: AuthService
  ) {}

  ngOnInit(){
    // Thêm event listener để đóng menu khi click vào overlay
    this.renderer.listen('document', 'click', (event: Event) => {
      const target = event.target as HTMLElement;

      // Chỉ xử lý khi mobile menu đang mở
      if (this.isMobileMenuOpen) {
        // Kiểm tra nếu click vào overlay (phần tử có class overlay)
        const clickedOnOverlay = target.classList.contains('bg-opacity-50');

        // Hoặc click bên ngoài mobile menu container
        const clickedOutsideMenu = !target.closest('.fixed.bottom-0') &&
          !target.closest('.lg\\:hidden') && // hamburger button
          !target.closest('app-header');

        if (clickedOnOverlay || clickedOutsideMenu) {
          this.isMobileMenuOpen = false;
          this.renderer.removeClass(document.body, 'overflow-hidden');
        }
      }
    });

    // Thêm event listener cho sự kiện vuốt lên để đóng mobile menu
    this.setupSwipeDetection();

    this.authService.initialize().subscribe((intialize)=>{
      if(intialize){
        this.isAuthenticated = this.authService.isAuthenticated();
        this.authService.getCurrentUser().subscribe((user)=>{
          this.userModel.set(user);
        });
      }
    })
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

   /* if (query.length >= 2) {
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
    }*/
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
   /* if (!this.searchHistory.includes(this.searchQuery) && this.searchQuery) {
      this.searchHistory.unshift(this.searchQuery);
      // Giữ tối đa 5 lịch sử tìm kiếm
      if (this.searchHistory.length > 5) {
        this.searchHistory.pop();
      }*/
  /*  }*/

/*    // Đóng overlay
    this.isSearchOverlayOpen = false;*/

    // Điều hướng đến trang chi tiết sách
    this.router.navigate(['/books', result.id]);
  }

 /* // Xóa một mục khỏi lịch sử tìm kiếm
  removeHistoryItem(index: number, event: Event): void {
    event.stopPropagation();
    this.searchHistory.splice(index, 1);
  }*/

/*  // Xóa tất cả lịch sử tìm kiếm
  clearSearchHistory(): void {
    this.searchHistory = [];
  }*/
  login(){
    this.authService.login("");
  }
  logout(){
    this.authService.logout();
  }
  isMenuOpen = false;

  toggleMenu() {
    this.isMenuOpen = !this.isMenuOpen;
  }
  closeMenu() {
    this.isMenuOpen = false;
  }
  @HostListener('document:click', ['$event.target'])
    onDocumentClick(target: HTMLElement) {
    const clickedInsideDropdown = target.closest('.user-menu-wrapper');
    const clickedInsideHeader = target.closest('app-header');

    if (!clickedInsideDropdown && !clickedInsideHeader) {
      this.isMenuOpen = false;
    }
  }
  closeMobileMenuOnNavigation(): void {
    if (this.isMobileMenuOpen) {
      this.isMobileMenuOpen = false;
      this.renderer.removeClass(document.body, 'overflow-hidden');
    }
  }

  search() {
    this.closeSearchOverlay();
    this.router.navigate(['/search', this.searchQuery]);
  }
}
