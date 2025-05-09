import { Component, Input } from '@angular/core';
import { convertUtcToLocal, formatRelativeTime } from "../../../core/utils/date.until";
@Component({
  selector: 'app-comment',
  imports: [],
  templateUrl: './comment.component.html',
  styleUrl: './comment.component.css'
})
export class CommentComponent {
  @Input() avatarUrl: string = "";
  @Input() userName: string = "";
  @Input() dateComment: Date | string = "";
  @Input() comment: string = "";
  convertDateComment(){
    return formatRelativeTime(convertUtcToLocal(this.dateComment));
  }

}
