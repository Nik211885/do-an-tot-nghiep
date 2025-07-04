import {HttpClient, HttpErrorResponse, HttpResponse} from "@angular/common/http";
import { Injectable } from "@angular/core";
import {catchError, map, Observable, of} from 'rxjs';
import {ChapterVersion} from '../models/book.model';
import {ChapterDiff} from '../chapter/chapter-editor/chapter-diff/chapter-diff.component';
import {DomSanitizer} from '@angular/platform-browser';

@Injectable({
    providedIn: "root"
})
export class ChapterVersionService{
    constructor(private httpClient: HttpClient, private sanitizer: DomSanitizer){}
    public getChapterVersionByChapterId(chapterId: string) : Observable<ChapterVersion[]>{
      const getChapterVersionUrl = `book-authoring/chapter/chapter-version?ChapterId=${chapterId}`;
      return this.httpClient.get<any>(getChapterVersionUrl)
        .pipe(map(res=>{
          return res.chapterVersions.map((item: any)=> this.mapToChapterVersion(item))
        }))
    }
    public renameChapterVersion(chapterVersion: ChapterVersion, chapterId: string) : Observable<boolean | string>{
      const renameChapterUrl = `book-authoring/chapter/rename-version?ChapterId=${chapterId}&ChapterVersionId=${chapterVersion.id}&NameVersion=${chapterVersion.name}`;
      return this.httpClient.put<any>(renameChapterUrl, null, { observe: 'response' }).pipe(
        map((res: HttpResponse<any>) => res.status === 204),
        catchError((error: HttpErrorResponse) => {
          const message = error.error?.message || 'Lỗi trong quá trình đặt tên phiên bản';
          return of(message);
        })
      );
    }
    public previewChapterVersion(chapterVersionId: string, chapterId: string): Observable<ChapterDiff>{
      const previewChapterVersionUrl = `book-authoring/chapter/preview-change-content?ChapterId=${chapterId}&ChapterVersionId=${chapterVersionId}`
      return this.httpClient.get<any>(previewChapterVersionUrl).pipe(
        map(res=>({
          chapterId: res.chapterId,
          chapterVersionId: res.chapterVersionId,
          dateCreateVersion: new Date(res.lastModified),
          version: res.version,
          diffContent: this.sanitizer.bypassSecurityTrustHtml(res.contentPretty) as string,
          diffTitle: this.sanitizer.bypassSecurityTrustHtml(res.titlePretty) as string,
        }) as ChapterDiff)
      )
    }

    private mapToChapterVersion(res: any): ChapterVersion {
      return  {
        id: res.id,
        name: res.name,
        createVersion: res.version,
        dateCreateVersion: new Date(res.createdDateTime)
      } as ChapterVersion;
    }
}
