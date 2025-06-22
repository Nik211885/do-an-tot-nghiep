import {Injectable} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {catchError, map, Observable, of} from 'rxjs';
import {UserProfileModel, UserProfileUpdateModel} from '../models/user-profile.model';
import {AuthConfig} from '../../../../core/auth/auth.config';

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
}
