export interface CreateComment {
  bookId: string;
  content: string;
  parentCommentId?: string;
}
