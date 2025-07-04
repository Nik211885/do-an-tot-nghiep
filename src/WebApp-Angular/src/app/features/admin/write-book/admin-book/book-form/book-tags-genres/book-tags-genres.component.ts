import { CommonModule } from '@angular/common';
import {Component, EventEmitter, Input, OnInit, Output} from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import {AVAILABLE_GENRES, Bookv1, Genre} from '../../../models/book.model';
import {GenresService} from '../../../services/genere.service';

@Component({
  selector: 'app-book-tags-genres',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './book-tags-genres.component.html',
  styleUrl: './book-tags-genres.component.css'
})
export class BookTagsGenresComponent implements OnInit {
  @Input()  ignore = false;
  @Input() book: Bookv1 | null = null;
  @Output() submitBook = new EventEmitter<Partial<Bookv1>>();
  @Output() previousStep = new EventEmitter<void>();
  genres: Genre[] = [];
  tags: string[] = [];
  selectedGenres: string[] = [];
  tagsForm: FormGroup;
  tagsFormSubmitted = false;

  constructor(private fb: FormBuilder, private genreService: GenresService) {
    this.tagsForm = this.fb.group({
      newTag: ['', [Validators.minLength(2), Validators.maxLength(20)]]
    });
  }

  ngOnInit(): void {
    this.genreService.getAllGenre().subscribe({
      next: (genres) => {
        this.genres = genres;
      },
      error: (err) => {
        console.error('Error fetching genres:', err);
      }
    });
    if (this.book) {
      this.tags = [...(this.book.tags || [])];
      this.selectedGenres = [...(this.book.genres.map(s=>s.id) || [])];
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

  toggleGenre(genre: Genre): void {
    if (this.selectedGenres.includes(genre.id)) {
      this.selectedGenres = this.selectedGenres.filter(g => g !== genre.id);
    } else {
      this.selectedGenres.push(genre.id);
    }
  }

  onSubmit(): void {
    this.tagsFormSubmitted = true;

    if (this.tags.length > 0 && this.selectedGenres.length > 0) {
      this.submitBook.emit({
        tags: this.tags,
        genres: this.selectedGenres.map(x=>({
          id: x
        } as Genre))
      });
    }
  }

  onPrevious(): void {
    this.previousStep.emit();
  }
}
