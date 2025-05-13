import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { DEFAULT_ADMIN_LAYOUT } from '../../models/layout-config.interface';
import { LayoutService } from '../../services/layout.service';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { AdminHeaderComponent } from '../../components/header/variants/admin-header/admin-header.component';
import { AdminFooterComponent } from '../../components/footer/variants/admin-footer/admin-footer.component';
import { BreadcrumbComponent } from '../../components/breadcrumb/breadcrumb.component';
import { SidebarComponent } from '../../components/sidebar/sidebar/sidebar.component';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-admin-layout',
  imports: [FormsModule,
     CommonModule,
     AdminHeaderComponent,
     AdminFooterComponent,
     SidebarComponent,
     BreadcrumbComponent,
     RouterOutlet
    ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css'
})
export class AdminLayoutComponent implements OnInit, OnDestroy {
  sidebarExpanded = true;
  pageHeader: {title: string, subtitle?: string} = {title: ''};
  private subscriptions = new Subscription();

  constructor(private layoutService: LayoutService) {}

  ngOnInit(): void {
    this.layoutService.setLayoutConfig(DEFAULT_ADMIN_LAYOUT);

    // Example breadcrumbs - would be set by router in a real app
    this.layoutService.setBreadcrumbs([
      { label: 'Admin', url: '/admin' },
      { label: 'Books', url: '/admin/books' },
      { label: 'Edit Book' }
    ]);

    // Example page header

    // Subscribe to sidebar state
    this.subscriptions.add(
      this.layoutService.sidebarExpanded$.subscribe(expanded => {
        this.sidebarExpanded = expanded;
      })
    );

    // Subscribe to page header
    this.subscriptions.add(
      this.layoutService.pageHeader$.subscribe(header => {
        this.pageHeader = header;
      })
    );
  }

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
