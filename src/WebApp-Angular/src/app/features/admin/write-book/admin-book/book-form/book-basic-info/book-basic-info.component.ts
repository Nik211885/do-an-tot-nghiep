import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Book } from '../../../models/book.model';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-book-basic-info',
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-basic-info.component.html',
  styleUrl: './book-basic-info.component.css'
})
export class BookBasicInfoComponent {
  @Input() book: Book | null = null;
  @Output() nextStep = new EventEmitter<Partial<Book>>();
  
  basicInfoForm: FormGroup;
  
  get coverImageUrl(): string | null {
    const url = this.basicInfoForm.get('coverImage')?.value;
    return url && url.trim() !== '' ? url : null;
  }
  
  constructor(private fb: FormBuilder) {
    this.basicInfoForm = this.fb.group({
      title: ['', [Validators.required]],
      description: ['', [Validators.required]],
      coverImage: ['']
    });
  }
  
  ngOnInit(): void {
    if (this.book) {
      this.basicInfoForm.patchValue({
        title: this.book.title,
        description: this.book.description,
        coverImage: this.book.coverImage
      });
    }
  }
  
  onNext(): void {
    if (this.basicInfoForm.valid) {
      this.nextStep.emit(this.basicInfoForm.value);
    }
  }
}
