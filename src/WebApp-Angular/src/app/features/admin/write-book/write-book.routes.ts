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
        path: 'books/:bookId', 
        loadComponent: ()=> import("../write-book/chapter/chapter-list/chapter-list.component").then(m=>m.ChapterListComponent)
      },
      { 
        path: 'books/:bookId/chapters/create', 
        loadComponent: ()=> import("../write-book/chapter/chapter-editor/chapter-editor.component").then(m=>m.ChapterEditorComponent)
      },
      { 
        path: 'books/:bookId/chapters/:chapterId/edit',
        loadComponent: ()=> import("../write-book/chapter/chapter-editor/chapter-editor.component").then(m=>m.ChapterEditorComponent)
      }
    ]
  }
];