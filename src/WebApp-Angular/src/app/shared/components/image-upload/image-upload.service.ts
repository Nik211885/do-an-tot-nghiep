import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ImageCropData } from './image-upload.model';

@Injectable({
  providedIn: 'root'
})
export class ImageUploadService {
  private formDataSubject = new BehaviorSubject<FormData | null>(null);
  public formData$: Observable<FormData | null> = this.formDataSubject.asObservable();

  setCroppedImage(data: ImageCropData): void {
    if (data.file) {
      const formData = new FormData();
      formData.append('image', data.file, data.filename);
      this.formDataSubject.next(formData);
    } else {
      this.formDataSubject.next(null);
    }
  }

  getFormData(): FormData | null {
    return this.formDataSubject.value;
  }

  resetData(): void {
    this.formDataSubject.next(null);
  }
}
