import { CommonModule } from '@angular/common';
import { Component, inject, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { SidebarItem } from '../../../models/sidebar-item.interface';
import { LayoutService } from '../../../services/layout.service';
import { SidebarService } from '../../../services/sidebar.service';
import { SidebarItemComponent } from '../sidebar-item/sidebar-item.component';
import {AuthService} from '../../../../core/auth/auth.service';
import {Role} from '../../../models/menu-permission.enum';

@Component({
  standalone: true,
  selector: 'app-sidebar',
  imports: [CommonModule, SidebarItemComponent],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css'
})
export class SidebarComponent implements OnInit, OnDestroy {
  sidebarItems: SidebarItem[] = [];
  expanded = true;
  visible = true;
  private subscriptions = new Subscription();
  layoutService = inject(LayoutService)

  constructor(
    private sidebarService: SidebarService,
    private authService: AuthService,
  ) {}

  ngOnInit(): void {
    this.authService.initialize().subscribe({
      next: data => {
        if(data){
          const role = this.authService.getRole();
          this.subscriptions.add(
            this.sidebarService.getUserSidebarItems(role).subscribe(items => {
              this.sidebarItems = items;
            })
          );
        }
      }
    })
    // Get sidebar items with permissions

    // Watch for sidebar expanded state changes
    this.subscriptions.add(
      this.layoutService.sidebarExpanded$.subscribe(expanded => {
        this.expanded = expanded;
      })
    );

    // Watch for sidebar visibility state changes
    this.subscriptions.add(
      this.layoutService.sidebarVisible$.subscribe(visible => {
        this.visible = visible;
      })
    );
  }

  toggleSidebar(): void {
    this.layoutService.toggleSidebarExpanded();
  }

  hideSidebar(): void {
    this.layoutService.setSidebarVisible(false);
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
  trackById(index: number, item: any): number {
    return item.id;
  }

}
