import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class  ModerationService{
  constructor(private http: HttpClient) {}
}
