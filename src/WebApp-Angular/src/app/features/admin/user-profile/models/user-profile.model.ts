export interface UserProfileModel{
  id: string,
  bio: string,
  countFollowing: number,
  countFollower: number,
  countFavoriteBook: number,
}

export interface UserProfileUpdateModel{
  bio: string,
  firstName: string,
  lastName: string,
}
