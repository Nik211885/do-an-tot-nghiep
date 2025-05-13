import { Component, Input, OnInit, signal, ViewChild } from '@angular/core';
import {CommentBookService} from "../../../services/comment-book.service";
import { CommentInputComponent } from "../../../../../shared/components/comment-input/comment-input.component";
import { CommonModule } from '@angular/common';
import { Comment, CommentComponent, ReplyComment, User } from "../../../../../shared/components/comment/comment.component";


@Component({
  standalone: true,
  selector: 'app-comment-book-section',
  imports: [CommentInputComponent, CommonModule, CommentComponent],
  templateUrl: './comment-book-section.component.html',
  styleUrl: './comment-book-section.component.css'
})
export class CommentBookSectionComponent implements OnInit {
  @Input() bookId!: string;
  
  comments: Comment[] = [];
  
  // Example current user - in a real app, this would come from an auth service
  currentUser: User = {
    id: 'user-1',
    name: 'Current User',
    avatar: ''
  };
  constructor(private commentBookService: CommentBookService){}
  
  get totalComments(): number {
    return this.countComments(this.comments);
  }
  
  ngOnInit(): void {
    // Initialize with any comments passed in
    this.comments = this.commentBookService.getCommentByBookId(this.bookId);
  }
  
  addComment(content: string): void {
    const newComment: Comment = {
      id: `comment-${Date.now()}`,
      content,
      user: this.currentUser,
      createdAt: new Date(),
      replies: []
    };
    
    this.comments = [newComment, ...this.comments];
  }
  
  addReply(data: {parentId: string, content: string}): void {
    const { parentId, content } = data;
    
    const newReply: Comment = {
      id: `comment-${Date.now()}`,
      content,
      user: this.currentUser,
      createdAt: new Date(),
      replies: []
    };
    
    this.comments = this.addReplyToComment(this.comments, parentId, newReply);
  }
  
  private addReplyToComment(comments: Comment[], parentId: string, newReply: Comment): Comment[] {
    return comments.map(comment => {
      if (comment.id === parentId) {
        return {
          ...comment,
          replies: [...comment.replies, newReply]
        };
      } else if (comment.replies.length > 0) {
        return {
          ...comment,
          replies: this.addReplyToComment(comment.replies, parentId, newReply)
        };
      }
      return comment;
    });
  }
  
  private countComments(comments: Comment[]): number {
    let count = comments.length;
    
    for (const comment of comments) {
      if (comment.replies && comment.replies.length > 0) {
        count += this.countComments(comment.replies);
      }
    }
    
    return count;
  }
  
  // Generate demo comments for display purposes

}
