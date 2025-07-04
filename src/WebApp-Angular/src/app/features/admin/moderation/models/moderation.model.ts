import {Pagination} from '../../../../shared/components/pagination/pagination.component';

export interface ModerationViewModel {
  id: string;
  bookId: string;
  chapterId: string;
  authorId: string;
  submittedAt: Date;
  chapterContent: string;
  chapterTitle: string;
  chapterNumber: string;
  chapterSlug: string;
  bookTitle: string;
  status: ApproveStatus;
  decisionViewModels: ModerationDecision[];
  copyrightChapterViewModel: CopyrightChapterViewModel;
}

export interface ModerationDecision{
  moderatorId: string;
  decisionDateTime: string;
  note: string;
  status: ApproveStatus;
}

export interface CopyrightChapterViewModel {
  bookTitle: string;
  chapterTitle: string;
  chapterSlug: string;
  chapterNumber: number;
  chapterContent: string;
  dateTimeCopyright: Date;
}

export interface DigitalSignatureViewModel {
  signatureValue: string;
  signatureAlgorithm: string;
  signingDateTime: Date;
}

export enum ApproveStatus
{
  Pending = 'Pending',
  Rejected = 'Rejected',
  Approved = 'Approved',
}


export type PaginationModeration = Pagination<ModerationViewModel>;

export type PaginationDecision = Pagination<ModerationDecision>

export type PaginationModerationForBookGroup = Pagination<ModerationForBookGroup>

export interface ModerationForBookGroup{
  bookId: string;
  bookTitle: string;
  authorId: string;
  status: ApproveStatus;
  authorName: string;
}
