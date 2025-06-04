import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Bookv1 } from '../../../models/book.model';

@Component({
  selector: 'app-book-pricing-info',
  imports: [CommonModule, ReactiveFormsModule],
  standalone: true,
  templateUrl: './book-pricing-info.component.html',
  styleUrl: './book-pricing-info.component.css'
})
export class BookPricingInfoComponent {
  @Input() ignore = false;
  @Input() book: Bookv1 | null = null;
  @Output() nextStep = new EventEmitter<Partial<Bookv1>>();
  @Output() previousStep = new EventEmitter<void>();

  pricingForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.pricingForm = this.fb.group({
      isPaid: [false, Validators.required],
      price: [null],
      requiresRegistration: [false, Validators.required]
    });

    // Add conditional validation for price
    this.pricingForm.get('isPaid')?.valueChanges.subscribe(isPaid => {
      const priceControl = this.pricingForm.get('price');
      if (isPaid) {
        priceControl?.setValidators([Validators.required, Validators.min(0.99)]);
      } else {
        priceControl?.clearValidators();
        priceControl?.setValue(null);
      }
      priceControl?.updateValueAndValidity();
    });
  }

  ngOnInit(): void {
    if (this.book) {
      this.pricingForm.patchValue({
        isPaid: this.book.isPaid,
        price: this.book.price,
        requiresRegistration: this.book.requiresRegistration
      });
    }
  }

  onNext(): void {
    if (this.pricingForm.valid) {
      this.nextStep.emit(this.pricingForm.value);
    }
  }

  onPrevious(): void {
    this.previousStep.emit();
  }
}
