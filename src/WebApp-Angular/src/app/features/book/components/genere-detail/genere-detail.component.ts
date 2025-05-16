import { Component, OnInit } from '@angular/core';
import { Genere } from '../../models/genere.model';
import { Book } from '../../models/book.model';
import { ActivatedRoute, Router } from '@angular/router';
import { GenereService } from '../../services/genere.service';

@Component({
  selector: 'app-genere-detail',
  imports: [],
  templateUrl: './genere-detail.component.html',
  styleUrl: './genere-detail.component.css'
})
export class GenereDetailComponent implements OnInit {
  genre: Genere | undefined;
  books: Book[] = [];
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private genereService: GenereService
  ) {}
  ngOnInit(): void {
    this.route.paramMap.subscribe(params => {
      const slug = params.get('slug');
      console.log(slug)
      if (slug) {
        const genere = this.genereService.getGenereBySlug(slug).subscribe(genere=>{
          if(genere){
            this.genre = genere;
          }
        });
      }
    });
  }
}
