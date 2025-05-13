import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { AVAILABLE_GENRES, Book } from '../../../models/book.model';

@Component({
  selector: 'app-book-tags-genres',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-tags-genres.component.html',
  styleUrl: './book-tags-genres.component.css'
})
export class BookTagsGenresComponent {
  @Input() book: Book | null = null;
  @Output() submitBook = new EventEmitter<Partial<Book>>();
  @Output() previousStep = new EventEmitter<void>();
  AVAILABLE_GENRES = AVAILABLE_GENRES;
  tags: string[] = [];
  selectedGenres: string[] = [];
  tagsForm: FormGroup;
  tagsFormSubmitted = false;
  
  constructor(private fb: FormBuilder) {
    this.tagsForm = this.fb.group({
      newTag: ['', [Validators.minLength(2), Validators.maxLength(20)]]
    });
  }
  
  ngOnInit(): void {
    if (this.book) {
      this.tags = [...(this.book.tags || [])];
      this.selectedGenres = [...(this.book.genres || [])];
    }
  }
  
  addTag(): void {
    const tagControl = this.tagsForm.get('newTag');
    const newTag = tagControl?.value?.trim();
    
    if (newTag && tagControl?.valid && !this.tags.includes(newTag)) {
      this.tags.push(newTag);
      tagControl.setValue('');
    }
  }
  
  removeTag(tag: string): void {
    this.tags = this.tags.filter(t => t !== tag);
  }
  
  toggleGenre(genre: string): void {
    if (this.selectedGenres.includes(genre)) {
      this.selectedGenres = this.selectedGenres.filter(g => g !== genre);
    } else {
      this.selectedGenres.push(genre);
    }
  }
  
  onSubmit(): void {
    this.tagsFormSubmitted = true;
    
    if (this.tags.length > 0 && this.selectedGenres.length > 0) {
      this.submitBook.emit({
        tags: this.tags,
        genres: this.selectedGenres
      });
    }
  }
  
  onPrevious(): void {
    this.previousStep.emit();
  }
}
