import { Component, Input, OnInit } from '@angular/core';
import {CommentBookService} from "../../../services/comment-book.service";
import { CommentInputComponent } from "../../../../../shared/components/comment-input/comment-input.component";
import { CommonModule } from '@angular/common';
import { Comment, CommentComponent} from "../../../../../shared/components/comment/comment.component";
import {AuthService} from '../../../../../core/auth/auth.service';
import {UserModel} from '../../../../../core/models/user.model';
import {CreateComment} from '../../../models/comment.model';
import {PaginationComment} from '../../../models/rating.model';
import {ToastService} from '../../../../../shared/components/toast/toast.service';

@Component({
  standalone: true,
  selector: 'app-comment-book-section',
  imports: [CommentInputComponent, CommonModule, CommentComponent],
  templateUrl: './comment-book-section.component.html',
  styleUrl: './comment-book-section.component.css'
})
export class CommentBookSectionComponent implements OnInit {
  @Input() bookId!: string;
  @Input() commentCount: number = 0;
  currentPage = 1;
  pageSize  = 1;
  isNexPage: boolean = false;
  comments: Comment[] | undefined = undefined;
  currentUser: UserModel | undefined = undefined;

  constructor(private commentBookService: CommentBookService,
              private toastService: ToastService,
              private authService: AuthService){}

  ngOnInit(): void {
    this.authService.getCurrentUser()
      .subscribe({
        next: params => {
          if(params){
            this.currentUser = params;
          }
        }
      });
    this.loadCommentForBookForBook();
  }

  addComment(content: string): void {
    this.commentBookService.createComment({
      bookId: this.bookId,
      content: content,
    } as CreateComment)
      .subscribe({
        next: (comment) => {
          if(comment){
            if(this.currentUser){
              comment.user  = this.currentUser;
            }
            this.comments = [comment, ...(this.comments || [])];
            this.commentCount +=1;
          }
          else{
            this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          }
        },
        error: (error) => {
          this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          console.error(error);
        }
      })
  }

  addReply(data: {parentComment: Comment, content: string}): void {
    this.commentBookService.createComment({
      bookId: this.bookId,
      content: data.content,
      parentCommentId: data.parentComment.id,
    } as CreateComment)
      .subscribe({
        next: (comment) => {
          if(comment){
            if(this.currentUser){
              comment.user = this.currentUser;
            }
            data.parentComment.replyCount +=1;
            this.commentCount +=1;
            data.parentComment.comments =[comment, ...(data.parentComment.comments || [])];
          }
          else{
            this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          }
        },
        error: (error) => {
          this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          console.error(error);
        }
      })
  }
  loadCommentForBookForBook(){
    this.commentBookService.getCommentForBookWithPagination(this.bookId, this.currentPage, this.pageSize)
      .subscribe({
        next: params => {
          if(params){
            this.commentBookService.aggregateCommentWithUser(params)
            this.comments = [ ...(this.comments || []), ...params.items];
            this.currentPage +=1;
            this.isNexPage = params.hasNextPage;
          }
          else{
            this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          }
        },
        error: (error) => {
          this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
          console.error(error);
        }
      })
  }
  loadCommentReply(data: {
    parentComment: Comment;
    currentPage: number;
    pageSize: number;
    hasNextPage: {hasNextPage: boolean};
  })
  {
    console.log(data);
    console.log("Parent")
    this.commentBookService.getCommentReplyWithPagination(data.parentComment.id,
        data.currentPage, data.pageSize
      ).subscribe({
      next: (comment) => {
        if(comment){
          this.commentBookService.aggregateCommentWithUser(comment)
          data.parentComment.comments = [ ...(data.parentComment.comments || []), ...comment.items];
          data.hasNextPage.hasNextPage = comment.hasNextPage;
        }
        else{
          this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
        }
      },
      error: (error) => {
        this.toastService.error("Có lỗi trong quá trình viết bình luận vui lòng thử lại sau");
        console.error(error);
      }
    })
  }

}
