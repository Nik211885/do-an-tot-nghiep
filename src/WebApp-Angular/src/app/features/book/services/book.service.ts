import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Book, BookPolicy, BookReleaseType } from "../models/book.model";

@Injectable({
  providedIn: 'root'
})

export class BookService{
  constructor(private httpClient: HttpClient) {}
  books: Book[] = [
    {
      id: '1',
      title: 'The Way of Kings',
      rating: 4.8,
      coutRating: 24356,
      bookReleaseType: BookReleaseType.Complete,
      slug: 'the-way-of-kings-abcd1234',
      isCompeleted: true,
      author: 'Brandon Sanderson',
      genres: [
        { id: '1', name: 'Fantasy', slug:'a' },
        { id: '2', name: 'Epic' , slug:'b'}
      ],
      avatarUrl: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1605960703i/55974838.jpg',
      description: 'Roshar is a world of stone and storms. Uncanny tempests of incredible power sweep across the rocky terrain so frequently that they have shaped ecology and civilization alike.',
      tagNames: ['bestseller', 'series'],
      policyReadBook: {
        bookPolicy: BookPolicy.Free
      }
    },
    {
      id: '2',
      title: 'Project Hail Mary',
      rating: 4.7,
      isCompeleted: true,
      bookReleaseType: BookReleaseType.Serialized,
      chapters: [{
        id: '1',
        version: 1,
        title: 'Chapter 1'
      }, {
        id: '2',
        version: 2,
        title: 'Chapter 2'
      }],
      coutRating: 18542,
      slug: 'project-hail-mary-efgh5678',
      author: 'Andy Weir',
      genres: [
        { id: '3', name: 'Sci-Fi' , slug: 'a'},
        { id: '4', name: 'Adventure',  slug: 'a' }
      ],
      avatarUrl: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1605960703i/55974838.jpg',
      description: 'Ryland Grace is the sole survivor on a desperate, last-chance mission—and if he fails, humanity and the Earth itself will perish.',
      tagNames: ['space', 'science'],
      policyReadBook: {
        bookPolicy: BookPolicy.Paid,
        price: 100000
      }
    },
    {
      id: '3',
      title: 'The Night Circus',
      slug: 'the-night-circus-ijkl9012',
      rating: 4.5,
      isCompeleted: true,
      bookReleaseType: BookReleaseType.Complete,
      coutRating: 12876,
      author: 'Erin Morgenstern',
      genres: [
        { id: '5', name: 'Fantasy' , slug: 'a'},
        { id: '6', name: 'Romance', slug: 'a' }
      ],
      avatarUrl: 'https://images-na.ssl-images-amazon.com/images/S/compressed.photo.goodreads.com/books/1605960703i/55974838.jpg',
      description: 'The circus arrives without warning. No announcements precede it. It is simply there, when yesterday it was not.',
      tagNames: ['magical', 'competition'],
      policyReadBook: {
        bookPolicy: BookPolicy.Subscription
      }
    }
  ];
  getBoooks() : Book[] {
    return this.books;
  }
  getBookBySlug(slug: string): Book | null {
    return this.books.find(book => book.slug === slug) || null;
  }

}
