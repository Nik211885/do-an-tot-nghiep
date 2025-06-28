import { Routes } from '@angular/router';
import {erorRoutes} from "./features/error-pages/error-pages.routes";
import { AdminLayoutComponent } from './layout/layouts/admin-layout/admin-layout.component';
import { bookRoutes } from './features/book/book.routes';
import {writeBook} from "./features/admin/write-book/write-book.routes"
import {statisPageRoutes} from "./features/static-pages/statis-pages.routes"
import { authorRoutes } from './features/author/author.routes';
import { moderationRoutes } from './features/admin/moderation/moderation.routes';
import {userProfileRoutes} from './features/admin/user-profile/user-profile.routes';
import {orderRoutes} from './features/admin/order/order.routes';
import {ReaderLayoutComponent} from './layout/layouts/reader-layout/reader-layout.component';
export const routes: Routes = [
  {
    path:"admin",
    component: AdminLayoutComponent
  },
  {
    path: 'reader',
    component: ReaderLayoutComponent,
    children: [
      {
        path:":bookSlug",
        loadComponent:()=>import("./features/reader-book/reader-book.component").then(x=>x.ReaderBookComponent)
      },
      {
        path:":bookSlug/:chapterSlug",
        loadComponent:()=>import("./features/reader-book/reader-book.component").then(x=>x.ReaderBookComponent)
      },
    ]
  },
    ...authorRoutes,
    ... moderationRoutes,
    ... orderRoutes,
    ...writeBook,
    ...erorRoutes,
    ...bookRoutes,
    ...statisPageRoutes,
    ... userProfileRoutes,
  /*  { path: '**', redirectTo:"error/not-found"},*/
];
