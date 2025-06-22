import { Routes } from '@angular/router';
import { AdminLayoutComponent } from '../../../layout/layouts/admin-layout/admin-layout.component';

export const writeBook: Routes = [
  {
    path:'write-book',
    component: AdminLayoutComponent,
    children: [
      {
        path: 'books',
        loadComponent: () => import("../write-book/admin-book/book-list/book-list.component").then(m => m.BookListComponent)
      },
      {
        path: 'books/create',
        loadComponent: ()=> import("../write-book/admin-book/book-form/book-create/book-create.component").then(m=>m.BookCreateComponent)
      },
      {
        path: 'books/:slug',
        loadComponent: ()=> import("../write-book/chapter/chapter-list/chapter-list.component").then(m=>m.ChapterListComponent)
      },
      {
        path: 'books/:bookSlug/chapters/create',
        loadComponent: ()=> import("../write-book/chapter/chapter-editor/chapter-editor.component").then(m=>m.ChapterEditorComponent)
      },
      {
        path: 'books/:bookSlug/chapters/:chapterSlug/edit',
        loadComponent: ()=> import("../write-book/chapter/chapter-editor/chapter-editor.component").then(m=>m.ChapterEditorComponent),
      },
      {
        path: 'books-information/:id',
        loadComponent:()=> import("../write-book/admin-book/book-detail/book-detail.component").then(m=>m.BookDetailComponent),
      } ,
      {
        path:'repository',
        loadComponent:()=> import("../write-book/admin-book/book-repository/book-repository.component").then(m=>m.BookRepositoryComponent)
      }
    ]
  }
];
