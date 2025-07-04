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

// New interfaces for input dialog
export interface InputDialogOptions extends DialogOptions {
  inputs: InputField[];
  initialData?: any;
}

export interface InputField {
  name: string;
  label: string;
  type: 'text' | 'number' | 'email' | 'password' | 'textarea' | 'select' | 'checkbox' | 'radio' | 'date';
  placeholder?: string;
  required?: boolean;
  value?: any;
  options?: Array<{value: any, label: string}>; // For select, radio, etc.
  validators?: InputValidator[];
  errorMessage?: string;
}

export interface InputValidator {
  type: 'required' | 'minLength' | 'maxLength' | 'pattern' | 'email' | 'min' | 'max' | 'custom';
  value?: any; // Value for validators that need it (minLength, pattern, etc.)
  message: string; // Error message to display
  validatorFn?: (value: any) => boolean; // For custom validators
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
  
  // New subjects for input dialog
  private isInputDialogSubject = new BehaviorSubject<boolean>(false);
  private inputFieldsSubject = new BehaviorSubject<InputField[]>([]);
  private inputValuesSubject = new BehaviorSubject<any>({});
  private inputErrorsSubject = new BehaviorSubject<{[key: string]: string}>({});
  
  dialogState$: Observable<boolean> = this.dialogStateSubject.asObservable();
  dialogTitle$: Observable<string> = this.dialogTitleSubject.asObservable();
  dialogMessage$: Observable<string> = this.dialogMessageSubject.asObservable();
  dialogSize$: Observable<string> = this.dialogSizeSubject.asObservable();
  dialogOptions$: Observable<DialogOptions> = this.dialogOptionsSubject.asObservable();
  customContent$: Observable<any> = this.customContentSubject.asObservable();
  hideDefaultButtons$: Observable<boolean> = this.hideDefaultButtonsSubject.asObservable();
  
  // New observables for input dialog
  isInputDialog$: Observable<boolean> = this.isInputDialogSubject.asObservable();
  inputFields$: Observable<InputField[]> = this.inputFieldsSubject.asObservable();
  inputValues$: Observable<any> = this.inputValuesSubject.asObservable();
  inputErrors$: Observable<{[key: string]: string}> = this.inputErrorsSubject.asObservable();
  
  constructor() {}
  
  open(titleOrOptions: string | DialogOptions, message: string = '', size: DialogSize = 'md'): Promise<DialogRef> {
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
        ...(titleOrOptions ?? {})
      };
    }
    
    // Reset input dialog state
    this.isInputDialogSubject.next(false);
    this.inputFieldsSubject.next([]);
    this.inputValuesSubject.next({});
    this.inputErrorsSubject.next({});
    
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
  
  // New method for input dialog
  openInputDialog(options: InputDialogOptions): Promise<DialogRef> {
    document.body.classList.add('dialog-open');
    
    const dialogOptions: DialogOptions = {
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
      ...options
    };
    
    // Set input fields
    this.inputFieldsSubject.next(options.inputs || []);
    
    // Initialize input values from initial data or default values
    const initialValues: {[key: string]: any} = {};
    if (options.initialData) {
      Object.assign(initialValues, options.initialData);
    } else {
      // Initialize with default values from fields
      (options.inputs || []).forEach(field => {
        if (field.value !== undefined) {
          initialValues[field.name] = field.value;
        } else {
          // Set empty defaults based on type
          switch (field.type) {
            case 'checkbox':
              initialValues[field.name] = false;
              break;
            case 'number':
              initialValues[field.name] = 0;
              break;
            default:
              initialValues[field.name] = '';
          }
        }
      });
    }
    
    this.inputValuesSubject.next(initialValues);
    this.isInputDialogSubject.next(true);
    
    this.dialogTitleSubject.next(dialogOptions.title || '');
    this.dialogMessageSubject.next(dialogOptions.message || '');
    this.dialogSizeSubject.next(dialogOptions.size || 'md');
    this.dialogOptionsSubject.next(dialogOptions);
    this.hideDefaultButtonsSubject.next(dialogOptions.hideDefaultButtons || false);
    
    if (dialogOptions.customContent) {
      this.customContentSubject.next(dialogOptions.customContent);
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
  
  // Method to update input values
  updateInputValue(name: string, value: any): void {
    const currentValues = this.inputValuesSubject.value;
    this.inputValuesSubject.next({
      ...currentValues,
      [name]: value
    });
    
    // Validate the field
    this.validateField(name, value);
  }
  
  // Validates a single field
  private validateField(name: string, value: any): void {
    const fields = this.inputFieldsSubject.value;
    const field = fields.find(f => f.name === name);
    
    if (!field || !field.validators || field.validators.length === 0) {
      return;
    }
    
    const currentErrors = {...this.inputErrorsSubject.value};
    let errorMessage = '';
    
    for (const validator of field.validators) {
      let isValid = true;
      
      switch (validator.type) {
        case 'required':
          isValid = value !== null && value !== undefined && value !== '';
          break;
        case 'minLength':
          isValid = !value || String(value).length >= (validator.value || 0);
          break;
        case 'maxLength':
          isValid = !value || String(value).length <= (validator.value || 0);
          break;
        case 'pattern':
          isValid = !value || new RegExp(validator.value).test(value);
          break;
        case 'email':
          isValid = !value || /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/.test(value);
          break;
        case 'min':
          isValid = !value || Number(value) >= (validator.value || 0);
          break;
        case 'max':
          isValid = !value || Number(value) <= (validator.value || 0);
          break;
        case 'custom':
          isValid = validator.validatorFn ? validator.validatorFn(value) : true;
          break;
      }
      
      if (!isValid) {
        errorMessage = validator.message;
        break;
      }
    }
    
    if (errorMessage) {
      currentErrors[name] = errorMessage;
    } else {
      delete currentErrors[name];
    }
    
    this.inputErrorsSubject.next(currentErrors);
  }
  
  // Validate all fields
  validateAllFields(): boolean {
    const fields = this.inputFieldsSubject.value;
    const values = this.inputValuesSubject.value;
    let isValid = true;
    
    fields.forEach(field => {
      this.validateField(field.name, values[field.name]);
    });
    
    // Check if there are any errors
    const errors = this.inputErrorsSubject.value;
    return Object.keys(errors).length === 0;
  }
  
  confirm(result: boolean = true, data?: any): void {
    // For input dialog, submit the form data
    if (this.isInputDialogSubject.value) {
      // Validate all fields first
      if (result && !this.validateAllFields()) {
        // Don't close if validation fails
        return;
      }
      
      // If validation passed, close and return the input values
      this.close();
      this.dialogResultSubject.next({ 
        isSuccess: result, 
        data: result ? this.inputValuesSubject.value : undefined 
      });
    } else {
      // Regular dialog behavior
      this.close();
      this.dialogResultSubject.next({ isSuccess: result, data });
    }
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