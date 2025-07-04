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
      },
      {
          path: 'detail/:id',
        loadComponent:()=>import ("./components/moderation-detail/moderation-detail.component")
          .then(c=>c.ModerationDetailComponent),
      },
      {
        path: 'all-chapter/in-book/:id',
        loadComponent:()=>import ("./components/all-chapter-in-book/all-chapter-in-book.component")
          .then(c=>c.AllChapterInBookComponent),
      }
    ]
  }
]
