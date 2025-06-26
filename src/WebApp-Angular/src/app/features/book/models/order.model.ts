export interface OrderItem{
  bookId: string;
  bookName: string;
  price: number;
}


export interface Order{
  orderDate: Date,
  buyerId: string,
  status: OrderStatus,
  orderItems: OrderItem[]
}


export enum OrderStatus{
  Pending = 0,
  Success = 1,
  Failure = 2,
  Canceled = 3,
}


export interface PaymentInfo{
  success: boolean,
  paymentUrl: string,
  qrCodeUrl: string,
  deeplink: string,
  applink: string,
  message: string,
}
