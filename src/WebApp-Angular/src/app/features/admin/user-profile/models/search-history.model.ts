export interface SearchHistoryViewModel{
  id: string;
  userId: string;
  searchTerm: string;
  created: Date,
}

export interface PaginationSearchHistoryViewModel{
  items:  SearchHistoryViewModel[];
  pageNumber: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
}
