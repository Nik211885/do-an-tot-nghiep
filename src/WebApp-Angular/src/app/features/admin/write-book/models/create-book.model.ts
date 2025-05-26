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
