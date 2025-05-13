export interface Book {
  id?: string;
  title: string;
  description: string;
  coverImage?: string;
  isPaid: boolean;
  price?: number;
  requiresRegistration: boolean;
  isCompleted: boolean;
  tags: string[];
  genres: string[];
  createdAt?: Date;
  updatedAt?: Date;
}

export interface Chapter {
  id?: string;
  bookId: string;
  title: string;
  content: string;
  chapterNumber: number;
  createdAt?: Date;
  updatedAt?: Date;
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