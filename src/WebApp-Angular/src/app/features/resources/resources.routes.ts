import {Routes} from '@angular/router';
import {AdminLayoutComponent} from '../../layout/layouts/admin-layout/admin-layout.component';

export const resourcesRoutes: Routes = [
  {
    path: "resources",
    component:  AdminLayoutComponent,
    children: [
      {
        path: "genres",
        loadComponent: ()=> import("./components/list-resources/list-resources.component")
          .then(c=>c.ListResourcesComponent)
      },
      {
        path:"genres/create",
        loadComponent:()=> import("./components/create-genre/create-genre.component")
          .then(c=>c.CreateGenreComponent)
      },
      {
        path: "genres/detail/:id",
        loadComponent: ()=> import("./components/detail-resources/detail-resources.component")
          .then(c=>c.DetailResourcesComponent)
      }
    ]
  }
]
