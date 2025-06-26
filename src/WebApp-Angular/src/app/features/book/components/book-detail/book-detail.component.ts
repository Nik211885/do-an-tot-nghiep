import {Component, OnInit, TemplateRef, ViewChild} from '@angular/core';
import {Book, BookPolicy, BookReleaseType, PaginationBook} from '../../models/book.model'
import {ActivatedRoute, Router} from '@angular/router';
import {CommonModule} from '@angular/common';
import {BookGenreTagComponent} from './book-genre-tag/book-genre-tag.component';
import {BookChapterListComponent} from './book-chapter-list/book-chapter-list.component';
import {BookPolicyBadgeComponent} from './book-policy-badge/book-policy-badge.component';
import {StarRatingComponent} from './star-rating/star-rating.component';
import {CommentBookSectionComponent} from "./comment-book-section/comment-book-section.component";
import {DialogService} from '../../../../shared/components/dialog/dialog.component.service';
import {RatingFormComponent} from './rating-form/rating-form.component';
import {RatingService} from '../../services/rating.service';
import {PublicBookService} from '../../services/public-book.service';
import {RatingViewModel} from '../../models/rating.model';
import {ToastService} from '../../../../shared/components/toast/toast.service';
import {OrderService} from '../../../admin/order/services/order.service';
import {OrderStatus, OrderViewModel} from '../../../admin/order/models/order.model';
import {PaymentInfo} from '../../models/order.model';
import {environment} from '../../../../../environments/environment';

@Component({
  selector: 'app-book-detail',
  standalone: true,
  imports: [
    CommonModule,
    BookGenreTagComponent,
    BookPolicyBadgeComponent,
    BookChapterListComponent,
    // BookCommentSectionComponent,
    RatingFormComponent,
    StarRatingComponent,
    CommentBookSectionComponent
],
  templateUrl: './book-detail.component.html',
  styleUrl: './book-detail.component.css'
})
export class BookDetailComponent implements OnInit {
  @ViewChild('ratingDialog') ratingDialogTemplate!: TemplateRef<any>;
  @ViewChild('ratingForm') ratingForm!: RatingFormComponent;
  rating: number = 5;
  ratingViewModel: RatingViewModel | null = null;
  book!: Book;
  BookPolicy = BookPolicy;
  BookReleaseType = BookReleaseType;
  order: OrderViewModel | null = null;
  paymentInfo: PaymentInfo | null = null;
  isDescriptionExpanded = false;
  paymentLoadding = false;
  activeTab: 'chapters' | 'comments' = 'chapters';
  constructor(private route: ActivatedRoute,
   private ratingService: RatingService,
   private toastService: ToastService,
   private dialogService: DialogService,
    private orderService: OrderService,
    private publicBookService: PublicBookService,
    private router: Router) {

  }
  ngOnInit(): void {
    if(history.state.book){
      this.book = history.state.book;
    }
    else{
      const bookSlug = decodeURIComponent(this.route.snapshot.paramMap.get('slug') || '');
      this.loadBookBySlug(bookSlug);
    }
  }
  loadBookBySlug(slug: string) {
    this.publicBookService.getPublicBookBySlug(slug)
      .subscribe({
        next: data => {
          if(data){
            const pagination = {
              items: [data]
            } as PaginationBook;
            this.publicBookService.paginationBookAggregate(pagination);
            this.book = pagination.items[0];
          }
          else{
            this.router.navigate(["error/not-found"])
          }
        },
        error : error => {
          this.toastService.error("Có lỗi xảy ra vui lòng thực hiện lại");
        }
      })
  }
  get genreNames(): string {
    return (this.book.genres ?? []).map(g => g.name).join(', ');
  }

  /**
   * Toggles the favorite state
   */
  toggleFavorite(e: Event): void {
    e.preventDefault();
    e.stopPropagation();
    this.book.isFavorite = !this.book.isFavorite;
    if(this.book.isFavorite){
      this.publicBookService.favoriteBook(this.book.id)
        .subscribe();
    }
    else{
      this.publicBookService.unFavoriteBook(this.book.id)
        .subscribe();
    }
  }

  /**
   * Toggles the description expanded state
   */
  toggleDescription(): void {
    this.isDescriptionExpanded = !this.isDescriptionExpanded;
  }

  /**
   * Sets the active tab
   */
  setActiveTab(tab: 'chapters' | 'comments'): void {
    this.activeTab = tab;
  }

  /**
   * Returns the appropriate status label based on book completion and release type
   */
  getStatusLabel(): string {
    if (this.book.isCompeleted) {
      return 'Hoàn thành';
    }
    return this.book.bookReleaseType === BookReleaseType.Serialized ? 'Đang ra' : 'Sắp ra mắt';
  }

  /**
   * Returns the appropriate status color class based on book status
   */
  getStatusColor(): string {
    if (this.book.isCompeleted) {
      return 'bg-emerald-100 text-emerald-800';
    }
    return this.book.bookReleaseType === BookReleaseType.Serialized
      ? 'bg-blue-100 text-blue-800'
      : 'bg-amber-100 text-amber-800';
  }

