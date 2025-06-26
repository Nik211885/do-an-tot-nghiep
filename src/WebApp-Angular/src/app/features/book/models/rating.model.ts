import {Pagination} from '../../../shared/components/pagination/pagination.component';
import {Comment} from '../../../shared/components/comment/comment.component';

export interface RatingViewModel{
  id: string;
  bookReviewId: string;
  reviewerId: string;
  star: number;
  bookId: string;
  dateTimeSubmitted: Date;
  lastUpdated: Date;
}


export type PaginationComment = Pagination<Comment>;
