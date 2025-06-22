import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { SidebarItem } from '../models/sidebar-item.interface';
import { MenuPermission } from '../models/menu-permission.enum';
import { AuthService } from '../../core/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private sidebarItemsSubject = new BehaviorSubject<SidebarItem[]>(this.getDefaultSidebarItems());
  sidebarItems$ = this.sidebarItemsSubject.asObservable();

  // Mock user permissions - in a real app, would come from auth service
  private userPermissions: string[] = [
    MenuPermission.ADMIN,
    MenuPermission.MANAGE_BOOKS,
    MenuPermission.MANAGE_USERS,
    MenuPermission.VIEW_DASHBOARD,
    MenuPermission.VIEW_STATISTICS
  ];

  constructor(private authService: AuthService) {}

  getUserSidebarItems(): Observable<SidebarItem[]> {
    // Filter items based on user permissions
    const filteredItems = this.getDefaultSidebarItems().filter(item =>
      this.hasPermission(item.permissions)
    ).map(item => {
      if (item.children) {
        return {
          ...item,
          children: item.children.filter(child => this.hasPermission(child.permissions))
        };
      }
      return item;
    });

    return of(filteredItems);
  }

  private hasPermission(requiredPermissions?: string[]): boolean {
    if (!requiredPermissions || requiredPermissions.length === 0) {
      return true;
    }

    return requiredPermissions.some(permission =>
      this.userPermissions.includes(permission) || this.userPermissions.includes(MenuPermission.ADMIN)
    );
  }

  private getDefaultSidebarItems(): SidebarItem[] {
    return [
      {
        id: 'dashboard',
        label: 'Trang chủ',
        icon: `https://www.svgrepo.com/show/459022/dashboard.svg`,
        route: '/admin/dashboard',
        permissions: [MenuPermission.VIEW_DASHBOARD]
      },
      {
        id: 'books',
        label: 'Sách',
        icon: 'https://www.svgrepo.com/show/94674/books-stack-of-three.svg',
        permissions: [MenuPermission.MANAGE_BOOKS],
        children: [
          {
            id: 'all-books',
            label: 'Sách đang viết',
            route: '/write-book/books',
            permissions: [MenuPermission.MANAGE_BOOKS]
          },
          {
            id: 'add-book',
            label: 'Thêm sách',
            route: '/write-book/books/create',
            permissions: [MenuPermission.MANAGE_BOOKS]
          },
          {
            id: 'repository-book',
            label: 'Sách đã viết',
            route: '/write-book/repository',
            permissions: [MenuPermission.MANAGE_BOOKS]
          },
        ]
      },
    ];
  }
}
