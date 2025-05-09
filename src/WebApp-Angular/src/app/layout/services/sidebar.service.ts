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
        label: 'Dashboard',
        icon: `https://www.svgrepo.com/show/459022/dashboard.svg`,
        route: '/admin/dashboard',
        permissions: [MenuPermission.VIEW_DASHBOARD]
      },
      {
        id: 'books',
        label: 'Books',
        icon: 'https://www.svgrepo.com/show/94674/books-stack-of-three.svg',
        permissions: [MenuPermission.MANAGE_BOOKS],
        children: [
          {
            id: 'all-books',
            label: 'All Books',
            route: '/admin/books',
            permissions: [MenuPermission.MANAGE_BOOKS]
          },
          {
            id: 'add-book',
            label: 'Add New Book',
            route: '/admin/books/new',
            permissions: [MenuPermission.MANAGE_BOOKS]
          },
          {
            id: 'categories',
            label: 'Categories',
            route: '/admin/categories',
            permissions: [MenuPermission.MANAGE_CATEGORIES]
          }
        ]
      },
      {
        id: 'users',
        label: 'Users',
        icon: 'https://www.svgrepo.com/show/532387/user-search.svg',
        permissions: [MenuPermission.MANAGE_USERS],
        children: [
          {
            id: 'all-users',
            label: 'All Users',
            route: '/admin/users',
            permissions: [MenuPermission.MANAGE_USERS]
          },
          {
            id: 'authors',
            label: 'Authors',
            route: '/admin/authors',
            permissions: [MenuPermission.MANAGE_USERS]
          }
        ]
      },
      {
        id: 'divider-1',
        label: '',
        divider: true
      },
      {
        id: 'statistics',
        label: 'Statistics',
        icon: 'https://www.svgrepo.com/show/408619/statistics-chart-graph-stats-analytics-business-2.svg',
        route: '/admin/statistics',
        permissions: [MenuPermission.VIEW_STATISTICS],
        badge: {
          text: 'New',
          color: 'bg-green-500'
        }
      },
      {
        id: 'payments',
        label: 'Payments',
        icon: 'https://www.svgrepo.com/show/502788/payment-card.svg',
        route: '/admin/payments',
        permissions: [MenuPermission.MANAGE_PAYMENTS]
      },
      {
        id: 'comments',
        label: 'Comments',
        icon: 'https://www.svgrepo.com/show/522070/comment-3.svg',
        route: '/admin/comments',
        permissions: [MenuPermission.MANAGE_COMMENTS]
      }
    ];
  }
}
