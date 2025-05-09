export interface Book{
    id: string;
    title: string;
    rating: number;
    coutRating: number;
    author: string;
    genres: Genre[];
    avatarUrl: string;
    description: string;
    chapterNumber?: Array<number>;
    slug: string;
    tagNames: string[];
    isCompeleted: boolean;
    policyReadBook: PolicyReadBook;
    bookReleaseType: BookReleaseType;
}

export interface Genre{
    id: string;
    name: string;
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
