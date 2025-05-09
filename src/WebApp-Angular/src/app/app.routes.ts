import { Routes } from '@angular/router';
import {erorRoutes} from "./features/error-pages/error-pages.routes";
import { AdminLayoutComponent } from './layout/layouts/admin-layout/admin-layout.component';
import { homeRoutes } from './features/home/home.routes';
export const routes: Routes = [
    {
        path: "admin",
        component: AdminLayoutComponent,
    },
    ...erorRoutes,
    ...homeRoutes,
    { path: '**', redirectTo:"error/not-found"},
];
