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
              path: "tag/:tag",
              loadComponent: () => import("./components/book-tag/book-tag.component").then(m => m.BookTagComponent)
            },
            {
              path: "search/:term",
              loadComponent: () => import("./components/book-search/book-search.component").then(m => m.BookSearchComponent)
            },
            {
              path: "books/id/:id",
              loadComponent: () => import("./components/book-detail/book-detail.component").then(m => m.BookDetailComponent)
            },
            {
              path: "free-books",
              loadComponent: ()=> import("./components/free-book/free-book.component").then(m => m.FreeBookComponent)
            },
            {
              path: "books",
              loadComponent: ()=> import("./components/all-book/all-book.component").then(m => m.AllBookComponent)
            },
            {
              path: "paid-books",
              loadComponent: ()=> import("./components/paid-book/paid-book.component").then(m => m.PaidBookComponent)
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
