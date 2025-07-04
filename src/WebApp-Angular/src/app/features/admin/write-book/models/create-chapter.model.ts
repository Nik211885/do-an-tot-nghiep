export interface CreateChapterCommand{
  bookId: string,
  title: string,
  content: string,
  chapterNumber: number,
}

export interface UpdateChapterCommand{
  title: string,
  content: string,
  chapterNumber: number,
}
