import { Component, OnInit } from '@angular/core';
import { Author } from '../../models/author.model';
import { AuthorServcie } from '../../services/author.service';
import { CommonModule } from '@angular/common';
import { RouterLink, RouterModule } from '@angular/router';
import { FollowButtonComponent } from '../follow-button/follow-button.component';

@Component({
  standalone: true,
  selector: 'app-author-list',
  imports: [CommonModule, RouterLink, FollowButtonComponent, RouterModule],
  templateUrl: './author-list.component.html',
  styleUrl: './author-list.component.css'
})
export class AuthorListComponent implements OnInit {
  authors: Author [] = [];
  constructor(private authorService: AuthorServcie){}
  ngOnInit(): void {
    this.authors = this.authorService.getAllAuthor();
  }

}
