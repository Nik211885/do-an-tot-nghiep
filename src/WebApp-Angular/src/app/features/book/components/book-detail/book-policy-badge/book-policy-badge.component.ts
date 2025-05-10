import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { BookPolicy } from '../../../models/book.model';

@Component({
  standalone: true,
  selector: 'app-book-policy-badge',
  imports: [CommonModule],
  templateUrl: './book-policy-badge.component.html',
  styleUrl: './book-policy-badge.component.css'
})
export class BookPolicyBadgeComponent {
  @Input() policy!: BookPolicy;
  @Input() price?: number;
  
  BookPolicy = BookPolicy;

  /**
   * Returns the appropriate background color class based on policy type
   */
  getBadgeColor(): string {
    switch (this.policy) {
      case BookPolicy.Free:
        return 'bg-emerald-500 shadow-emerald-500/30';
      case BookPolicy.Paid:
        return 'bg-amber-500 shadow-amber-500/30';
      case BookPolicy.Subscription:
        return 'bg-indigo-500 shadow-indigo-500/30';
      default:
        return 'bg-emerald-500 shadow-emerald-500/30';
    }
  }

  /**
   * Returns the display text for the policy type
   */
  getPolicyLabel(): string {
    switch (this.policy) {
      case BookPolicy.Free:
        return 'Miễn Phí';
      case BookPolicy.Paid:
        return 'Trả Phí';
      case BookPolicy.Subscription:
        return 'Đăng Ký';
      default:
        return 'Miễn Phí';
    }
  }
}
