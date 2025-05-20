import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, input, Input, OnInit, Output } from '@angular/core';
import { DomSanitizer } from '@angular/platform-browser';
import {ChapterVersion} from '../chapter-version/chapter-version.component';

interface ChapterDiff{
  id: string,
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
  chapterDiff!: ChapterDiff;
  @Output() closeDiffContentEvent = new EventEmitter<void>();
  @Output() chapterRenameEvent = new EventEmitter<ChapterVersion>();
  @Output() chapterRollBackEvent = new EventEmitter<string>();

  constructor(private sanitizer: DomSanitizer,
    private httpClient: HttpClient,
  ) {
   }
  ngOnInit(): void {
    console.log(this.chapterVersion);
    const chapterDiff : ChapterDiff = {
      id: this.chapterVersion.id,
      version:'1',
      dateCreateVersion: new Date(),
      diffContent: '<div>Ki an anh trang<del>bi kich</del> <ins>cau truyen</ins></div>',
      diffTitle: 'Ki an <del>dem khuya</del> <ins>Anh trang</ins>'
    }
    this.chapterDiff = {
      id: chapterDiff.id,
      version: chapterDiff.version,
      dateCreateVersion: chapterDiff.dateCreateVersion,
      diffContent: this.sanitizer.bypassSecurityTrustHtml(chapterDiff.diffContent) as string,
      diffTitle: this.sanitizer.bypassSecurityTrustHtml(chapterDiff.diffTitle) as string
    }
  }
  closeDiffContent(){
    this.closeDiffContentEvent.emit();
  }
  renameChapterVersion(){
    this.chapterRenameEvent.emit(this.chapterVersion);
  }
  rollBackToVersion(){
    this.chapterRollBackEvent.emit(this.chapterDiff.id);
  }
}
