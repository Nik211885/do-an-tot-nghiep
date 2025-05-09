import { Routes } from "@angular/router";
import { PublicLayoutComponent } from "../../layout/layouts/public-layout/public-layout.component";

export const homeRoutes: Routes = [
    {
        path:"",
        component: PublicLayoutComponent,
        children: [
            {
                path: "",
                loadComponent: () => import("../home/home.component").then(m => m.HomeComponent)
            },
            {
                path: "books/:slug",
                loadComponent: () => import("./components/book-detail/book-detail.component").then(m => m.BookDetailComponent)
            }
        ]
    }
]