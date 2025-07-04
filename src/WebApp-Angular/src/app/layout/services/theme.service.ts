import { Injectable, Renderer2, RendererFactory2 } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

export type ThemeMode = 'light' | 'dark' | 'system';
export type FontSize = 'small' | 'medium' | 'large' | 'x-large';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  private renderer: Renderer2;
  private themeKey = 'book-platform-theme';
  private fontSizeKey = 'book-platform-font-size';

  private themeModeSubject = new BehaviorSubject<ThemeMode>(this.getInitialTheme());
  themeMode$ = this.themeModeSubject.asObservable();

  private fontSizeSubject = new BehaviorSubject<FontSize>(this.getInitialFontSize());
  fontSize$ = this.fontSizeSubject.asObservable();

  private fontSizeMap = {
    'small': '0.875rem',
    'medium': '1rem',
    'large': '1.125rem',
    'x-large': '1.25rem'
  };

  private lineHeightMap = {
    'small': '1.6',
    'medium': '1.8',
    'large': '1.9',
    'x-large': '2'
  };

  constructor(rendererFactory: RendererFactory2) {
    this.renderer = rendererFactory.createRenderer(null, null);
    this.applyTheme(this.themeModeSubject.value);
    this.applyFontSize(this.fontSizeSubject.value);
  }

  setTheme(theme: ThemeMode): void {
    this.themeModeSubject.next(theme);
    localStorage.setItem(this.themeKey, theme);
    this.applyTheme(theme);
  }

  setFontSize(size: FontSize): void {
    this.fontSizeSubject.next(size);
    localStorage.setItem(this.fontSizeKey, size);
    this.applyFontSize(size);
  }

  toggleTheme(): void {
    const currentTheme = this.themeModeSubject.value;
    const newTheme = currentTheme === 'light' ? 'dark' : 'light';
    this.setTheme(newTheme);
  }

  private getInitialTheme(): ThemeMode {
    const savedTheme = localStorage.getItem(this.themeKey) as ThemeMode;

    if (savedTheme) {
      return savedTheme;
    }

    if (savedTheme === 'system' || !savedTheme) {
      return window.matchMedia('(prefers-color-scheme: dark)').matches ? 'dark' : 'light';
    }

    return 'light';
  }

  private getInitialFontSize(): FontSize {
    return (localStorage.getItem(this.fontSizeKey) as FontSize) || 'medium';
  }

  private applyTheme(theme: ThemeMode): void {
    const isDark = theme === 'dark' ||
      (theme === 'system' && window.matchMedia('(prefers-color-scheme: dark)').matches);

    if (isDark) {
      document.documentElement.classList.add('dark');
    } else {
      document.documentElement.classList.remove('dark');
    }
  }

  private applyFontSize(size: FontSize): void {
    document.documentElement.style.setProperty('--reader-font-size', this.fontSizeMap[size]);
    document.documentElement.style.setProperty('--reader-line-height', this.lineHeightMap[size]);
  }
}
