import { HttpClient, HttpEvent, HttpHeaders, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import {EMPTY, Observable, switchMap} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UploadFileService {
  private urlUploadFile = "/extend-service/upload-file-by-singature"; // endpoint backend tạo URL upload Cloudinary có ký

  constructor(private http: HttpClient) {}

  // Lấy URL upload có signature từ backend
  private getUploadFile(): Observable<string> {
    return this.http.get<string>(`${this.urlUploadFile}`);
  }

  uploadFile(file: File): Observable<any> {
    console.log(file);
    if(!file){
      return EMPTY;
    }
    return this.getUploadFile().pipe(
      switchMap(uploadUrl => {
        const formData = new FormData();
        formData.append("file", file);

        return this.http.post<any>(uploadUrl, formData);
      })
    );
  }
}
