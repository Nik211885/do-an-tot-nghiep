import { Routes } from '@angular/router';
import { HeaderComponent } from './shared/components/header/header.component';
import { LoginComponent } from './shared/components/login/login.component';

export const routes: Routes = [
    {
        path:'',
        component: HeaderComponent
    },
    {
      path:'login',
      component: LoginComponent  
    }
];
