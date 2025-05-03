import { Routes } from '@angular/router';
import { ErrorLayoutComponent } from './layouts/error-layout/error-layout.component';
import { PublicLayoutComponent } from './layouts/public-layout/public-layout.component';
import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
import { ReaderLayoutComponent } from './layouts/reader-layout/reader-layout.component';

// These routes would be imported in the main app-routing.module.ts file
export const layoutRoutes: Routes = [
  {
    path: '',
    component: PublicLayoutComponent,
    children: [
      // Main application routes will be nested here
      // { path: '', redirectTo: 'home', pathMatch: 'full' },
      // { path: 'home', component: HomeComponent }, - This would be defined in the app module
      // { path: 'books', component: BooksComponent },
      // { path: 'category/:id', component: CategoryComponent },
      // etc.
    ]
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      // Admin routes will be nested here
      // { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      // { path: 'dashboard', component: AdminDashboardComponent },
      // { path: 'books', component: AdminBooksComponent },
      // etc.
    ]
  },
  {
    path: 'reader',
    component: ReaderLayoutComponent,
    children: [
      // Auth routes will be nested here
      // { path: 'login', component: LoginComponent },
      // { path: 'register', component: RegisterComponent },
      // { path: 'forgot-password', component: ForgotPasswordComponent },
      // etc.
    ]
  },
  {
    path: 'error',
    component: ErrorLayoutComponent,
    children: [
      // Error routes will be nested here
      // { path: '404', component: NotFoundComponent },
      // { path: '500', component: ServerErrorComponent },
      // etc.
    ]
  },
  // Catch-all route for 404
  { path: '**', redirectTo: 'error/404' }
];

