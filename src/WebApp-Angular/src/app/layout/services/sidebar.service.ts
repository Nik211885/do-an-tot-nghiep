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
    MenuPermission.VIEW_STATISTICS,
    MenuPermission.MANAGE_INFO,
    MenuPermission.VIEW_ORDER,
    MenuPermission.VIEW_MODERATION,
    MenuPermission.VIEW_GENRES
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
      {
        id: 'information',
        label: 'Cá nhân',
        permissions: [MenuPermission.MANAGE_INFO],
        icon: 'https://www.svgrepo.com/show/487970/user-information.svg',
        children: [
          {
            id:"view-profile",
            label: 'Thông tin cá nhân',
            permissions: [MenuPermission.MANAGE_INFO],
            route: '/user-profile/profile'
          },
          {
            id:"view-search-history",
            label: 'Lịch sử tìm kiếm',
            permissions: [MenuPermission.MANAGE_INFO],
            route: '/user-profile/search-history',
          },
          {
            id:"view-book-favorite",
            label: 'Sách yêu thích',
            permissions: [MenuPermission.MANAGE_INFO],
            route: '/user-profile/favorite-book',
          }
        ]
      },
      {
        id: 'order',
        label: 'Sách mua',
        permissions: [MenuPermission.VIEW_ORDER],
        icon: 'https://www.svgrepo.com/show/493951/order.svg',
        route: '/order/book',
      },
      {
        id: 'moderation',
        label: 'Kiểm duyệt',
        permissions: [MenuPermission.VIEW_MODERATION],
        icon: 'https://www.svgrepo.com/show/307792/exam-article-examination-test.svg',
        children:[
          {
            id: 'view-moderation',
            label: 'Cần kiểm duyệt',
            permissions: [MenuPermission.VIEW_MODERATION],
            route: '/moderation/view-moderation',
          },
          {
            id: 'view-moderation-pass',
            label: 'Đã kiểm duyệt',
            permissions: [MenuPermission.VIEW_MODERATION],
            route: '/moderation/repository',
          }
        ]
      },
      {
        id:'resources_system',
        label: "Hệ thống",
        permissions: [MenuPermission.ADMIN],
        icon: "https://www.svgrepo.com/show/486224/system-settings-backup.svg",
        children:[
          {
            id: 'genres',
            label: "Quản lý thể loại sách",
            permissions: [MenuPermission.ADMIN],
            route: 'resources/genres',
          }
        ]
      }
    ];
  }
}
