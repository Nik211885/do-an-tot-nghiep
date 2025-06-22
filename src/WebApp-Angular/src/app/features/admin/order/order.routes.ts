import {Route} from '@angular/router';
import {AdminLayoutComponent} from '../../../layout/layouts/admin-layout/admin-layout.component';

export const orderRoutes: Route[] = [
  {
    path: "order",
    component: AdminLayoutComponent,
    children:[
      {
        path:'book',
        loadComponent:()=> import("./component/order-book/order-book.component").then(m=>m.OrderBookComponent),
      }
    ]
  }
]
