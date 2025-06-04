import {Genre} from './book.model';

export interface CreateBookCommand{
  title: string,
  avatarUrl?: string,
  description?: string,
  readerBookPolicy: BookPolicy,
  readerBookPolicyPrice?: number,
  bookReleaseType: BookReleaseType,
  tagsName?: string[],
  genreIds: string[],
}

export interface UpdateBookCommand extends CreateBookCommand{
  visibility: boolean;
}

export enum BookPolicy{
  Free = 1,
  Paid =2,
  Subscription = 3,
}

export enum BookReleaseType
{
  Serialized = 1,
  Complete = 2
}

export interface BookResponse{
  id: string;
  title: string;
  avatarUrl: string;
  description: string;
  createDateTimeOffset: Date;
  lastUpdateDateTime: Date;
  isComplete: boolean;
  slug: string;
  policyReadBook: {
    price?: number;
    policy: BookPolicy;
  };
  bookReleaseType: BookReleaseType;
  tags: string[];
  genres: Genre[]
}
