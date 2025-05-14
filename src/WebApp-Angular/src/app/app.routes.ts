import { Routes } from '@angular/router';
import {erorRoutes} from "./features/error-pages/error-pages.routes";
import { AdminLayoutComponent } from './layout/layouts/admin-layout/admin-layout.component';
import { bookRoutes } from './features/book/book.routes';
import {writeBook} from "./features/admin/write-book/write-book.routes"
import {statisPageRoutes} from "./features/static-pages/statis-pages.routes"
export const routes: Routes = [
    {
        path: "admin",
        component: AdminLayoutComponent,
    },
    ...writeBook,
    ...erorRoutes,
    ...bookRoutes,
    ...statisPageRoutes,
    { path: '**', redirectTo:"error/not-found"},
];
