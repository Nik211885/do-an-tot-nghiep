import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import {BehaviorSubject, map, Observable, of} from "rxjs";
import { Book } from "../models/book.model";
import {Genere} from "../models/genere.model"

@Injectable({
    providedIn: "root"
})

export class GenereService{
    constructor(private readonly httpClient: HttpClient){}
    private selectedGenreSubject = new BehaviorSubject<Genere | null>(null);
    selectedGenre$ = this.selectedGenreSubject.asObservable();

    getGeneres(): Observable<Genere[]> {
        const url = "book-authoring/genre/all";
        return this.httpClient.get<any[]>(url).pipe(
          map(res => res.map(item => this.mapToViewModel(item)))
        );
    }
    selectGenere(genre: Genere): void {
        this.selectedGenreSubject.next(genre);
    }
    getGenereBySlug(slug: string) : Observable<Genere| undefined>{
      const url = `book-authoring/genre/slug?Slug=${encodeURIComponent(slug)}`;
      return this.httpClient.get<any>(url).pipe(
        map(res => this.mapToViewModel(res))
      );
    }

    private mapToViewModel(res: any) : Genere{
      return {
        id: res.id,
        name: res.name,
        description: res.description,
        slug: res.slug,
        imageUrl: res.avatarUrl,
        isActive: res.isActive,
        bookCount: res.coutBook,
      } as Genere;
   }
}
