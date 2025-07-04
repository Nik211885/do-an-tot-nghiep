import {Component, OnInit} from '@angular/core';
import {OrderService} from '../../services/order.service';
import {OrderItemViewModel, OrderStatus, OrderViewModel, PaginationOrder} from '../../models/order.model';
import {DatePipe, DecimalPipe, formatDate, NgForOf, NgIf} from '@angular/common';
import {Router} from '@angular/router';

@Component({
  selector: 'app-order-book',
  imports: [
    NgIf,
    NgForOf,
    DatePipe
  ],
  templateUrl: './order-book.component.html',
  styleUrl: './order-book.component.css'
})
export class OrderBookComponent implements OnInit {
  paginationOrder!: PaginationOrder;
  currentPage=  1;
  pageSize = 1;
  constructor(private order: OrderService,
              private router: Router) {
  }
  ngOnInit(): void {
      this.loadOrderBooks();
  }
  loadOrderBooks(): void {
    this.order.getOrder(this.currentPage,this.pageSize)
      .subscribe(order => {
        this.paginationOrder = order;
      })
  }
  totalPrice(order: OrderViewModel): string {
    const total = order.orderItems.reduce((sum, item) => sum + Number(item.price), 0);
    return new Intl.NumberFormat('vi-VN', { maximumFractionDigits: 0 }).format(total);
  }

  protected readonly formatDate = formatDate;
  getLabelForOrderStatus(order: OrderViewModel){
    switch (order.status){
      case OrderStatus.Success:
        return "Thành công";
      case OrderStatus.Pending:
        return "Chở thanh toán";
      case OrderStatus.Canceled:
        return "Hủy thanh toán";
      case  OrderStatus.Failure:
        return "Thất bại";
      default:
        return "Thất bại"
    }
  }
  previousPage(): void {
    if (this.currentPage > 1) {
      this.currentPage--;
      this.loadOrderBooks();
    }
  }
  getVisiblePages(): (number | string)[] {
    const totalPages = this.paginationOrder?.totalPages ?? 1;
    const current = this.currentPage;
    const pages: (number | string)[] = [];

    if (totalPages <= 7) {
      // If total pages <= 7, show all pages
      for (let i = 1; i <= totalPages; i++) {
        pages.push(i);
      }
    } else {
      // Always show first page
      pages.push(1);

      if (current <= 4) {
        // Current page is near the beginning
        for (let i = 2; i <= 5; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(totalPages);
      } else if (current >= totalPages - 3) {
        // Current page is near the end
        pages.push('...');
        for (let i = totalPages - 4; i <= totalPages; i++) {
          pages.push(i);
        }
      } else {
        // Current page is in the middle
        pages.push('...');
        for (let i = current - 1; i <= current + 1; i++) {
          pages.push(i);
        }
        pages.push('...');
        pages.push(totalPages);
      }
    }

    return pages;
  }
  isCurrentPage(page: number | string): boolean {
    return typeof page === 'number' && page === this.currentPage;
  }

  isClickablePage(page: number | string): boolean {
    return typeof page === 'number';
  }
  nextPage(): void {
    if (this.currentPage < (this.paginationOrder?.totalPages ?? 1)) {
      this.currentPage++;
      this.loadOrderBooks();
    }
  }
  goToPage(page: number): void {
    if (page >= 1 && page <= (this.paginationOrder?.totalPages ?? 1) && page !== this.currentPage) {
      this.currentPage = page;
      this.loadOrderBooks();
    }
  }

  protected readonly Math = Math;
  protected readonly Number = Number;

  viewBookDetail(b: OrderItemViewModel, event: Event) {
    event.stopPropagation();
    this.router.navigate(['/books/id', b.bookId]);
  }
}
