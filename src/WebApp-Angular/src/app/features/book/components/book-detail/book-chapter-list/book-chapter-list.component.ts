import { CommonModule } from '@angular/common';
import {Component, EventEmitter, HostListener, Input, Output} from '@angular/core';
import { Chapter, BookPolicy } from '../../../models/book.model';

@Component({
  standalone: true,
  selector: 'app-book-chapter-list',
  imports: [CommonModule],
  templateUrl: './book-chapter-list.component.html',
  styleUrl: './book-chapter-list.component.css'
})
export class BookChapterListComponent {
  @Input() chapters: Chapter[] = [];
  @Input() bookSlug!: string;
  @Input() policy!: BookPolicy;
  @Output() chapterClicked = new EventEmitter<{
    chapterSlug: Chapter,
    bookSlug: string,
  }>();
  BookPolicy = BookPolicy;
  expandedChapters = false;
  displayLimit = 1;
  toggleExpand(): void {
    this.expandedChapters = !this.expandedChapters;
  }

  /**
   * Gets the chapters to display based on expanded state
   */
  get displayedChapters(): Chapter[] {
    return this.expandedChapters ? this.chapters : this.chapters.slice(0, this.displayLimit);
  }
  chapterClick(chapter: Chapter){
    this.chapterClicked.emit({
      chapterSlug: chapter,
      bookSlug: this.bookSlug,
    });
  }
}
