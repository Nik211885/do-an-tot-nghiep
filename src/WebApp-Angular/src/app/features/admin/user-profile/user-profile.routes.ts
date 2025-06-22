import {Routes} from '@angular/router';
import {AdminLayoutComponent} from '../../../layout/layouts/admin-layout/admin-layout.component';
import {ProfileComponent} from './profile/profile.component';

export const userProfileRoutes: Routes = [
  {
    path: "user-profile",
    component: AdminLayoutComponent,
    children: [
      {
        path: "profile",
        loadComponent:()=> import ("../user-profile/profile/profile.component").then(m=>m.ProfileComponent),
      }
    ]
  }
]
