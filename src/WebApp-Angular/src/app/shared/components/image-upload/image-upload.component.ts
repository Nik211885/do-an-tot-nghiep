import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ImageCropperComponent, ImageCroppedEvent, ImageTransform } from 'ngx-image-cropper';
import { ImageUploadService } from './image-upload.service';
import { ImageCropData, CropDimensions } from './image-upload.model';
import { UploadFileService } from '../../../core/services/upload-file.service';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css'],
  standalone: true,
  imports: [CommonModule, ImageCropperComponent]
})
export class ImageUploadComponent implements OnInit {
  @Input() imageUrl: string ="";
  @Input() titleForUploadFile: string = "";
  @Output() imageUpload = new EventEmitter<any>();
  @Output() starUpload = new EventEmitter<void>();
  @Output() endUpload = new EventEmitter<void>();

  imageChangedEvent: any = '';
  croppedImage: any = '';
  croppedFile: File | null = null;

  showCropper = false;
  originalFile: File | null = null;
  originalFileName: string = '';
  formData: FormData | null = null;

  // Cropper settings
  canvasRotation = 0;
  rotation = 0;
  scale = 1;
  transform: ImageTransform = {};
  maintainAspectRatio = true;
  aspectRatio = 1;
  cropDimensions: CropDimensions = { width: 0, height: 0 };

  // UI states
  isLoading = false;
  isDragging = false;
  errorMessage = '';

  constructor(
    private imageUploadService: ImageUploadService,
    private uploadFileService: UploadFileService
  ) {}

  ngOnInit(): void {
    this.croppedImage = this.imageUrl;
    if(this.croppedImage){
      this.showCropper = false;
      this.isLoading = false;
    }
    this.imageUploadService.formData$.subscribe(formData => {
      this.formData = formData;
    });
  }

  fileChangeEvent(event: any): void {
    this.errorMessage = '';

    if (event.target.files && event.target.files.length > 0) {
      const file = event.target.files[0];

      if (this.isValidImage(file)) {
        this.isLoading = true;
        this.imageChangedEvent = event;
        this.originalFile = file;
        this.originalFileName = file.name;

        setTimeout(() => {
          this.showCropper = true;
          this.isLoading = false;
        }, 300);
      }
    }
  }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = true;
  }

  onDragLeave(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
    this.isDragging = false;

    if (event.dataTransfer?.files && event.dataTransfer.files.length > 0) {
      const file = event.dataTransfer.files[0];

      if (this.isValidImage(file)) {
        this.isLoading = true;
        this.originalFile = file;
        this.originalFileName = file.name;

        const fileInput = {
          target: {
            files: event.dataTransfer.files
          }
        };

        this.imageChangedEvent = fileInput;

        setTimeout(() => {
          this.showCropper = true;
          this.isLoading = false;
        }, 300);
      }
    }
  }

  isValidImage(file: File): boolean {
    const validTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/webp'];
    if (!validTypes.includes(file.type)) {
      this.errorMessage = 'Please upload a valid image file (JPEG, PNG, GIF, WEBP)';
      return false;
    }

    const maxSize = 5 * 1024 * 1024;
    if (file.size > maxSize) {
      this.errorMessage = 'Image size should be less than 5MB';
      return false;
    }

    return true;
  }

  imageCropped(event: ImageCroppedEvent) {
    if (event.blob && this.originalFileName) {
      const file = new File([event.blob], this.originalFileName, { type: event.blob.type });
      this.croppedFile = file; // ✅ Lưu lại file để upload
      this.croppedImage = URL.createObjectURL(event.blob); // dùng để preview

      const cropData: ImageCropData = {
        file: file,
        filename: this.originalFileName,
        base64: this.croppedImage
      };
      this.imageUploadService.setCroppedImage(cropData);

      if (event.width && event.height) {
        this.cropDimensions = {
          width: event.width,
          height: event.height
        };
      }
    }
  }

  saveCroppedImage(): void {
    this.starUpload.emit();
    if (this.croppedFile) {
      this.uploadFileService.uploadFile(this.croppedFile).subscribe(res => {
        console.log(res)
        this.imageUpload.emit(res);
        this.endUpload.emit();
      });
    }
    this.showCropper = false;
  }

  resetImage(): void {
    this.imageChangedEvent = '';
    this.croppedImage = '';
    this.croppedFile = null;
    this.showCropper = false;
    this.originalFile = null;
    this.originalFileName = '';
    this.cropDimensions = { width: 0, height: 0 };
    this.errorMessage = '';
    this.canvasRotation = 0;
    this.rotation = 0;
    this.scale = 1;
    this.transform = {};

    this.imageUploadService.resetData();
  }

  uploadNewImage(): void {
    this.resetImage();
  }

  rotateLeft() {
    this.canvasRotation--;
    this.flipAfterRotate();
  }

  rotateRight() {
    this.canvasRotation++;
    this.flipAfterRotate();
  }

  private flipAfterRotate() {
    const flippedH = this.transform.flipH;
    const flippedV = this.transform.flipV;
    this.transform = {
      ...this.transform,
      flipH: flippedV,
      flipV: flippedH
    };
  }

  zoomOut() {
    this.scale -= 0.1;
    this.transform = {
      ...this.transform,
      scale: this.scale
    };
  }

  zoomIn() {
    this.scale += 0.1;
    this.transform = {
      ...this.transform,
      scale: this.scale
    };
  }

  toggleAspectRatio() {
    this.maintainAspectRatio = !this.maintainAspectRatio;
  }

  setAspectRatio(ratio: number) {
    this.aspectRatio = ratio;
    this.maintainAspectRatio = true;
  }

  getFormDataForUpload(): FormData | null {
    return this.imageUploadService.getFormData();
  }
}
