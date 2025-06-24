import {Pagination} from '../../../shared/components/pagination/pagination.component';

export interface Book{
    id: string;
    title: string;
    rating: number;
    coutRating: number;
    author: string;
    genres: Genre[];
    avatarUrl: string;
    description: string;
    created?: Date;
    lastModified?: Date;
    chapters?: Chapter[];
    slug: string;
    tagNames: string[];
    isCompeleted: boolean;
    policyReadBook: PolicyReadBook;
    bookReleaseType: BookReleaseType;
    authorId?: string;
}

export interface Genre{
    id: string;
    name: string;
    slug: string;
}

export interface PolicyReadBook{
    price?: number,
    bookPolicy: BookPolicy,
}

export enum BookPolicy{
    Free = 1,
    Paid = 2,
    Subscription = 3,
}
export enum BookReleaseType
{
    Serialized = 1,
    Complete = 2
}

export interface Chapter{
    id: string;
    version: number;
    title: string;
}


export type PaginationBook = Pagination<Book>;
