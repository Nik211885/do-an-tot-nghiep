import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
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
  @Input() policy!: BookPolicy;
  BookPolicy = BookPolicy;
  expandedChapters = false;
  displayLimit = 5;
  toggleExpand(): void {
    this.expandedChapters = !this.expandedChapters;
  }
  
  /**
   * Gets the chapters to display based on expanded state
   */
  get displayedChapters(): Chapter[] {
    return this.expandedChapters ? this.chapters : this.chapters.slice(0, this.displayLimit);
  }
}
