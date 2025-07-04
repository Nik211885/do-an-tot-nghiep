import { Component, EventEmitter, Input, Output } from '@angular/core';
import {AuthorServcie} from '../../services/author.service'
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-follow-button',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './follow-button.component.html',
  styleUrl: './follow-button.component.css'
})
export class FollowButtonComponent {
  @Input() authorId!: string;
  @Input() isFollowing: boolean = false;
  @Input() size: 'sm' | 'md' | 'lg' = 'md';
  @Input() displayText: boolean = true;
  @Output() followChanged = new EventEmitter<boolean>();
  constructor(private authorService: AuthorServcie) { }
  toggleFollow(): void {
    this.authorService.toggleFollow(this.authorId).subscribe(author => {
      if (author) {
        this.isFollowing = author.isFollowing;
        this.followChanged.emit(this.isFollowing);  
      }
    });
  }
}
