import { HttpClient } from '@angular/common/http';
import {Injectable} from '@angular/core';
import {Chapter} from '../models/book.model';
import {catchError, map, Observable, of, throwError} from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ReaderServices{
  constructor(private http: HttpClient) {}
  getChapterForBook(id: string): Observable<Chapter[]> {
    const url = `moderation/chapter-approval?bookId=${id}`;
    return this.http.get<any[]>(url).pipe(
      map(res =>
        res.map((item: any) => ({
          slug: item.chapterSlug  ,
          title: item.chapterTitle,
          version: item.chapterNumber,
        } as Chapter))
      )
    );
  }
  getChapterContent(bookId: string, chapterslug: string): Observable<string| true|false>{
    const url =`reader/chapter?chapterSlug=${chapterslug}&bookId=${bookId}`;
    return this.http.get<string>(url, { observe: 'response' }).pipe(
      map(response => response.body!),
      catchError(err => {
        if (err.status === 404) {
          return of(true);
        } else if (err.status === 403) {
          return of(false);
        } else {
          return throwError(() => err);
        }
      })
    );
  }
}
