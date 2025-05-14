// dialog.service.ts
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, Subject } from 'rxjs';

export type DialogSize = 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl' | '6xl' | '7xl' | 'full'; 

export interface DialogRef {
  isSuccess: boolean;
  data?: any;
}

export interface DialogOptions {
  title?: string;
  message?: string;
  size?: DialogSize; 
  showCancelButton?: boolean;
  confirmButtonText?: string;
  cancelButtonText?: string;
  customContent?: any;
  position?: 'center' | 'top';
  hideDefaultButtons?: boolean;
}

@Injectable({
  providedIn: 'root'
})
export class DialogService {
  private dialogStateSubject = new BehaviorSubject<boolean>(false);
  private dialogTitleSubject = new BehaviorSubject<string>('');
  private dialogMessageSubject = new BehaviorSubject<string>('');
  private dialogSizeSubject = new BehaviorSubject<string>('md');
  private dialogResultSubject = new Subject<DialogRef>();
  private dialogOptionsSubject = new BehaviorSubject<DialogOptions>({});
  private customContentSubject = new BehaviorSubject<any>(null);
  private hideDefaultButtonsSubject = new BehaviorSubject<boolean>(false);
  
  dialogState$: Observable<boolean> = this.dialogStateSubject.asObservable();
  dialogTitle$: Observable<string> = this.dialogTitleSubject.asObservable();
  dialogMessage$: Observable<string> = this.dialogMessageSubject.asObservable();
  dialogSize$: Observable<string> = this.dialogSizeSubject.asObservable();
  dialogOptions$: Observable<DialogOptions> = this.dialogOptionsSubject.asObservable();
  customContent$: Observable<any> = this.customContentSubject.asObservable();
  hideDefaultButtons$: Observable<boolean> = this.hideDefaultButtonsSubject.asObservable();
  
  constructor() {}
  
  open(titleOrOptions: string | DialogOptions, message: string = '', size: 'sm' | 'md' | 'lg' | 'xl' | '2xl' | '3xl' | '4xl' | '5xl' | '6xl' | '7xl' | 'full' = 'md'): Promise<DialogRef> {
    // Add a class to body for additional page styling when dialog is open
    document.body.classList.add('dialog-open');
    
    let options: DialogOptions;
    
    if (typeof titleOrOptions === 'string') {
      options = {
        title: titleOrOptions,
        message: message,
        size: size,
        showCancelButton: true,
        confirmButtonText: 'Xác nhận',
        cancelButtonText: 'Hủy',
        position: 'center',
        hideDefaultButtons: false
      };
    } else {
      options = {
        ...{
          title: '',
          message: '',
          size: 'md',
          showCancelButton: true,
          confirmButtonText: 'Xác nhận',
          cancelButtonText: 'Hủy',
          position: 'center',
          hideDefaultButtons: false
        },
        ...titleOrOptions
      };
    }
    
    this.dialogTitleSubject.next(options.title || '');
    this.dialogMessageSubject.next(options.message || '');
    this.dialogSizeSubject.next(options.size || 'md');
    this.dialogOptionsSubject.next(options);
    this.hideDefaultButtonsSubject.next(options.hideDefaultButtons || false);
    
    if (options.customContent) {
      this.customContentSubject.next(options.customContent);
    } else {
      this.customContentSubject.next(null);
    }
    
    this.dialogStateSubject.next(true);
    
    return new Promise<DialogRef>((resolve) => {
      const subscription = this.dialogResultSubject.subscribe(result => {
        resolve(result);
        subscription.unsubscribe();
      });
    });
  }
  
  confirm(result: boolean = true, data?: any): void {
    this.close();
    this.dialogResultSubject.next({ isSuccess: result, data });
  }
  
  close(): void {
    document.body.classList.remove('dialog-open');
    this.dialogStateSubject.next(false);
  }
  
  cancel(): void {
    this.close();
    this.dialogResultSubject.next({ isSuccess: false });
  }
  
  get isOpen(): boolean {
    return this.dialogStateSubject.value;
  }
}