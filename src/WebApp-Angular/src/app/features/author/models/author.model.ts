export interface Author {
  id: string;
  name: string;
  bio: string;
  imageUrl: string;
  followersCount: number;
  followingCount: number;
  isFollowing: boolean;
}