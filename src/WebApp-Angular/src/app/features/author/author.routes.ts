import { Routes } from "@angular/router";
import { PublicLayoutComponent } from "../../layout/layouts/public-layout/public-layout.component";

export const authorRoutes: Routes = [
    {
        path: "author",
        component: PublicLayoutComponent,
        children:[
            {
                path: "",
                loadComponent: ()=> import("./components/author-list/author-list.component").then(c=>c.AuthorListComponent)
            },
            {
                path: ":id",
                loadComponent: ()=> import("./components/author-detail/author-detail.component").then(c=>c.AuthorDetailComponent)
            }
        ]
    }
]