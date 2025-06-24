import { Component, OnInit } from '@angular/core';
import {OrderService} from '../../services/order.service';
import {PaginationOrder} from '../../models/order.model';
import {PublicBookService} from '../../../../book/services/public-book.service';
import {Book} from '../../../../book/models/book.model';

@Component({
  selector: 'app-order-book',
  imports: [],
  templateUrl: './order-book.component.html',
  styleUrl: './order-book.component.css'
})
export class OrderBookComponent implements OnInit {
  paginationOrder!: PaginationOrder;
  book!: Book[];
  currentPag=  1;
  pageSize = 5;
  constructor(private order: OrderService,
              private readonly publicBookService: PublicBookService) {
  }
  ngOnInit(): void {
      this.loadOrderBooks();
  }
  loadOrderBooks(): void {
    this.order.getOrder(this.currentPag,this.pageSize)
      .subscribe(order => {
        this.paginationOrder = order;
        const ids = this.paginationOrder.items.flatMap(item =>
          item.orderItems.map(o => o.bookId)
        );
        console.log(this.paginationOrder);
        if(ids) {
          this.loadBookInOrder(ids);
        }
      })
  }
  loadBookInOrder(ids: string[]): void {
    this.publicBookService.getBookInIds(ids)
      .subscribe(bookInOrder => {
        this.book = bookInOrder;
      })
  }

}
