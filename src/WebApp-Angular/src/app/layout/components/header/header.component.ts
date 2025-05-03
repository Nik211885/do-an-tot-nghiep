import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { LayoutService } from '../../services/layout.service';
import { ThemeService } from '../../services/theme.service';
import { CommonModule } from '@angular/common';
import { ThemeToggleDirective } from '../../directives/theme-toggle.directive';

@Component({
  standalone: true,
  selector: 'app-header',
  imports: [CommonModule, ThemeToggleDirective],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent implements OnInit, OnDestroy {
  @Input() variant: 'public' | 'admin' | 'reader' | 'error' = 'public';

  private subscriptions = new Subscription();

  constructor(
    private themeService: ThemeService,
    private layoutService: LayoutService
  ) {}

  ngOnInit(): void {}

  ngOnDestroy(): void {
    this.subscriptions.unsubscribe();
  }
}
