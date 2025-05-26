import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormGroup, FormBuilder, Validators, ReactiveFormsModule } from '@angular/forms';
import { Book } from '../../../models/book.model';
import { CommonModule } from '@angular/common';
import {ImageUploadComponent} from '../../../../../../shared/components/image-upload/image-upload.component';

@Component({
  standalone: true,
  selector: 'app-book-basic-info',
  imports: [CommonModule, ReactiveFormsModule, ImageUploadComponent],
  templateUrl: './book-basic-info.component.html',
  styleUrl: './book-basic-info.component.css'
})
export class BookBasicInfoComponent {
  @Input() book: Book | null = null;
  @Output() nextStep = new EventEmitter<Partial<Book>>();

  basicInfoForm: FormGroup;

  getImageUpload(fileInfo: any){
    const url = fileInfo['secure_url'];
    if(url){
      this.basicInfoForm.get("coverImage")?.setValue(url);
    }
    return url;
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
      console.log(this.basicInfoForm.value);
      this.nextStep.emit(this.basicInfoForm.value);
    }
  }
}
