import { Routes } from "@angular/router";
import { PublicLayoutComponent } from "../../layout/layouts/public-layout/public-layout.component";

export const bookRoutes: Routes = [
    {
        path:"",
        component: PublicLayoutComponent,
        children: [
            {
                path: "",
                loadComponent: () => import("../book/book.component").then(m => m.HomeComponent)
            },
            {
                path: "books/:slug",
                loadComponent: () => import("./components/book-detail/book-detail.component").then(m => m.BookDetailComponent)
            },
            {
                path: 'genere',
                children: [
                    {
                        path: '',
                        loadComponent: () => import('./components/genere-list/genere-list.component').then(c => c.GenereListComponent)
                    },
                    {
                        path: ':slug',
                        loadComponent: () => import('./components/genere-detail/genere-detail.component').then(c => c.GenereDetailComponent)
                    }
                ]
            }

        ]
    }
]