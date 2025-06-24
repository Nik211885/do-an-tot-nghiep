export interface BookReviewModel{
  id: string;
  bookId: string;
  viewCount: number;
  commentCount: number;
  ratingCount: number;
  ratingStar: number;
  createdAt: Date;
  updatedAt: Date;
}
