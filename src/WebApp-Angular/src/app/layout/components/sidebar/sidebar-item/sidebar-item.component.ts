import { Component, Input, OnInit } from '@angular/core';
import { SidebarItem } from '../../../models/sidebar-item.interface';
import { CommonModule } from '@angular/common';
import { SidebarService } from '../../../services/sidebar.service';
import { LayoutService } from '../../../services/layout.service';
import { Subscription } from 'rxjs';
import {RouterLink} from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-sidebar-item',
  imports: [CommonModule, RouterLink],
  templateUrl: './sidebar-item.component.html',
  styleUrl: './sidebar-item.component.css'
})
export class SidebarItemComponent implements OnInit {
  @Input() item!: SidebarItem;
  @Input() collapsed = false;

  isActive = false;
  isExpanded = false;
  isHovered = false;

  ngOnInit(): void {
    // In a real app, would check if route is active
    // For now, just randomly set some items as active
    this.isActive = Math.random() > 0.8;

    // Expand any item with children that has an active child
    if (this.item.children && this.item.children.some(child => Math.random() > 0.8)) {
      this.isExpanded = true;
    }
  }

  toggleExpand(event: Event): void {
    // Only toggle expand state if item has children
    if (this.item.children && this.item.children.length > 0) {
      event.preventDefault(); // Prevent navigation
      this.isExpanded = !this.isExpanded;
    }
  }

  onMouseEnter(): void {
    this.isHovered = true;
  }

  onMouseLeave(): void {
    this.isHovered = false;
  }
  trackById(index: number, item: any): number {
    return item.id;
  }

}
