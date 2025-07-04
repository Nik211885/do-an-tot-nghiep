import {Component, OnInit, Sanitizer} from '@angular/core';
import {ActivatedRoute, Router} from '@angular/router';
import {ModerationService} from '../../services/moderation.service';
import {ModerationViewModel} from '../../models/moderation.model';
import {DatePipe, NgClass, NgForOf, NgIf, SlicePipe} from '@angular/common';
import {DomSanitizer, SafeHtml} from '@angular/platform-browser';

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
              console.log(this.moderation);
              console.log(this.moderation.length);
            }
          }
        }
      )
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
