import {Injectable} from '@angular/core';
import {BehaviorSubject, catchError, map, Observable, of} from 'rxjs';
import {Bookv1, Chapter, ChapterVersion, Genre, PaginationBook} from '../models/book.model';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {BookPolicy, BookReleaseType, CreateBookCommand, UpdateBookCommand} from '../models/create-book.model';
import {CreateChapterCommand, UpdateChapterCommand} from '../models/create-chapter.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private books: Bookv1[] = [];
  private chapters: Chapter[] = [];

  private booksSubject = new BehaviorSubject<Bookv1[]>([]);
  books$ = this.booksSubject.asObservable();

  private currentBookSubject = new BehaviorSubject<Bookv1 | null>(null);
  currentBook$ = this.currentBookSubject.asObservable();

  constructor( private httpClient: HttpClient) {
    // Load from local storage when available
    const storedBooks = localStorage.getItem('books');
    const storedChapters = localStorage.getItem('chapters');

    if (storedBooks) {
      this.books = JSON.parse(storedBooks);
      this.booksSubject.next(this.books);
    }

    if (storedChapters) {
      this.chapters = JSON.parse(storedChapters);
    }
  }

  private saveToStorage(): void {
    localStorage.setItem('books', JSON.stringify(this.books));
    localStorage.setItem('chapters', JSON.stringify(this.chapters));
  }

  getBooks(): Observable<Bookv1[]> {
    const urlGetMyBooks = "/book-authoring/book/my-book";
    return this.httpClient.get<any>(urlGetMyBooks).pipe(
      map(res=>{
        return res.map((item: any)=> this.mapToBook(item));
      })
    )
  }

  getBookBySlug(slug: string): Observable<Bookv1 | undefined> {
    const getBookDetails = "/book-authoring/book/slug?Slug="
    return this.httpClient.get<any>(`${getBookDetails}${encodeURIComponent(slug)}`).pipe(
      map(res=>this.mapToBook(res))
    )
  }
  getBookById(id: string): Observable<Bookv1 | undefined> {
    const getBookDetailById = `/book-authoring/book/id?id=${id}`;
    return this.httpClient.get<any>(getBookDetailById).pipe(
      map(res=>this.mapToBook(res))
    )
  }


  setCurrentBook(book: Bookv1 | null): void {
    this.currentBookSubject.next(book);
  }

  createBook(book: Bookv1): Observable<Bookv1> {
    const urlCreateBook = "/book-authoring/book/create";
    return this.httpClient.post<any>(urlCreateBook,{
      title: book.title,
      description: book.description,
      avatarUrl: book.coverImage,
      tagsName: book.tags,
      genreIds: book.genres.map(genre => genre.id),
      bookReleaseType: book.isCompleted ? BookReleaseType.Complete : BookReleaseType.Serialized,
      readerBookPolicyPrice: book.price,
      readerBookPolicy: book.isPaid ? BookPolicy.Paid
        : book.requiresRegistration ? BookPolicy.Subscription : BookPolicy.Free

    } as CreateBookCommand).pipe(
      map(this.mapToBook))
  }
  markBookComplete(id: string) : Observable<Bookv1>{
    const url = `/book-authoring/book/mark-complete?id=${id}`;
    return this.httpClient.put<any>(url, null).pipe(
      map(res=>this.mapToBook(res))
    )
  }

  updateBook(book: UpdateBookCommand, id: string): Observable<Bookv1> {
    const url = `/book-authoring/book/update?id=${id}`;
    return this.httpClient.post<any>(url,book).pipe(
      map(res=>this.mapToBook(res))
    )
  }

  deleteBook(id: string): Observable<boolean> {
    let url = `/book-authoring/book/delete?Id=${id}`;
    return this.httpClient.delete<void>(url, {observe:'response'}).pipe(
      map(response=>response.status === 204),
    );
  }

  getBookWithPagination(pageNumber: number, pageSize: number,
                        bookReleaseType?: BookReleaseType, bookPolicy?: BookPolicy,
                        tag?: string, genre?: string, title?: string)
    : Observable<PaginationBook | undefined>{
    let url = "book-authoring/book/pagination/filter?";
    if(pageNumber && pageNumber > 0){
      url += "&PageNumber=" + pageNumber;
    }
    if(pageSize && pageSize >= 1){
      url += "&PageSize=" + pageSize;
    }
    if(bookReleaseType){
      url += "&BookReleaseType=" + bookReleaseType;
    }
    if(bookPolicy){
      url += "&BookPolicy=" + bookPolicy;
    }
    if(tag){
      url += "&Tag=" + tag;
    }
    if(genre){
      url += "&Genre=" + genre;
    }
    if(title){
      url += "&Title=" + title;
    }
    return this.httpClient.get<any>(url).pipe(
      map(res=>{
        return {
          items: res.items.map((item:any)=> this.mapToBook(item)),
          pageNumber: res.pageNumber,
          totalPages: res.totalPages,
          totalCount: res.totalCount,
          hasPreviousPage: res.hasPreviousPage,
          hasNextPage: res.hasNextPage,
        }  as PaginationBook
      })
    )
  }

  // Chapter methods
  getChapters(bookSlug: string): Observable<Chapter[]> {
    const getChapter = "book-authoring/chapter/by-book-slug?Slug=";

    return this.httpClient.get<any>(`${getChapter}${encodeURIComponent(bookSlug)}`).pipe(
      map(res => {
        return res.map((item: any) => this.mapToChapter(item));
      })
    );
  }
  getChapter(slug: string): Observable<Chapter | undefined> {
    const getChapterBySlugUrl = "book-authoring/chapter/slug?Slug=";
    return this.httpClient.get<any>(`${getChapterBySlugUrl}${slug}`).pipe(
      map(res => this.mapToChapter(res)));
  }

  createChapter(chapter: Chapter): Observable<Chapter> {
    const createChapterUrl = "book-authoring/chapter/create";
    return this.httpClient.post<any>(createChapterUrl,{
      bookId: chapter.bookId,
      title: chapter.title,
      content: chapter.content,
      chapterNumber: chapter.chapterNumber,
    } as CreateChapterCommand).pipe(
      map(res => this.mapToChapter(res)));
  }

  updateChapter(chapter: Chapter): Observable<Chapter> {
    const updateChapterUrl = "book-authoring/chapter/update?id="
    return this.httpClient.put<any>(`${updateChapterUrl}${chapter.id}`,{
      title: chapter.title,
      content: chapter.content,
      chapterNumber: chapter.chapterNumber,
    } as UpdateChapterCommand).pipe(
      map(res => this.mapToChapter(res)));
  }
  submitAndReview(chapterId: string): Observable<Chapter>{
    const url = `/book-authoring/chapter/submit-review?Id=${chapterId}`;
    return this.httpClient.post<any>(url, null)
      .pipe(map(res=> this.mapToChapter(res)))

  }

  deleteChapter(id: string): Observable<boolean | string> {
    const url = `book-authoring/chapter/delete?Id=${id}`;
    return this.httpClient.delete(url, { observe: 'response' }).pipe(
      map((res: HttpResponse<any>) => res.status === 204),
      catchError((error: HttpErrorResponse) => {
        const message = error.error?.message || 'Lỗi khi xóa chương';
        return of(message);
      })
    );
  }
  rollbackChapter(chapterVersionId: string, chapterId: string){
    const url = `book-authoring/chapter/roll-back?ChapterId=${chapterId}&ChapterVersionId=${chapterVersionId}`;
    return this.httpClient.get<any>(url)
    .pipe(map(res=>this.mapToChapter(res)));
  }

  private mapToChapter(res: any): Chapter {
    let updatedAt: Date = new Date(res.createdDateTime);

    if (res.chapterVersions?.length) {
      const latest = res.chapterVersions.reduce(
        (latest: any, current: any) =>
          new Date(current.createdDateTime) > new Date(latest.createdDateTime)
            ? current
            : latest
      );
      updatedAt = new Date(latest.createdDateTime);
    }
    const chapterVersion: ChapterVersion[] = res.chapterVersions
      .map((item: any) => ({
        id: item.id,
        name: item.name,
        dateCreateVersion: new Date(item.createdDateTime),
        createVersion: item.version
      }) as ChapterVersion)
      .sort((a: ChapterVersion, b: ChapterVersion) =>
        b.dateCreateVersion.getTime() - a.dateCreateVersion.getTime()
      );
    return {
      id: res.id,
      bookId: res.bookId,
      content: res.content,
      title: res.title,
      slug: res.slug,
      createdAt: new Date(res.createdDateTime),
      updatedAt,
      chapterNumber: res.chapterNumber,
      chapterVersion: chapterVersion
    } as Chapter;
  }
  private mapToBook(res: any): Bookv1 {
    return {
      id: res.id,
      title: res.title,
      coverImage: res.avatarUrl,
      createdAt: res.createDateTimeOffset,
      updatedAt: res.lastUpdateDateTime,
      isCompleted: res.isComplete,
      description: res.description,
      price: res.policyReadBook?.price,
      tags: res.tags?.map((t: any) => t.tagName) ?? [],
      slug: res.slug,
      genres: res.genres?.map((g: any) => ({
        id: g.id,
        name: g.name,
      } as Genre)) ?? [],
      isPaid: res.policyReadBook?.policy === 'Paid',
      requiresRegistration: res.policyReadBook?.policy === 'Subscription'
    };
  }

}
