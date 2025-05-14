import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { Subscription } from 'rxjs';
import { Toast, ToastService, ToastType } from './toast.service';

@Component({
  standalone: true,
  selector: 'app-toast',
  imports: [CommonModule],
  templateUrl: './toast.component.html',
  styleUrl: './toast.component.css'
})
export class ToastComponent {
  toasts: (Toast & { id: number })[] = [];
  private subscription: Subscription | null = null;
  private counter = 0;
  toastType = ToastType
  constructor(private toastService: ToastService) {}

  ngOnInit(): void {
    this.subscription = this.toastService.toast$.subscribe(toast => {
      this.addToast(toast);
    });
  }

  ngOnDestroy(): void {
    if (this.subscription) {
      this.subscription.unsubscribe();
    }
  }

  private addToast(toast: Toast): void {
    const id = this.counter++;
    
    this.toasts.push({
      ...toast,
      id
    });

    // Auto remove toast after duration
    if (toast.duration !== 0) {
      setTimeout(() => {
        this.removeToastById(id);
      }, toast.duration || 3000);
    }
  }
  removeToast(toast: Toast & { id: number }): void {
    this.removeToastById(toast.id);
  }

  private removeToastById(id: number): void {
    this.toasts = this.toasts.filter(t => t.id !== id);
  }
}
