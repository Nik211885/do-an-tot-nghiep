import {Injectable} from "@angular/core";
import {HttpClient, HttpErrorResponse} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';
import {Book, BookPolicy, Genre, PaginationBook, PolicyReadBook} from '../models/book.model';
import {BookReviewModel} from '../models/book-review.model';
import {BookFavoriteViewModel} from '../models/book-favorite.model';
import {RatingViewModel} from '../models/rating.model';
import {OrderService} from "../../admin/order/services/order.service";
import {OrderStatus} from '../../admin/order/models/order.model';
import {ErrorModel} from '../../../core/models/error.model';

@Injectable({
  providedIn: "root",
})
export class PublicBookService {
  constructor(private http: HttpClient,
              private orderService: OrderService) {}
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
  getFavoriteBook(ids: string[]) : Observable<BookFavoriteViewModel[]> {
    const url = "user-profile/book/favorite-in";
    return this.http.post<BookFavoriteViewModel[]>(url, ids);
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
  getAllBookWithPagination(pageNumber: number, pageSize: number): Observable<PaginationBook> {
    const url = `public-books/all?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.http.get<PaginationBook>(url).pipe(
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
  getBookByTagNameWithPagination(tag: string, pageNumber: number, pageSize: number) : Observable<PaginationBook>{
    const url = `public-books/tag?tag=${tag}&PageNumber=${pageNumber}&PageSize=${pageSize}`
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
    );
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
                  item.meanRatingStar = item.coutRating
                    ? parseFloat((item.rating / item.coutRating).toFixed(1))
                    : 0;
                  item.coutComment = bookReview?.commentCount ?? 0;
                })
            }
          }
        },
        error: err => {
          console.error(err);
        }
      })
    this.getFavoriteBook(ids)
      .subscribe({
        next: (result) => {
          if(result){
            paginationBook.items
              .forEach(item => {
                const favoriteBook = result
                  .find(x=>x.favoriteBookId === item.id);
                item.isFavorite = favoriteBook ? true : false;
              })
          }
        },
        error: err => {
          console.error(err)
        }
      })
    const idsBookPaid = paginationBook.items
      .filter(i=> i.policyReadBook.bookPolicy === BookPolicy.Paid)
      .map(b=>b.id);
    this.orderService.getOrderInBookIdsHasSuccess(idsBookPaid)
      .subscribe({
        next: (result) => {
          if(result && result.length > 0){
            paginationBook.items.forEach(item=>{
              const order = result
                .find(x=>x.orderItems
                  .find(y=>y.bookId == item.id));
              if(order && order.status === OrderStatus.Success){
                item.isPayemnt = true;
              }
            })
          }
        },
        error: err => {
          console.error(err);
        }
      })
  }
  getMyBook(currentPage: number, pageSize: number) : Observable<PaginationBook>{
    const url = `public-books/my-book?PageNumber=${currentPage}&PageSize=${pageSize}`;
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
    );
  }
  getBookReviewHasTopView(top: number) : Observable<BookReviewModel[]>{
    const url = `book-review/top/view?top=${top}`;
    return this.http.get<BookReviewModel[]>(url);
  }
  getPublicBookBySlug(slug: string) : Observable<Book>{
    const url =  `public-books/slug?slug=${encodeURIComponent(slug)}`;
    return this.http.get<Book>(url).pipe(
      map(this.mapToBook)
    )
  }
  favoriteBook(id: string): Observable<boolean> {
    const url = `user-profile/book/favorite?BookId=${id}`;
    return this.http.get<any>(url).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }
  unFavoriteBook(id: string): Observable<boolean> {
    const url = `user-profile/book/un-favorite?BookId=${id}`
    return this.http.get<any>(url).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }
  getTopGenres(top: number) : Observable<Genre>{
    const url =`book-authoring/top/genre?top=${top}`;
    return this.http.get<Genre>(url);
  }
  getBookWithPaginationForTerm(searchTerm: string, pageNumber: number, pageSize: number)
    : Observable<PaginationBook> {
    const url = `public-books/search?search=${searchTerm}&PageNumber=${pageNumber}&PageSize=${pageSize}`;
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
    );
  }
  getBookBoolPrefix(prefix: string): Observable<string[]>{
    const url = `public-books/bool-prefix?search=${prefix}`;
    return this.http.get<string[]>(url);
  }
  createRatingBook(id: string, starValue: number) : Observable<boolean | ErrorModel>{
    const  url = `book-review/rating/create`;
    const body = {
      bookId: id,
      starValue
    }
    return this.http.post<any>(url, body).pipe(
      map(() => true),
      catchError((err: HttpErrorResponse) => {
        const error: ErrorModel = {
          title: err.error?.title || 'Bad Request',
          status: err.status,
          error: err.error?.error || 'Request is invalid'
        };
        return of(error);
      })
    );
  }
  getMyRatingForBook(id: string) : Observable<RatingViewModel>{
    const url = `book-review/rating/book-and-user?bookId=${id}`;
    return this.http.get<RatingViewModel>(url);
  }
  updateRating(id: string, starValue: number) : Observable<boolean>{
    const url = `book-review/rating/update`;
    const body = {
      id,
      star: starValue
    };
    return this.http.post<any>(url, body).pipe(
      map(() => true),
      catchError(() => of(false))
    )
  }
  getMyRatingForBookIds(ids: string[]): Observable<RatingViewModel[]>{
    let url = "/book-review/my-rating/books-in?";
    ids.forEach(id=>{
      url += `bookIds=${id}&`
    });
    return this.http.get<RatingViewModel[]>(url);
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
