import { Routes } from "@angular/router";
import { ErrorLayoutComponent } from "../../layout/layouts/error-layout/error-layout.component";
import { UnauthorizedComponent } from "./unauthorized/unauthorized.component";
import { NotFoundComponent } from "./not-found/not-found.component";
import { InternalServerComponent } from "./internal-server/internal-server.component";
import { ErrorComponent } from "./error/error.component";
import { ForbiddenComponent } from "./forbidden/forbidden.component";

export const erorRoutes: Routes = [
    {
        path:"error",
        component: ErrorLayoutComponent,
        children: [
            {
                component: UnauthorizedComponent,
                path:"unauthorized"
            },
            {
                component: ForbiddenComponent,
                path: "forbidden"
            },
            {
                component: NotFoundComponent,
                path: "not-found"
            },
            {
                component: InternalServerComponent,
                path: "internal-server"
            },
            {
                component: ErrorComponent,
                path: ":status-code"
            }
        ]
    }
]