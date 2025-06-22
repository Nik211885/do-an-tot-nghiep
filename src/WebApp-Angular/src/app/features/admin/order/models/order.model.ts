export interface OrderItemViewModel{
  id: string,
  bookName: string,
  price: number
}

export interface OrderViewModel{
  orderDate: Date,
  buyerId: string,
  status: OrderStatus,
  orderItems: OrderItemViewModel[]
}


export enum OrderStatus {
  Pending = 0,
  Success = 1,
  Failure,
  Canceled
}

export interface PaginationOrder{
  items: OrderItemViewModel[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
