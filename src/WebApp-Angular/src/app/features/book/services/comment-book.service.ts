import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import {Comment } from '../../../shared/components/comment/comment.component';
import {HttpClient} from '@angular/common/http';
import {PaginationComment} from '../models/rating.model';
import {CreateComment} from '../models/comment.model';
import {UserModel} from '../../../core/models/user.model';

@Injectable({
  providedIn: 'root'
})

export class CommentBookService {
  constructor(private http: HttpClient) {}
  getCommentForBookWithPagination(bookId: string,
                                  pageNumber: number,
                                  pageSize: number)
    : Observable<PaginationComment>{
    const url = `book-review/comment/pagination/book?bookId=${bookId}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationComment>(url);
  }
  createComment(createComment: CreateComment) : Observable<Comment>  {
    const url = "book-review/comment/create";
    return this.http.post<Comment>(url, createComment);
  }
  getCommentReplyWithPagination(commentReplyId: string,
                                pageNumber: number,
                                pageSize: number)
    : Observable<PaginationComment> {
    const url = `book-review/comment/reply/pagination?commentReplyId=${commentReplyId}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationComment>(url);
  }
  getUserByIds(userIds: string[]) : Observable<UserModel[]>{
    let url = `/user-profile/by-ids?`;
    userIds.forEach(u=>{
      url += `userIds=${u}&`
    });
    return this.http.get<UserModel[]>(url);
  }
  aggregateCommentWithUser(paginationComment: PaginationComment)  {
    const userIds = [...new Set(paginationComment.items.map(item => item.reviewerId))];
    const users = this.getUserByIds(userIds)
      .subscribe({
        next:(value)=>{
          if(value){
            paginationComment.items.forEach(item=>{
              const useModel = value.find(x=>x.id == item.reviewerId);
              if(useModel){
                item.user = useModel;
              }
            })
          }
          else{
            return;
          }
        },
        error:(error)=>{
          console.log(error);
          return;
        }
      })
  }
}
