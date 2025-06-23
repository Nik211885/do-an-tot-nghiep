import { Injectable } from "@angular/core";
import {HttpClient} from '@angular/common/http';
import {map} from 'rxjs';
import {Book, Genre} from '../models/book.model';

@Injectable({
  providedIn: "root",
})
export class PublicBookService {
  constructor(private http: HttpClient) {}
  getBookInIds(ids: string[]) {
    const url = "/public-books/by-ids";
    return this.http.post<any>(url, ids).pipe(
      map(res=>{
        return res.map(this.mapToBook);
      }
    ));
  }
  private mapToBook(res: any): Book {
    return {
      authorId: res.authorId,
      author: res.authorName,
      id: res.id,
      title: res.title,
      avatarUrl: res.avatarUrl,
      description: res.description,
      created: res.createDateTimeOffset,
      lastModified: res.updateDateTimeOffset,
      tagNames: res.tags,
      slug: res.slug,
      isCompeleted: res.isCompleted,
      bookReleaseType: res.bookReleaseType,
      genres: res.genres.map((g: any) => ({
        id: g.id,
        name: g.name,
        slug: g.slug,
      } as Genre))
    } as Book;
  }
}
