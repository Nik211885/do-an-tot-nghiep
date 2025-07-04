import {Pagination} from '../../../shared/components/pagination/pagination.component';

export interface GenreViewModel{
  id: string,
  name: string,
  description: string,
  slug: string,
  avatarUrl: string,
  isActive: boolean,
  coutBook: boolean,
  createdDateTime: Date,
  lastUpdatedDateTime: Date,
}


export interface CreateGenre{
  name: string,
  description: string,
  avatarUrl: string,
}

export type PaginationGenreViewModel = Pagination<GenreViewModel>;
