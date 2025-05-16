import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Author } from '../../models/author.model';
import { AuthorServcie } from '../../services/author.service';
import { FollowButtonComponent } from '../follow-button/follow-button.component';
import { CommonModule } from '@angular/common';

@Component({
  standalone: true,
  selector: 'app-author-detail',
  imports: [FollowButtonComponent,CommonModule],
  templateUrl: './author-detail.component.html',
  styleUrl: './author-detail.component.css'
})
export class AuthorDetailComponent implements OnInit{
  author!: Author;
  constructor(private route: ActivatedRoute,
    private authorService: AuthorServcie,
    private router: Router
  ){}
  ngOnInit(): void {
    const authorId = decodeURIComponent(this.route.snapshot.paramMap.get("id") || "")
    const author = this.authorService.getAuthorById(authorId);
    if(author){
      this.author = author;
    }
    else{
      this.router.navigate(["/error/not-found"]);
      return;
    }
  }
}
