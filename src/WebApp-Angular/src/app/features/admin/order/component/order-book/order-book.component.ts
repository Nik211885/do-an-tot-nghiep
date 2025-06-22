import { Component, OnInit } from '@angular/core';
import {OrderService} from '../../services/order.service';
import {PaginationOrder} from '../../models/order.model';

@Component({
  selector: 'app-order-book',
  imports: [],
  templateUrl: './order-book.component.html',
  styleUrl: './order-book.component.css'
})
export class OrderBookComponent implements OnInit {
  paginationOrder!: PaginationOrder;
  constructor(private order: OrderService) {
  }
    ngOnInit(): void {
        this.order.getOrder(1,10).subscribe(order => {
          console.log(order);
          this.paginationOrder = order;
        })
    }

}
