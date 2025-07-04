export interface Bookv1 {
  id?: string;
  title: string;
  description: string;
  coverImage?: string;
  isPaid: boolean;
  price?: number;
  requiresRegistration: boolean;
  slug: string;
  isCompleted: boolean;
  tags: string[];
  genres: Genre[];
  createdAt?: Date;
  updatedAt?: Date;
}

export interface PaginationBook{
  items: Bookv1[],
  pageNumber: number,
  totalPages: number,
  totalCount: number,
  hasPreviousPage: boolean,
  hasNextPage: boolean,
}


export interface ChapterVersion{
  id: string,
  name: string,
  dateCreateVersion: Date,
  createVersion: string,
}

export interface Chapter {
  id?: string;
  bookId: string;
  title: string;
  content: string;
  chapterNumber: number;
  slug: string;
  createdAt?: Date;
  chapterVersion: ChapterVersion[];
  updatedAt?: Date;
}

export interface Genre{
  id: string;
  name: string;
  description: string;
}

export const AVAILABLE_GENRES = [
  'Giả tưởng',
  'Khoa học viễn tưởng',
  'Bí ẩn',
  'Phim kinh dị',
  'Lãng mạn',
  'Kinh dị',
  'Tiểu thuyết lịch sử',
  'Thanh thiếu niên',
  'Trẻ em',
  'Tiểu thuyết văn học',
  'Tiểu sử',
  'Tự lực',
  'Kinh doanh',
  'Du lịch',
  'Nấu ăn',
  'Nghệ thuật',
  'Thơ ca',
  'Kịch',
  'Truyện tranh',
  'Hồi ký',
  'Phi hư cấu'
];