  /**
   * Returns the appropriate CTA button text based on book policy
   */
  getCtaButtonText(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'Đọc Ngay';
      case BookPolicy.Paid:
        if(this.book.isPayemnt){
          return 'Đọc Ngay';
        }
        return `Mua ${this.book.policyReadBook.price?.toLocaleString('vi-VN')}đ`;
      case BookPolicy.Subscription:
        return 'Đăng Ký Để Đọc';
      default:
        return 'Đọc Ngay';
    }
  }

  /**
   * Returns the appropriate CTA button color based on book policy
   */
  getCtaButtonColor(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'bg-emerald-600 hover:bg-emerald-700';
      case BookPolicy.Paid:
        return 'bg-amber-600 hover:bg-amber-700';
      case BookPolicy.Subscription:
        return 'bg-indigo-600 hover:bg-indigo-700';
      default:
        return 'bg-emerald-600 hover:bg-emerald-700';
    }
  }

  /**
   * Returns the policy display text
   */
  getPolicyLabel(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'Miễn phí';
      case BookPolicy.Paid:
        return 'Trả phí';
      case BookPolicy.Subscription:
        return 'Đăng ký';
      default:
        return 'Miễn phí';
    }
  }

  /**
   * Returns the appropriate policy text color
   */
  getPolicyTextColor(): string {
    switch (this.book.policyReadBook.bookPolicy) {
      case BookPolicy.Free:
        return 'text-emerald-600';
      case BookPolicy.Paid:
        return 'text-amber-600';
      case BookPolicy.Subscription:
        return 'text-indigo-600';
      default:
        return 'text-emerald-600';
    }
  }

  /**
   * Returns a random percentage for the rating distribution visualization
   */
  getRandomPercentage(): number {
    return Math.floor(Math.random() * 100);
  }

  openRatingDialog() {
    this.publicBookService.getMyRatingForBook(this.book.id).subscribe({
      next: async result => {
        if (result) {
          this.ratingViewModel = result;
          this.rating = this.ratingViewModel.star
        }
        await this.processRating();
      }
    });
  }

  async processRating(){
    const dialogOptions = {
      title: 'Đánh giá sách',
      size: 'md' as const,
      context: {
        rating: 5,
      },
      customContent: this.ratingDialogTemplate,
      confirmButtonText: 'Gửi đánh giá',
      cancelButtonText: 'Hủy bỏ',
      showCancelButton: true
    };

    // Mở dialog
    const dialog = await this.dialogService.open(dialogOptions);
    if (dialog.isSuccess) {
      const isValid = this.ratingForm.submitRating();

      if (isValid) {
        if(this.ratingViewModel){
          // Update
          this.publicBookService.updateRating(this.ratingViewModel.id, this.ratingForm.rating)
            .subscribe({
              next: result => {
                if(result) {
                  this.showSuccessMessage();
                }
                else{
                  this.showErrorMessage();
                }
              },
              error: result => {
                console.log(result);
                this.showErrorMessage();
              }
            })
        }
        else{
          // Add
          this.publicBookService.createRatingBook(this.book.id, this.ratingForm.rating)
            .subscribe({
              next: result => {
                if(result) {
                  this.showSuccessMessage();
                }
                else{
                  this.showErrorMessage();
                }
              },
              error: result => {
                console.log(result);
                this.showErrorMessage();
              }
            })
        }
      }
    } else {
      this.ratingForm.resetForm();
    }
  }
  showSuccessMessage(): void {
    this.dialogService.open({
      title: 'Thành công',
      message: 'Cảm ơn bạn đã đánh giá sản phẩm!',
      size: 'sm',
      showCancelButton: false,
      confirmButtonText: 'Đóng'
    });
  }

  showErrorMessage(): void {
    this.dialogService.open({
      title: 'Lỗi',
      message: 'Có lỗi xảy ra khi gửi đánh giá. Vui lòng thử lại sau.',
      size: 'sm',
      showCancelButton: false,
      confirmButtonText: 'Đóng'
    });
  }

  actionForBook() {
    if(this.book.policyReadBook.bookPolicy === BookPolicy.Free){

    }
    else {
      if(this.book.policyReadBook.bookPolicy === BookPolicy.Paid){
        this.paymentLoadding = true;
        this.orderService.createOrder(this.book.id)
          .subscribe({
            next: (result)=>{
              this.order = result;
              this.orderService.paymentForOrder(this.order.id, `${environment.publicUrl}/books/${this.book.slug}`)
                .subscribe({
                  next: (result)=>{
                    if(result){
                      this.paymentInfo = result;
                      window.location.href = this.paymentInfo.paymentUrl
                    }
                  },
                  error: result => {
                    this.paymentLoadding = false;
                    this.toastService.error("Có lỗi xãy ra")
                    console.error(result);
                  }
                })
            },
            error: result => {
              this.paymentLoadding = false;
              this.toastService.error("Có lỗi xãy ra")
              console.error(result);
            }
          })
      }
    }
  }
}
