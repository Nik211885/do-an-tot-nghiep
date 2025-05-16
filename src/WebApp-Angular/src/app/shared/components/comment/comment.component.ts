import { Component, EventEmitter, Input, Output, signal } from '@angular/core';
import {convertUtcToLocal,formatRelativeTime} from "../../../core/utils/date.until";
import { CommentInputComponent } from "../comment-input/comment-input.component";
import { CommonModule } from '@angular/common';
export interface Comment {
  id: string;
  content: string;
  user: User;
  createdAt: Date;
  replies: Comment[];
}

export interface User {
  id: string;
  name: string;
  avatar?: string;
}

export interface ReplyComment{
  commentId: string;
  comment: string;
}
@Component({
  standalone: true,
  selector: 'app-comment',
  imports: [CommentInputComponent,CommonModule],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() comment!: Comment;
  @Input() currentUser: User | null = null;
  
  @Output() addNewReply = new EventEmitter<{parentId: string, content: string}>();
  
  isReplying = false;


  toggleReplyForm(): void {
    this.isReplying = !this.isReplying;
  }

  addReply(content: string): void {
    this.addNewReply.emit({ parentId: this.comment.id, content });
    this.isReplying = false;
  }

  formatDate(date: Date): string {
    return formatRelativeTime(convertUtcToLocal(date));
  }
}
