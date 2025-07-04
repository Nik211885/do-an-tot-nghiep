export interface FavoriteBookViewModel{
  id: string,
  userId: string,
  favoriteBookId: string
  createDate: Date
}


export interface PaginationFavoriteBookViewModel{
  items: FavoriteBookViewModel[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
