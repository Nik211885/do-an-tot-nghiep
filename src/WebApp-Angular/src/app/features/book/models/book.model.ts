import {Pagination} from '../../../shared/components/pagination/pagination.component';

export interface Book{
    id: string;
    title: string;
    rating: number;
    coutRating: number;
    author: string;
    isPayemnt?: boolean;
    genres: Genre[];
    avatarUrl: string;
    description: string;
    created?: Date;
    isFavorite: boolean;
    myRating?: number;
    lastModified?: Date;
    meanRatingStar: number;
    chapters?: Chapter[];
    coutComment: number;
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
    Free = 'Free',
    Paid = 'Paid',
    Subscription = 'Subscription',
}
export enum BookReleaseType
{
    Serialized = 1,
    Complete = 2
}

export interface Chapter{
    id: string;
    slug: string;
    version: number;
    title: string;
    content: string;
}


export type PaginationBook = Pagination<Book>;
