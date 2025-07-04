import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Genere } from '../../../models/genere.model';
import { Router } from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-genre-card',
  imports: [],
  templateUrl: './genere-card.component.html',
  styleUrl: './genere-card.component.css'
})
export class GenereCardComponent {
  @Input() genre!: Genere;
  @Output() selectGenre = new EventEmitter<Genere>();

  constructor(private router: Router) {}

  onSelectGenre(): void {
    this.selectGenre.emit(this.genre);
    this.router.navigate(['/genere', this.genre.slug]);
  }
}
