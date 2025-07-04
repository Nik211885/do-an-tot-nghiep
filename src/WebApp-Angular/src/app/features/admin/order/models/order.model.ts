import {Book} from '../../../book/models/book.model';

export interface OrderItemViewModel{
  id: string;
  bookId: string,
  bookName: string,
  price: number,
}

export interface OrderViewModel{
  id: string;
  orderDate: Date,
  buyerId: string,
  status: OrderStatus,
  orderItems: OrderItemViewModel[]
}


export enum OrderStatus {
  Pending = 'Pending',
  Success = 'Success',
  Failure = 'Failure',
  Canceled = 'Canceled'
}

export interface PaginationOrder{
  items: OrderViewModel[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
