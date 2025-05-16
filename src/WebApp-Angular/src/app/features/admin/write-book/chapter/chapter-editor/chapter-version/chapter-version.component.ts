import { CommonModule } from '@angular/common';
import { Component, ElementRef, EventEmitter, HostListener, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import {formatVietnameseDate} from "../../../../../../core/utils/date.until"
import { DialogService, InputDialogOptions } from '../../../../../../shared/components/dialog/dialog.component.service';

export interface ChapterVersion{
  id: string,
  name: string,
  dateCreateVersion: Date,
  createVersion: string,
}

@Component({
  standalone:true,
  selector: 'app-chapter-version',
  imports: [CommonModule],
  templateUrl: './chapter-version.component.html',
  styleUrl: './chapter-version.component.css'
})
export class ChapterVersionComponent {
    constructor(private dialogService: DialogService){}
    @Input() chapterVersion!: ChapterVersion;
    @Input() currentVersion:boolean = false;
    isDropdownOpen: boolean = false;
    getFormatDateTime() : string {
      return formatVietnameseDate(this.chapterVersion.dateCreateVersion);
    }
    async renameChapterVersion(){
      const options: InputDialogOptions = {
        title: 'Đặt tên phiên bản chương',
        inputs:[
          {
            name: 'name',
            label: 'Tên phiên bản',
            type: 'text',
            value: this.chapterVersion.name,
            required: true,
            validators: [
              {
                type: 'required',
                message: 'Tên phiên bản không được để trống'
              },
            ]
          }
        ]
      }
      const renameChapterVersionDialog = await this.dialogService.openInputDialog(options);
      if(renameChapterVersionDialog.isSuccess){
        console.log(renameChapterVersionDialog.data);
      }
    }
    rollBackToVersion(){

    }
    @ViewChild('dropdownRef') dropdownRef!: ElementRef;
    @HostListener('document:click', ['$event'])
    onDocumentClick(event: MouseEvent) {
    const clickedInside = this.dropdownRef?.nativeElement.contains(event.target);
    if (!clickedInside) {
      this.isDropdownOpen = false;
    }
  }
}
