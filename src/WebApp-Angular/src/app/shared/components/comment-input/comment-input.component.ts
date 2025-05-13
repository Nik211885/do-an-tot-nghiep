import { CommonModule } from '@angular/common';
import { Component, output, Output, EventEmitter, ElementRef, Input, ViewChild } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User } from '../comment/comment.component';

@Component({
  standalone: true,
  selector: 'app-comment-input',
  imports: [FormsModule, CommonModule],
  templateUrl: './comment-input.component.html',
  styleUrl: './comment-input.component.css'
})
export class CommentInputComponent {
  @Input() currentUser: User | null = null;
  @Input() submitLabel: string = 'Bình luận';
  @Input() placeholder: string = 'Thêm một bình luận...';
  @Input() showCancel: boolean = false;
  @Input() autoFocus: boolean = false;
  
  @Output() handleSubmit = new EventEmitter<string>();
  @Output() handleCancel = new EventEmitter<void>();
  
  @ViewChild('commentInput') commentInput!: ElementRef<HTMLTextAreaElement>;
  
  commentText: string = '';

  ngOnInit(): void {}

  ngAfterViewInit(): void {
    if (this.autoFocus) {
      setTimeout(() => {
        this.commentInput.nativeElement.focus();
      }, 100);
    }
  }

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
