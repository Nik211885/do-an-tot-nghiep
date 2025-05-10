import { Component, Input } from '@angular/core';
import { Genre } from '../../../models/book.model';

@Component({
  standalone: true,
  selector: 'app-book-genre-tag',
  imports: [],
  templateUrl: './book-genre-tag.component.html',
  styleUrl: './book-genre-tag.component.css'
})
export class BookGenreTagComponent {
  @Input() genre!: Genre;
}
