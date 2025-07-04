import { Directive, HostListener, ElementRef, Renderer2, OnInit, OnDestroy } from '@angular/core';
import { ThemeService } from '../services/theme.service';
import { Subscription } from 'rxjs';

@Directive({
  selector: '[appThemeToggle]',
  standalone: true
})
export class ThemeToggleDirective implements OnInit, OnDestroy {
  private themeSubscription!: Subscription;

  constructor(
    private el: ElementRef,
    private renderer: Renderer2,
    private themeService: ThemeService
  ) {}

  ngOnInit(): void {
    this.themeSubscription = this.themeService.themeMode$.subscribe(theme => {
      this.updateButtonState(theme === 'dark');
    });
  }

  @HostListener('click')
  onClick(): void {
    this.themeService.toggleTheme();
  }

  private updateButtonState(isDark: boolean): void {
    // Update button appearance based on theme
    if (isDark) {
      this.renderer.addClass(this.el.nativeElement, 'theme-dark');
      this.renderer.removeClass(this.el.nativeElement, 'theme-light');
    } else {
      this.renderer.addClass(this.el.nativeElement, 'theme-light');
      this.renderer.removeClass(this.el.nativeElement, 'theme-dark');
    }
  }

  ngOnDestroy(): void {
    if (this.themeSubscription) {
      this.themeSubscription.unsubscribe();
    }
  }
}
