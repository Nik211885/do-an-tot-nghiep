import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

export enum ToastType {
  SUCCESS = 'success',
  ERROR = 'error',
  WARNING = 'warning',
  INFO = 'info'
}

export interface Toast {
  message: string;
  type: ToastType;
  duration?: number;
}

@Injectable({
  providedIn: 'root'
})
export class ToastService {
  private toastSubject = new Subject<Toast>();
  toast$: Observable<Toast> = this.toastSubject.asObservable();

  constructor() { }

  show(message: string, type: ToastType = ToastType.INFO, duration: number = 3000): void {
    this.toastSubject.next({ message, type, duration });
  }

  success(message: string, duration?: number): void {
    this.show(message, ToastType.SUCCESS, duration);
  }

  error(message: string, duration?: number): void {
    this.show(message, ToastType.ERROR, duration);
  }

  warning(message: string, duration?: number): void {
    this.show(message, ToastType.WARNING, duration);
  }

  info(message: string, duration?: number): void {
    this.show(message, ToastType.INFO, duration);
  }
}