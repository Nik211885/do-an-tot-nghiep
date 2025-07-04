import { CommonModule } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { Breadcrumb } from '../../models/breadcrumb.interface';
import { LayoutService } from '../../services/layout.service';

@Component({
  standalone: true,
  selector: 'app-breadcrumb',
  imports: [CommonModule],
  templateUrl: './breadcrumb.component.html',
  styleUrl: './breadcrumb.component.css'
})
export class BreadcrumbComponent implements OnInit, OnDestroy {
  breadcrumbs: Breadcrumb[] = [];
  private subscription: Subscription = new Subscription();

  constructor(private layoutService: LayoutService) {}

  ngOnInit(): void {
    this.subscription = this.layoutService.breadcrumbs$.subscribe(
      breadcrumbs => {
        this.breadcrumbs = breadcrumbs;
      }
    );
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }
}
