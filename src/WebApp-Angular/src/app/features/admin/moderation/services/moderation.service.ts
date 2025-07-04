import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {
  ApproveStatus,
  ModerationDecision,
  ModerationViewModel,
  PaginationDecision,
  PaginationModeration, PaginationModerationForBookGroup
} from '../models/moderation.model';
import {Observable, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class  ModerationService{
  constructor(private http: HttpClient) {}
  approvalChapter(id: string, note: string){
    const url = 'moderation/approval/chapter';
    const body = {
      id: id,
      note: note
    }
    return this.http.post(url,body)
  }
  rejectChapter(id: string, note: string){
    const url = 'moderation/reject/chapter';
    const body = {
      id: id,
      note: note
    }
    return this.http.post(url,body)
  }
  getApprovalChapter(pageNumber: number, pageSize: number, status: ApproveStatus){
    const url = `moderation/approval?PageNumber=${pageNumber}&PageSize=${pageSize}&status=${status}`;
    return this.http.get<PaginationModeration>(url);
  }
  getApprovalById(id: string){
    const url = `moderation/approval/id?bookApprovalId=${id}`;
    return this.http.get<ModerationViewModel>(url);
  }
  getDecisionChapter(id: string, pageNumber: number, pageSize: number) {
    const url =`moderation/decision/pagination?bookApprovalId=${id}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationDecision>(url);
  }
  getModerationForBookGroup(pageNumber: number, pageSize: number) {
    const url = `moderation/repository?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationModerationForBookGroup>(url);
  }
  getAllModerationForBookId(id: string){
    const url = `moderation/approval-for-book?bookId=${id}`;
    return this.http.get<ModerationViewModel[]>(url);
  }
}
