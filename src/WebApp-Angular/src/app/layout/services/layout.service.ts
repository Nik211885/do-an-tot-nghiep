import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { LayoutConfig, DEFAULT_PUBLIC_LAYOUT } from '../models/layout-config.interface';
import { Breadcrumb } from '../models/breadcrumb.interface';

@Injectable({
  providedIn: 'root'
})
export class LayoutService {
  private layoutConfigSubject = new BehaviorSubject<LayoutConfig>(DEFAULT_PUBLIC_LAYOUT);
  layoutConfig$ = this.layoutConfigSubject.asObservable();

  private breadcrumbsSubject = new BehaviorSubject<Breadcrumb[]>([]);
  breadcrumbs$ = this.breadcrumbsSubject.asObservable();

  private sidebarExpandedSubject = new BehaviorSubject<boolean>(true);
  sidebarExpanded$ = this.sidebarExpandedSubject.asObservable();

  private sidebarVisibleSubject = new BehaviorSubject<boolean>(true);
  sidebarVisible$ = this.sidebarVisibleSubject.asObservable();

  private pageHeaderSubject = new BehaviorSubject<{title: string, subtitle?: string}>({title: ''});
  pageHeader$ = this.pageHeaderSubject.asObservable();

  constructor() {
    const savedExpanded = localStorage.getItem('sidebarExpanded');
    const savedVisible = localStorage.getItem('sidebarVisible');
    
    if (savedExpanded !== null) {
      this.sidebarExpandedSubject.next(savedExpanded === 'true');
    }
    
    if (savedVisible !== null) {
      this.sidebarVisibleSubject.next(savedVisible === 'true');
    }
  }

  setLayoutConfig(config: Partial<LayoutConfig>): void {
    this.layoutConfigSubject.next({
      ...this.layoutConfigSubject.value,
      ...config
    });
  }

  setBreadcrumbs(breadcrumbs: Breadcrumb[]): void {
    this.breadcrumbsSubject.next(breadcrumbs);
  }

  toggleSidebar(): void {
    this.sidebarExpandedSubject.next(!this.sidebarExpandedSubject.value);
    this.setLayoutConfig({ sidebarExpanded: this.sidebarExpandedSubject.value });
  }


  getLayoutConfig(): Observable<LayoutConfig> {
    return this.layoutConfig$;
  }

  getCurrentLayoutConfig(): LayoutConfig {
    return this.layoutConfigSubject.value;
  }

  toggleSidebarExpanded(): void {
    const newState = !this.sidebarExpandedSubject.value;
    this.sidebarExpandedSubject.next(newState);
    localStorage.setItem('sidebarExpanded', String(newState));
  }
  
  setSidebarExpanded(expanded: boolean): void {
    this.sidebarExpandedSubject.next(expanded);
    localStorage.setItem('sidebarExpanded', String(expanded));
  }
  
  setSidebarVisible(visible: boolean): void {
    this.sidebarVisibleSubject.next(visible);
    localStorage.setItem('sidebarVisible', String(visible));
  }
}
