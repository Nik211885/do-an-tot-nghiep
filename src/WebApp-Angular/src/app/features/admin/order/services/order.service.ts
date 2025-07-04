import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {OrderStatus, OrderViewModel, PaginationOrder} from '../models/order.model';
import {Observable} from 'rxjs';
import {PaymentInfo} from '../../../book/models/order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  constructor(private httpClient: HttpClient) {
  }
  getOrder(pageNumber: number, pageSize: number): Observable<PaginationOrder> {
    const url = `order/for-my/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.httpClient.get<PaginationOrder>(url);
  }
  paymentForOrder(orderId: string, returnUrl: string) : Observable<PaymentInfo> {
    const url = `order/payment?OrderId=${orderId}&ReturnUrl=${returnUrl}`;
    return this.httpClient.get<PaymentInfo>(url);
  }
  createOrder(bookId: string): Observable<OrderViewModel> {
    const url = "/order/create-order/v1";
    const body ={
      bookId:bookId,
    }
    return this.httpClient.post<OrderViewModel>(url, body);
  }
  getOrderInBookIdsHasSuccess(bookId: string[]) : Observable<OrderViewModel[]>{
    let url = "/order/my-order/book-in?";
    bookId.forEach(id=>{
      url += `bookId=${id}&`;
    });
    url += `status=${OrderStatus.Success}`;
    return this.httpClient.get<OrderViewModel[]>(url);
  }
}
