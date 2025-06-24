import {Routes} from '@angular/router';
import {AdminLayoutComponent} from '../../../layout/layouts/admin-layout/admin-layout.component';

export const moderationRoutes: Routes = [
  {
    path: 'moderation',
    component: AdminLayoutComponent,
    children: [
      {
        path:'view-moderation',
        loadComponent: ()=> import("./components/list-moderation/list-moderation.component")
          .then(c=>c.ListModerationComponent),
      },
      {
        path:'repository',
        loadComponent: ()=> import("./components/moderation-repository/moderation-repository.component")
          .then(c=>c.ModerationRepositoryComponent),
      }
    ]
  }
]
