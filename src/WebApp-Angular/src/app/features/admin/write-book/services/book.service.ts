import {Injectable} from '@angular/core';
import {BehaviorSubject, catchError, map, Observable, of} from 'rxjs';
import {Bookv1, Chapter, ChapterVersion, Genre} from '../models/book.model';
import {HttpClient, HttpErrorResponse, HttpResponse} from '@angular/common/http';
import {BookPolicy, BookReleaseType, CreateBookCommand} from '../models/create-book.model';
import {ToastService} from '../../../../shared/components/toast/toast.service';
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
        return res.map((item: any)=>({
          id: item.id,
          title: item.title,
          coverImage: item.avatarUrl,
          createdAt: item.createDateTimeOffset,
          updatedAt: item.lastUpdateDateTime,
          isCompleted: item.isComplete,
          description: item.description,
          price:item.policyReadBook.price,
          tags: item.tags.map((t: any) => t.tagName),
          slug: item.slug,
          genres: item.genres.map((t:any)=>t.name),
          isPaid: item.policyReadBook.policy === 'Paid',
          requiresRegistration: item.policyReadBook.policy === 'Subscription'
        }) as Bookv1);
      })
    )
  }

  getBook(slug: string): Observable<Bookv1 | undefined> {
    const getBookDetails = "/book-authoring/book/slug?Slug="
    return this.httpClient.get<any>(`${getBookDetails}${slug}`).pipe(
      map(res=>{
        return {
          id: res.id,
          title: res.title,
          coverImage: res.avatarUrl,
          createdAt: res.createDateTimeOffset,
          updatedAt: res.lastUpdateDateTime,
          isCompleted: res.isComplete,
          description: res.description,
          price:res.policyReadBook.price,
          tags: res.tags.map((t: any) => t.tagName),
          slug: res.slug,
          genres: res.genres.map((t:any)=>t.name),
          isPaid: res.policyReadBook.policy === 'Paid',
          requiresRegistration: res.policyReadBook.policy === 'Subscription'
        } as Bookv1 | undefined
      })
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
      genreIds: book.genres,
      bookReleaseType: book.isCompleted ? BookReleaseType.Complete : BookReleaseType.Serialized,
      readerBookPolicyPrice: book.price,
      readerBookPolicy: book.isPaid ? BookPolicy.Paid
        : book.requiresRegistration ? BookPolicy.Subscription : BookPolicy.Free

    } as CreateBookCommand).pipe(
      map(this.mapToBook))
  }

  updateBook(book: Bookv1): Observable<Bookv1> {
    const index = this.books.findIndex(b => b.id === book.id);

    if (index !== -1) {
      this.books[index] = {
        ...book,
        updatedAt: new Date()
      };

      this.booksSubject.next([...this.books]);
      this.saveToStorage();
    }

    return of(this.books[index]);
  }

  deleteBook(id: string): Observable<boolean> {
    const initialLength = this.books.length;
    this.books = this.books.filter(book => book.id !== id);

    if (this.books.length !== initialLength) {
      // Also delete all chapters for this book
      this.chapters = this.chapters.filter(chapter => chapter.bookId !== id);

      this.booksSubject.next([...this.books]);
      this.saveToStorage();
      return of(true);
    }

    return of(false);
  }

  // Chapter methods
  getChapters(bookSlug: string): Observable<Chapter[]> {
    const getChapter = "book-authoring/chapter/by-book-slug?Slug=";

    return this.httpClient.get<any>(`${getChapter}${bookSlug}`).pipe(
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
      genres: res.genres?.map((g: any) => g.name) ?? [],
      isPaid: res.policyReadBook?.policy === 'Paid',
      requiresRegistration: res.policyReadBook?.policy === 'Subscription'
    };
  }

}
