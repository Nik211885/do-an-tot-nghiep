import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, HostListener, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import {formatVietnameseDate} from "../../../../../../core/utils/date.until"
import { DialogService, InputDialogOptions } from '../../../../../../shared/components/dialog/dialog.component.service';
import {ChapterVersion} from '../../../models/book.model';

@Component({
  standalone:true,
  selector: 'app-chapter-version',
  imports: [CommonModule],
  templateUrl: './chapter-version.component.html',
  styleUrl: './chapter-version.component.css'
})
export class ChapterVersionComponent {
  constructor(private dialogService: DialogService){}
  @Output() chapterVersionClickedEvent = new EventEmitter<ChapterVersion>();
  @Output() renameChapterVersionEvent = new EventEmitter<ChapterVersion>();
  @Output() rollBackChapterVersionEvent = new EventEmitter<[string, string]>();
  @Input() chapterVersion!: ChapterVersion;
  @Input() chapterId!: string;
  @Input() currentVersion:boolean = false;
  isDropdownOpen: boolean = false;
  dropDownToggle(event: MouseEvent): void{
    this.isDropdownOpen = !this.isDropdownOpen;
    event.stopPropagation();
  }

  getFormatDateTime() : string {
    return formatVietnameseDate(this.chapterVersion.dateCreateVersion);
  }
  async renameChapterVersion(event: MouseEvent){
    event.stopPropagation();
    this.renameChapterVersionEvent.emit(this.chapterVersion);
  }
  rollBackToVersion(event: MouseEvent){
    event.stopPropagation();
    this.rollBackChapterVersionEvent.emit([this.chapterVersion.id, this.chapterId])
  }
  @ViewChild('dropdownRef') dropdownRef!: ElementRef;
  @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent) {
    const clickedInside = this.dropdownRef?.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.isDropdownOpen = false;
    }
  }
  @HostListener('click',['$event'])
  onHostClick(event: MouseEvent){
    /// you can use library diff and rollback and comapre with new version
    // if i want to watch this verison i will compare this version with with back version
    // but i'm in top version i want to back content with this version and get back one version
    // and comapre and display for ui
    this.chapterVersionClickedEvent.emit(this.chapterVersion);
  }
}
