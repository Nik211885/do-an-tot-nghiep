import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { SidebarItem } from '../models/sidebar-item.interface';
import {MenuPermission, Role} from '../models/menu-permission.enum';
import { AuthService } from '../../core/auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class SidebarService {
  private sidebarItemsSubject = new BehaviorSubject<SidebarItem[]>(this.getDefaultSidebarItems());
  sidebarItems$ = this.sidebarItemsSubject.asObservable();

  constructor(private authService: AuthService) {}

    getUserSidebarItems(role: string[]): Observable<SidebarItem[]> {
      const allItems = this.getDefaultSidebarItems();

      const filteredItems = this.filterSidebarItemsByRole(allItems, role);

      return of(filteredItems);
    }
    private filterSidebarItemsByRole(items: SidebarItem[], userRoles: string[]): SidebarItem[] {
      return items
        .filter(item => this.hasPermission(item, userRoles))
        .map(item => ({
          ...item,
          children: item.children ? this.filterSidebarItemsByRole(item.children, userRoles) : undefined
        }));
    }
    private hasPermission(item: SidebarItem, userRoles: string[]): boolean {
      if (!item.permissions || item.permissions.length === 0) return true;

      return item.permissions.some(role => userRoles.includes(role));
    }
    private getDefaultSidebarItems(): SidebarItem[] {
      return [
        {
          id: 'dashboard',
          label: 'Trang chủ',
          icon: `https://www.svgrepo.com/show/459022/dashboard.svg`,
          route: '/admin/dashboard',
        },
        {
          id: 'books',
          label: 'Sách',
          icon: 'https://www.svgrepo.com/show/94674/books-stack-of-three.svg',
          permissions: [Role.Author],
          children: [
            {
              id: 'all-books',
              label: 'Sách đang viết',
              route: '/write-book/books',
              permissions: [Role.Author]
            },
            {
              id: 'add-book',
              label: 'Thêm sách',
              route: '/write-book/books/create',
              permissions: [Role.Author]
            },
            {
              id: 'repository-book',
              label: 'Sách đã viết',
              route: '/write-book/repository',
              permissions: [Role.Author]
            },
          ]
        },
      {
        id: 'information',
        label: 'Cá nhân',
        permissions: [Role.Author],
        icon: 'https://www.svgrepo.com/show/487970/user-information.svg',
        children: [
          {
            id:"view-profile",
            label: 'Thông tin cá nhân',
            route: '/user-profile/profile'
          },
          {
            id:"view-search-history",
            label: 'Lịch sử tìm kiếm',
            permissions: [Role.Author],
            route: '/user-profile/search-history',
          },
          {
            id:"view-book-favorite",
            label: 'Sách yêu thích',
            permissions: [Role.Author],
            route: '/user-profile/favorite-book',
          }
        ]
      },
      {
        id: 'order',
        label: 'Sách mua',
        permissions: [Role.Author],
        icon: 'https://www.svgrepo.com/show/493951/order.svg',
        route: '/order/book',
      },
      {
        id: 'moderation',
        label: 'Kiểm duyệt',
        permissions: [Role.Moderation],
        icon: 'https://www.svgrepo.com/show/307792/exam-article-examination-test.svg',
        children:[
          {
            id: 'view-moderation',
            label: 'Cần kiểm duyệt',
            permissions: [Role.Moderation],
            route: '/moderation/view-moderation',
          },
          {
            id: 'view-moderation-pass',
            label: 'Đã kiểm duyệt',
            permissions: [Role.Moderation],
            route: '/moderation/repository',
          }
        ]
      },
      {
        id:'resources_system',
        label: "Hệ thống",
        permissions: [Role.ManagerResources],
        icon: "https://www.svgrepo.com/show/486224/system-settings-backup.svg",
        children:[
          {
            id: 'genres',
            label: "Quản lý thể loại sách",
            permissions: [Role.ManagerResources],
            route: 'resources/genres',
          }
        ]
      }
    ];
  }
}
