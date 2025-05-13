import { Routes } from '@angular/router';
import {erorRoutes} from "./features/error-pages/error-pages.routes";
import { AdminLayoutComponent } from './layout/layouts/admin-layout/admin-layout.component';
import { bookRoutes } from './features/book/book.routes';
import {writeBook} from "./features/admin/write-book/write-book.routes"
export const routes: Routes = [
    {
        path: "admin",
        component: AdminLayoutComponent,
    },
    ...writeBook,
    ...erorRoutes,
    ...bookRoutes,
    { path: '**', redirectTo:"error/not-found"},
];
