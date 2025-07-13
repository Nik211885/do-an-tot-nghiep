import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {
  ApproveStatus,
  ModerationDecision,
  ModerationViewModel,
  PaginationDecision,
  PaginationModeration, PaginationModerationForBookGroup
} from '../models/moderation.model';
import {catchError, map, Observable, of} from 'rxjs';
import {UserModel} from '../../../../core/models/user.model';

@Injectable({
  providedIn: 'root'
})
export class  ModerationService{
  constructor(private http: HttpClient) {}
  approvalChapter(id: string, note: string){
    const url = `moderation/chapter/approval?ChapterApprovalId=${id}&Note=${note}`;
    return this.http.post(url,null);
  }
  rejectChapter(id: string, note: string){
    const url =`/moderation/chapter/reject?ChapterApprovalId=${id}&Note=${note}`;
    return this.http.post(url, null);
  }
  getApprovalChapter(pageNumber: number, pageSize: number){
    const url = `moderation/chapter/approval/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}&status=${status}`;
    return this.http.get<PaginationModeration>(url);
  }
  getBookApprovalByIds(moderation: ModerationViewModel[]){
    let url = `moderation/book/ids?`;
    moderation.forEach((m)=>{
      url += `ids=${m.bookApprovalId}&`;
    })
    return this.http.get<any>(url).subscribe({
      next: (items) => {
        moderation.forEach((book)=>{
          const bookApproval = items.find((x: any)=>x.id === book.bookApprovalId);
          if(bookApproval){
            book.bookId = bookApproval.bookId;
            book.bookTitle  = bookApproval.bookTitle;
            book.authorId = bookApproval.authorId;
            book.isBookActive = bookApproval.isActive;
          }
        })
      }
    })
  }
  getApprovalById(id: string){
    const url = `moderation/chapter/detail?chapterApprovalId=${id}`;
    return this.http.get<ModerationViewModel>(url);
  }
  getDecisionChapter(id: string, pageNumber: number, pageSize: number) {
    const url =`moderation/chapter/decision/pagination?chapterApprovalId=${id}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationDecision>(url);
  }
  getModerationForBookGroup(pageNumber: number, pageSize: number, title?: string) {
    const url = `moderation/book/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}${title ? `&title=${title}` : ''}`;
    return this.http.get<PaginationModerationForBookGroup>(url);
  }
  getAllModerationForBookId(id: string){
    const url = `moderation/book/chapters?bookApprovalId=${id}`;
    return this.http.get<ModerationViewModel[]>(url);
  }
  activeForBook(id: string) : Observable<boolean>{
    const url = `moderation/active?bookId=${id}`;
    return this.http.put<boolean>(url, null, {observe: 'response'})
      .pipe(map(response=>{
        return response.status === 200;
      }),
        catchError(()=>  of(false)));
  }
  unActiveForBook(id: string) : Observable<boolean>{
    const url = `moderation/unactive?bookId=${id}`;
    return this.http.put<boolean>(url, null, {observe: 'response'})
      .pipe(map(response=>{
          return response.status === 200;
        }),
        catchError(()=>  of(false)));
  }
}
