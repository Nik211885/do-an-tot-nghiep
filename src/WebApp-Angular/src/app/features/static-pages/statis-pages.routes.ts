import {Routes} from "@angular/router"
import { PublicLayoutComponent } from "../../layout/layouts/public-layout/public-layout.component"
export const statisPageRoutes: Routes = [
    {
        path: "static-page",
        component: PublicLayoutComponent,
        children:[
            {
                path: "about-us",
                loadComponent: ()=> import("./about-us/about-us.component").then(c=>c.AboutUsComponent)
            }
        ]
    }
]