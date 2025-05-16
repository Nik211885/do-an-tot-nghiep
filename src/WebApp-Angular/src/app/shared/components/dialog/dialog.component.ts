import { Component, OnInit, OnDestroy } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { trigger, transition, style, animate } from '@angular/animations';
import { Subscription } from 'rxjs';
import { DialogService, DialogOptions, DialogSize, InputField } from './dialog.component.service';

@Component({
  selector: 'app-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.css'],
  animations: [
    trigger('dialogAnimation', [
      transition(':enter', [
        style({ opacity: 0, transform: 'scale(0.98) translateY(-10px)' }),
        animate('200ms cubic-bezier(0.4, 0, 0.2, 1)', 
          style({ opacity: 1, transform: 'scale(1) translateY(0)' }))
      ]),
      transition(':leave', [
        animate('150ms cubic-bezier(0.4, 0, 0.2, 1)', 
          style({ opacity: 0, transform: 'scale(0.98) translateY(-10px)' }))
      ])
    ]),
    trigger('backdropAnimation', [
      transition(':enter', [
        style({ opacity: 0 }),
        animate('200ms cubic-bezier(0.4, 0, 0.2, 1)', style({ opacity: 1 }))
      ]),
      transition(':leave', [
        animate('150ms cubic-bezier(0.4, 0, 0.2, 1)', style({ opacity: 0 }))
      ])
    ])
  ]
})
export class DialogComponent implements OnInit, OnDestroy {
  isOpen = false;
  title = '';
  message = '';
  size: DialogSize = 'md';
  options: DialogOptions = {};
  customContent: any = null;
  hideDefaultButtons = false;
  
  // Input dialog properties
  isInputDialog = false;
  inputFields: InputField[] = [];
  inputValues: { [key: string]: any } = {};
  inputErrors: { [key: string]: string } = {};
  
  private subscriptions: Subscription[] = [];
  
  constructor(private dialogService: DialogService) {}
  
  ngOnInit(): void {
    this.subscriptions.push(
      this.dialogService.dialogState$.subscribe(state => {
        this.isOpen = state;
        if (state) {
          // Handle accessibility focus trap and keyboard navigation
          document.addEventListener('keydown', this.handleKeyboardEvent);
        } else {
          document.removeEventListener('keydown', this.handleKeyboardEvent);
        }
      }),
      this.dialogService.dialogTitle$.subscribe(title => {
        this.title = title;
      }),
      this.dialogService.dialogMessage$.subscribe(message => {
        this.message = message;
      }),
      this.dialogService.dialogSize$.subscribe(size => {
        this.size = size as DialogSize;
      }),
      this.dialogService.dialogOptions$.subscribe(options => {
        this.options = options;
      }),
      this.dialogService.customContent$.subscribe(content => {
        this.customContent = content;
      }),
      this.dialogService.hideDefaultButtons$.subscribe(hide => {
        this.hideDefaultButtons = hide;
      }),
      
      // Input dialog subscriptions
      this.dialogService.isInputDialog$.subscribe(isInput => {
        this.isInputDialog = isInput;
      }),
      this.dialogService.inputFields$.subscribe(fields => {
        this.inputFields = fields;
      }),
      this.dialogService.inputValues$.subscribe(values => {
        this.inputValues = values;
      }),
      this.dialogService.inputErrors$.subscribe(errors => {
        this.inputErrors = errors;
      })
    );
  }
  
  ngOnDestroy(): void {
    this.subscriptions.forEach(sub => sub.unsubscribe());
    document.removeEventListener('keydown', this.handleKeyboardEvent);
    
    // Make sure to clean up any body classes if component is destroyed while dialog is open
    document.body.classList.remove('dialog-open');
  }
  
  get dialogSizeClass(): string {
    const sizes = {
      'sm': 'max-w-sm',
      'md': 'max-w-md',
      'lg': 'max-w-lg',
      'xl': 'max-w-xl',
      '2xl': 'max-w-2xl',
      '3xl': 'max-w-3xl',
      '4xl': 'max-w-4xl',
      '5xl': 'max-w-5xl',
      '6xl': 'max-w-6xl',
      '7xl': 'max-w-7xl',
      'full': 'max-w-full sm:mx-6 md:mx-12'
    };
    return sizes[this.size] || sizes['md'];
  }
  
  handleKeyboardEvent = (event: KeyboardEvent) => {
    // Close on Escape key
    if (event.key === 'Escape') {
      this.cancel();
      event.preventDefault();
    }
    
    // Trap focus within dialog for better accessibility
    if (event.key === 'Tab') {
      // Add focus trap logic here if needed
    }
  };
  
  confirm(): void {
    this.dialogService.confirm(true);
  }
  
  cancel(): void {
    this.dialogService.cancel();
  }
  
  onBackdropClick(event: MouseEvent): void {
    // Close only if backdrop was clicked directly
    if ((event.target as HTMLElement).classList.contains('dialog-backdrop')) {
      this.cancel();
    }
  }
  
  // Prevent closing when clicking inside the dialog
  onDialogClick(event: Event): void {
    event.stopPropagation();
  }
  
  // New method for updating input values
  updateInputValue(name: string, value: any): void {
    this.dialogService.updateInputValue(name, value);
  }
}