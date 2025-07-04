import {Component, EventEmitter, Input, OnInit, Output, signal} from '@angular/core';
import {convertUtcToLocal,formatRelativeTime} from "../../../core/utils/date.until";
import { CommentInputComponent } from "../comment-input/comment-input.component";
import { CommonModule } from '@angular/common';
import {UserModel} from '../../../core/models/user.model';

export interface Comment {
  id: string;
  bookReviewId: string;
  reviewerId: string;
  content: string;
  user: UserModel;
  dateCreated: Date;
  comments: Comment[];
  parentCommentId?: string;
  replyCount: number;
}

@Component({
  standalone: true,
  selector: 'app-comment',
  imports: [CommentInputComponent,CommonModule],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent{
  currentPage: number =  1;
  pageSize: number = 1;
  hasNextPage = {
    hasNextPage: false,
  }
  isTurnOfComment = true;
  @Input() comment!: Comment;
  @Input() currentUser: UserModel | undefined = undefined;

  @Output() addNewReply = new EventEmitter<{parentComment: Comment, content: string}>();
  @Output() showMoreMessageEvent  = new EventEmitter<
    {
    parentComment: Comment;
    currentPage: number;
    pageSize: number;
    hasNextPage: {hasNextPage: boolean};
  }>();
  isReplying = false;


  toggleReplyForm(): void {
    this.isReplying = !this.isReplying;
  }

  addReply(content: string): void {
    this.addNewReply.emit({ parentComment: this.comment, content });
    this.isReplying = false;
    this.isTurnOfComment = false;
  }

  formatDate(date: Date): string {
    return formatRelativeTime(date);
  }

  showMoreComment(event: Event) {
    event.stopPropagation();
    if(this.isTurnOfComment){
      if(!(this.comment.comments && this.comment.comments.length > 0)){
        console.log("Child")
        this.showMoreMessageEvent.emit({
          parentComment: this.comment,
          currentPage: this.currentPage,
          pageSize: this.pageSize,
          hasNextPage: this.hasNextPage
        })
        this.currentPage +=1;
      }
      this.isTurnOfComment = false;
    }
    else{
      this.isTurnOfComment = true;
    }
  }

  moreMessage(event: Event) {
    if (this.comment.replyCount > 0) {
      event.stopPropagation();
      this.showMoreMessageEvent.emit({
        parentComment: this.comment,
        currentPage: this.currentPage,
        pageSize: this.pageSize,
        hasNextPage: this.hasNextPage
      })
      this.currentPage += 1;
    }
  }
}
