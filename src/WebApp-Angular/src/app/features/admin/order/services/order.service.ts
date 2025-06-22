import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {OrderViewModel, PaginationOrder} from '../models/order.model';
import {Observable} from 'rxjs';

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
}
