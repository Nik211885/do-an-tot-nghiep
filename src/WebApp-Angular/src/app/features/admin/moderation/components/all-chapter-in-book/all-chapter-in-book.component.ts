import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ModerationService} from '../../services/moderation.service';
import {ApproveStatus, ModerationViewModel} from '../../models/moderation.model';
import {DatePipe, NgClass, NgForOf, NgIf} from '@angular/common';
import {DomSanitizer} from '@angular/platform-browser';
import {CommentBookService} from '../../../../book/services/comment-book.service';

@Component({
  selector: 'app-all-chapter-in-book',
  imports: [
    DatePipe,
    NgClass,
    NgIf,
    NgForOf
  ],
  templateUrl: './all-chapter-in-book.component.html',
  styleUrl: './all-chapter-in-book.component.css'
})
export class AllChapterInBookComponent implements OnInit{
  bookId: string | null = null;
  bookApproval: ModerationViewModel | null = null;
  moderation: ModerationViewModel[] = [];
  constructor(private activatedRoute: ActivatedRoute,
              private sanitizer: DomSanitizer,
              private router: Router,
              private moderationService: ModerationService) {
  }
  ngOnInit(): void {
    this.bookId = this.activatedRoute.snapshot.paramMap.get('id');
    if(this.bookId){
      this.moderationService.getAllModerationForBookId(this.bookId).subscribe({
          next: data => {
            if(data){
              this.moderation = data;
            }
          }
        }
      )
      this.bookApproval = {
        id: "",
        bookId: "",
        chapterId: "",
        authorId: "",
        submittedAt: null,
        chapterContent: "",
        chapterTitle: "",
        chapterNumber: "",
        chapterSlug: "",
        bookTitle: "",
        status: ApproveStatus.Rejected,
        decision: [],
        copyrightChapter: null,
        isBookActive: true,
        bookApprovalId: this.bookId ?? ""
      };
      this.moderationService.getBookApprovalByIds([this.bookApproval]);
    }
  }
  getPendingCount(): number {
    return this.moderation.filter(item => item.status === 'Pending').length;
  }

  getApprovedCount(): number {
    return this.moderation.filter(item => item.status === 'Approved').length;
  }

  getRejectedCount(): number {
    return this.moderation.filter(item => item.status === 'Rejected').length;
  }
  getChapterContentPreview(content: string) {
    const previewContent = content.length > 500 ?
      content.substring(0, 500) + '...' :
      content;

    return this.sanitizer.bypassSecurityTrustHtml(previewContent);
  }

  viewDetail(chapter: ModerationViewModel) {
    console.log(chapter);
    this.router.navigate(["/moderation/detail", chapter.id]);
  }
}
