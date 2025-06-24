import { Injectable } from "@angular/core";
import {HttpClient} from '@angular/common/http';
import {map, Observable} from 'rxjs';
import {Book, BookPolicy, Genre, PaginationBook, PolicyReadBook} from '../models/book.model';
import {BookReviewModel} from '../models/book-review.model';

@Injectable({
  providedIn: "root",
})
export class PublicBookService {
  constructor(private http: HttpClient) {}
  getBookInIds(ids: string[]) : Observable<Book[]> {
    const url = "/public-books/by-ids";
    return this.http.post<any>(url, ids).pipe(
      map(res=>{
        return res.map(this.mapToBook);
      }
    ));
  }
  getBookByGenreId(slug: string, pageNumber: number, pageSize: number)
    : Observable<PaginationBook> {
    const url = `public-books/genre?slug=${encodeURIComponent(slug)}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<any>(url).pipe(
      map(res=> {
        const items = res.items.map(this.mapToBook);
        return {
          items: items,
          pageNumber: res.pageNumber,
          pageSize: res.pageSize,
          totalPages: res.totalPages,
          totalCount: res.totalCount,
          hasPreviousPage:res.hasPreviousPage,
          hasNextPage:res.hasNextPage,
        } as PaginationBook})
    )
  }
  getBookReview(ids: string[]) : Observable<BookReviewModel[]> {
    let url = `book-review/by-ids?`;
    ids.forEach((id) => {
      url += `bookIds=${id}&`;
    })
    return this.http.get<BookReviewModel[]>(url);
  }
  getBookByPolicy(bookPolicy: BookPolicy, pageNumber: number, pageSize: number)
    : Observable<PaginationBook> {
    const url = `public-books/policy?policy=${bookPolicy}&pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<any>(url).pipe(
      map(res=> {
        const items = res.items.map(this.mapToBook);
        return {
          items: items,
          pageNumber: res.pageNumber,
          pageSize: res.pageSize,
          totalPages: res.totalPages,
          totalCount: res.totalCount,
          hasPreviousPage:res.hasPreviousPage,
          hasNextPage:res.hasNextPage,
        } as PaginationBook})
    )
  }

  paginationBookAggregate(paginationBook: PaginationBook){
    const ids = paginationBook
      .items.map(i=>i.id);
    this.getBookReview(ids)
      .subscribe({
        next: (result) => {
          if(result){
            if(paginationBook) {
              paginationBook.items
                .forEach(item => {
                  const bookReview = result
                    .find(r => r.bookId === item.id);
                  item.rating = bookReview?.ratingStar ?? 0;
                  item.coutRating = bookReview?.ratingCount ?? 0;
                })
            }
          }
        },
        error: err => {
          console.error(err);
        }
      })
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
      policyReadBook: {
        bookPolicy: res.bookPolicy,
        price: res.price,
      } as PolicyReadBook,
      genres: res.genres.map((g: any) => ({
        id: g.id,
        name: g.name,
        slug: g.slug,
      } as Genre))
    } as Book;
  }
}
