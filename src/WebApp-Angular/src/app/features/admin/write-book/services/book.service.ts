import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { Book, Chapter } from '../models/book.model';

@Injectable({
  providedIn: 'root'
})
export class BookService {
  private books: Book[] = [];
  private chapters: Chapter[] = [];
  
  private booksSubject = new BehaviorSubject<Book[]>([]);
  books$ = this.booksSubject.asObservable();
  
  private currentBookSubject = new BehaviorSubject<Book | null>(null);
  currentBook$ = this.currentBookSubject.asObservable();

  constructor() {
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

  getBooks(): Observable<Book[]> {
    return this.books$;
  }

  getBook(id: string): Observable<Book | undefined> {
    const book = this.books.find(b => b.id === id);
    return of(book);
  }

  setCurrentBook(book: Book | null): void {
    this.currentBookSubject.next(book);
  }

  createBook(book: Book): Observable<Book> {
    const newBook = {
      ...book,
      id: Date.now().toString(),
      createdAt: new Date(),
      updatedAt: new Date()
    };
    
    this.books.push(newBook);
    this.booksSubject.next([...this.books]);
    this.saveToStorage();
    
    return of(newBook);
  }

  updateBook(book: Book): Observable<Book> {
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
  getChapters(bookId: string): Observable<Chapter[]> {
    const bookChapters = this.chapters.filter(chapter => chapter.bookId === bookId)
      .sort((a, b) => a.chapterNumber - b.chapterNumber);
    return of(bookChapters);
  }

  getChapter(id: string): Observable<Chapter | undefined> {
    const chapter = this.chapters.find(c => c.id === id);
    return of(chapter);
  }

  createChapter(chapter: Chapter): Observable<Chapter> {
    const newChapter = {
      ...chapter,
      id: Date.now().toString(),
      createdAt: new Date(),
      updatedAt: new Date()
    };
    
    this.chapters.push(newChapter);
    this.saveToStorage();
    
    return of(newChapter);
  }

  updateChapter(chapter: Chapter): Observable<Chapter> {
    const index = this.chapters.findIndex(c => c.id === chapter.id);
    
    if (index !== -1) {
      this.chapters[index] = {
        ...chapter,
        updatedAt: new Date()
      };
      
      this.saveToStorage();
    }
    
    return of(this.chapters[index]);
  }

  deleteChapter(id: string): Observable<boolean> {
    const initialLength = this.chapters.length;
    this.chapters = this.chapters.filter(chapter => chapter.id !== id);
    
    if (this.chapters.length !== initialLength) {
      this.saveToStorage();
      return of(true);
    }
    
    return of(false);
  }
}