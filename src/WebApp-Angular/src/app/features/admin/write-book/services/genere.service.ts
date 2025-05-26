import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {Genre} from '../models/book.model';
import {Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class GenresService{
  private baseUrl = "/book-authoring/genre/all";
  constructor(private http: HttpClient) {}
  getAllGenre() : Observable<Genre[]> {
    return this.http.get<Genre[]>(this.baseUrl)
  }
}
