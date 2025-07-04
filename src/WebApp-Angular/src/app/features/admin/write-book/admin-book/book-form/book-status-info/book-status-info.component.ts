import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { Bookv1 } from '../../../models/book.model';

@Component({
  selector: 'app-book-status-info',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-status-info.component.html',
  styleUrl: './book-status-info.component.css'
})
export class BookStatusInfoComponent {
  @Input() ignore = false;
  @Input() book: Bookv1 | null = null;
  @Output() nextStep = new EventEmitter<Partial<Bookv1>>();
  @Output() previousStep = new EventEmitter<void>();

  statusForm: FormGroup;

  constructor(private fb: FormBuilder) {
    this.statusForm = this.fb.group({
      isCompleted: [false, Validators.required],
    });
  }

  ngOnInit(): void {
    if (this.book) {
      this.statusForm.patchValue({
        isCompleted: this.book.isCompleted
      });
    }
  }

  onNext(): void {
    if (this.statusForm.valid) {
      this.nextStep.emit(this.statusForm.value);
    }
  }

  onPrevious(): void {
    this.previousStep.emit();
  }
}
