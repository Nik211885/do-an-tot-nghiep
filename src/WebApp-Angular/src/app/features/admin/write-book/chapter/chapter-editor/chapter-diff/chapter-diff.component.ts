import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, input, Input, OnInit, Output } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import {} from '../chapter-version/chapter-version.component';
import {ChapterVersion} from '../../../models/book.model';
import {ChapterVersionService} from '../../../services/chapter.service';

export interface ChapterDiff{
  chapterId: string,
  chapterVersionId: string,
  version: string,
  dateCreateVersion: Date,
  diffContent: string,
  diffTitle: string,
}

@Component({
  standalone: true,
  selector: 'app-chapter-diff',
  imports: [],
  templateUrl: './chapter-diff.component.html',
  styleUrl: './chapter-diff.component.css'
})
export class ChapterDiffComponent implements OnInit {
  @Input() chapterVersion!: ChapterVersion;
  @Input() chapterId!: string;
  chapterDiff!: ChapterDiff;
  @Output() closeDiffContentEvent = new EventEmitter<void>();
  @Output() chapterRenameEvent = new EventEmitter<ChapterVersion>();
  @Output() chapterRollBackEvent = new EventEmitter<[string, string]>();

  constructor(private sanitizer: DomSanitizer,
              private chapterVersionService: ChapterVersionService
  ) {
   }
  ngOnInit(): void {
    this.chapterVersionService.previewChapterVersion(this.chapterVersion.id, this.chapterId)
      .subscribe(chapterVersion => {
        this.chapterDiff = chapterVersion;
      })
  }
  closeDiffContent(){
    this.closeDiffContentEvent.emit();
  }
  renameChapterVersion(){
    this.chapterRenameEvent.emit(this.chapterVersion);
  }
  rollBackToVersion(){
    this.chapterRollBackEvent.emit([this.chapterDiff.chapterVersionId, this.chapterId]);
  }
}
