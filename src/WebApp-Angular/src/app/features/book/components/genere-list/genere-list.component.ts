import { Component, OnInit } from '@angular/core';
import { Genere } from '../../models/genere.model';
import { GenereService } from '../../services/genere.service';
import { GenereCardComponent } from './genere-card/genere-card.component';

@Component({
  standalone: true,
  selector: 'app-genere-list',
  imports: [GenereCardComponent],
  templateUrl: './genere-list.component.html',
  styleUrl: './genere-list.component.css'
})
export class GenereListComponent implements OnInit  {
  genres: Genere[] = [];
  constructor(private genereService: GenereService) {}
  ngOnInit(): void {
    this.genereService.getGeneres().subscribe(genres => {
      this.genres = genres;
    });
  }
  onSelectGenre(genre: Genere): void {
    this.genereService.selectGenere(genre);
  }
}
