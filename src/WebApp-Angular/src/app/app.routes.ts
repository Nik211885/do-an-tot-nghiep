import { Routes } from '@angular/router';
import {erorRoutes} from "./features/error-pages/error-pages.routes";
import { PublicLayoutComponent } from './layout/layouts/public-layout/public-layout.component';
import { NotFoundComponent } from './features/error-pages/not-found/not-found.component';
export const routes: Routes = [
    {
        path:"",
        component: PublicLayoutComponent,
    },
    { path: '**', redirectTo:"error/not-found"},
    ...erorRoutes,
];
