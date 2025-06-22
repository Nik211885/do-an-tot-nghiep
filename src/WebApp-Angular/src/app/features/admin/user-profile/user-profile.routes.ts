import {Routes} from '@angular/router';
import {AdminLayoutComponent} from '../../../layout/layouts/admin-layout/admin-layout.component';
import {ProfileComponent} from './components/profile/profile.component';

export const userProfileRoutes: Routes = [
  {
    path: "user-profile",
    component: AdminLayoutComponent,
    children: [
      {
        path: "profile",
        loadComponent:()=> import ("./components/profile/profile.component").then(m=>m.ProfileComponent),
      },
      {
        path: 'favorite-book',
        loadComponent:()=> import ("./components/book-favorite/book-favorite.component").then(m=>m.BookFavoriteComponent),
      },
      {
        path: 'search-history',
        loadComponent:()=> import ("./components/search-history/search-history.component").then(m=>m.SearchHistoryComponent),
      }
    ]
  }
]
