export interface CropDimensions {
  width: number;
  height: number;
}

export interface ImageCropData {
  file: File | null;
  filename: string;
  base64?: string;
}
