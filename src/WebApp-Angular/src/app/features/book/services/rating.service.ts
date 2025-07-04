import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { subscribe } from 'diagnostics_channel';
export interface BookRating {
  bookId: string;
  rating: number;
}
@Injectable({
  providedIn: 'root'
})  
export class RatingService {
  private apiUrl = 'api/ratings';
  
  constructor(private http: HttpClient) {}
  
  submitRating(productRating: BookRating) : Observable<any>{
    return new Observable((subscribe)=>{
        subscribe.next("Ok")
    })

  }
}