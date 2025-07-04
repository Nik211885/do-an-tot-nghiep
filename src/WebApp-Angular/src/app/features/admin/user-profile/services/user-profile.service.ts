import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';
import {UserProfileModel, UserProfileUpdateModel} from '../models/user-profile.model';
import {AuthConfig} from '../../../../core/auth/auth.config';
import {PaginationSearchHistoryViewModel, SearchHistoryViewModel} from '../models/search-history.model';
import {PaginationFavoriteBookViewModel} from '../models/favorite-book.model';

@Injectable({
  providedIn: 'root',
})
export class UserProfileService {
  private authConfig: AuthConfig = new AuthConfig();
  constructor(private httpClient: HttpClient) { }
  getUserProfile() : Observable<UserProfileModel | undefined>{
    const url = "/user-profile/my";
    return this.httpClient.get<UserProfileModel>(url);
  }
  updateUserProfile(userProfileUpdateModel : UserProfileUpdateModel){
    const url = "/user-profile/update";
    return this.httpClient.put<void>(url, userProfileUpdateModel, {
      observe: 'response',
    })
      .pipe(map(res=>res.status === 200));
  }
  resetPasswordByEmail() : Observable<boolean>{
    const url = `/user-profile/reset-password-by-email?clientId=${this.authConfig.clientId}&returnUri=${this.authConfig.publicUrl}`;
    return this.httpClient.put<void>(url, null, {observe: 'response'})
      .pipe(map(res=>res.status === 204));
  }
  getHistorySearch(pageNumber: number, pageSize: number): Observable<PaginationSearchHistoryViewModel>{
    const url = `/user-profile/search-history/pagination?pageNumber=${pageNumber}&pageSize=${pageSize}`;
    return this.httpClient.get<PaginationSearchHistoryViewModel>(url);
  }
  getFavoriteBook(pageNumber: number, pageSize: number): Observable<PaginationFavoriteBookViewModel> {
    const url = `/user-profile/book/my-favorite/pagination?PageNumber=${pageNumber}&PageSize=${pageSize}`;
    return this.httpClient.get<PaginationFavoriteBookViewModel>(url);
  }
  deleteSearchHistory(id: string) : Observable<boolean>{
    const url = "/user-profile/search-history/delete";
    const body = {
      searchHistoryIds: [id]
    }
    return this.httpClient.delete(url, {
      body,
      observe: 'response'
    }).pipe(map(res=>res.status === 204));
  }
  cleanSearchHistory() : Observable<boolean>{
    const url = "/user-profile/search-history/clean";
    return this.httpClient.delete(url, {
      observe: 'response'
    }).pipe(map(res=>res.status === 204));
  }
}
