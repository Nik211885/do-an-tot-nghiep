import { CommonModule } from '@angular/common';
import { Component, output, Output, EventEmitter, ElementRef, Input, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import {UserModel} from '../../../core/models/user.model';

@Component({
  standalone: true,
  selector: 'app-comment-input',
  imports: [FormsModule, CommonModule],
  templateUrl: './comment-input.component.html',
  styleUrl: './comment-input.component.css'
})
export class CommentInputComponent {
  @Input() currentUser: UserModel | undefined = undefined;
  @Input() submitLabel: string = 'Bình luận';
  @Input() placeholder: string = 'Thêm một bình luận...';
  @Input() showCancel: boolean = false;
  @Input() autoFocus: boolean = false;

  @Output() handleSubmit = new EventEmitter<string>();
  @Output() handleCancel = new EventEmitter<void>();

  @ViewChild('commentInput') commentInput!: ElementRef<HTMLTextAreaElement>;

  commentText: string = '';

  submitComment(): void {
    if (this.commentText.trim()) {
      this.handleSubmit.emit(this.commentText);
      this.commentText = '';
    }
  }

  cancelReply(): void {
    this.commentText = '';
    this.handleCancel.emit();
  }
}
