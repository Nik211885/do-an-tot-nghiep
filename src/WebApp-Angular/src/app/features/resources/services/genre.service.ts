import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {CreateGenre, GenreViewModel, PaginationGenreViewModel} from '../models/genres.model';
import {catchError, map, Observable, of} from 'rxjs';

@Injectable({
  providedIn: 'root'
})

export class GenreService{
  constructor(private http: HttpClient) {}
  createGenre(createGenere: CreateGenre) : Observable<GenreViewModel> {
    const url =  `book-authoring/genre/create`;
    return this.http.post<GenreViewModel>(url,createGenere);
  }
  updateGenres(id: string, updateGenre: CreateGenre) : Observable<boolean> {
    const url = `book-authoring/genre/update?id=${id}`;
    return this.http.put<boolean>(url, updateGenre,{observe:'response'})
      .pipe(map(response=>{
        return response.status ===  200;
      }),
      catchError(()=>  of(false)))
  }
  changeActive(id: string) : Observable<boolean>{
    const url = `book-authoring/genre/change-active?id=${id}`;
    return this.http.put<boolean>(url, null, {observe:'response'})
      .pipe(
        map(response=>{
          return response.status ===  200;
        }),
         catchError(()=>  of(false))
      )
  }
  getGenresWithPagination(pageNumber: number, pageSize: number)
    : Observable<PaginationGenreViewModel> {
    const url = `book-authoring/genre/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.http.get<PaginationGenreViewModel>(url);
  }
  getGenreById(id: string) : Observable<GenreViewModel> {
    const url = `book-authoring/genre/id?id=${id}`;
    return this.http.get<GenreViewModel>(url);
  }
}
