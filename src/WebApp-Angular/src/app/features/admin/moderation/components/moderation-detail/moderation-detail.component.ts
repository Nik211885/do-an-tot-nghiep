import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ModerationService } from '../../services/moderation.service';
import {ModerationViewModel, ModerationDecision, ApproveStatus} from '../../models/moderation.model';

@Component({
  selector: 'app-moderation-detail',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './moderation-detail.component.html',
  styleUrls: ['./moderation-detail.component.css']
})
export class ModerationDetailComponent implements OnInit {
  moderation!: ModerationViewModel;
  moderationId: string | null = null;
  error: string | null = null;

  // Decisions data
  decisions: ModerationDecision[] = [];
  loadingDecisions = false;
  currentDecisionPage = 1;
  decisionPageSize = 1;
  totalDecisionPages = 1;

  // Modal states
  showApprovalModal = false;
  showRejectionModal = false;
  approvalNote = '';
  rejectionNote = '';
  submitting = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private moderationService: ModerationService
  ) { }

  ngOnInit(): void {
    this.moderationId = this.route.snapshot.paramMap.get('id');
    this.loadModeration();
    this.loadDecisions();
  }

  loadModeration(): void {
    if (this.moderationId) {
      this.moderationService.getApprovalById(this.moderationId)
        .subscribe({
          next: (data) => {
            if (data) {
              this.moderation = data;
              this.moderationService.getBookApprovalByIds([this.moderation]);
              this.error = null;
            } else {
              this.error = 'Moderation not found';
            }
          },
          error: (err) => {
            console.error('Error loading moderation:', err);
            this.error = 'Failed to load moderation details. Please try again.';
          }
        });
    } else {
      this.error = 'Invalid moderation ID';
    }
  }

  loadDecisions(): void {
    if (!this.moderationId) return;

    this.loadingDecisions = true;
    this.moderationService.getDecisionChapter(this.moderationId, this.currentDecisionPage, this.decisionPageSize)
      .subscribe({
        next: (data) => {
          this.decisions = data.items || [];
          this.loadingDecisions = false;
          if (data && data.items.length === this.decisionPageSize) {
            this.totalDecisionPages = Math.max(this.currentDecisionPage + 1, this.totalDecisionPages);
          } else {
            this.totalDecisionPages = this.currentDecisionPage;
          }
        },
        error: (err) => {
          console.error('Error loading decisions:', err);
          this.loadingDecisions = false;
          this.decisions = [];
        }
      });
  }

  refreshDecisions(): void {
    this.currentDecisionPage = 1;
    this.loadDecisions();
  }

  previousDecisionPage(): void {
    if (this.currentDecisionPage > 1) {
      this.currentDecisionPage--;
      this.loadDecisions();
    }
  }

  nextDecisionPage(): void {
    if (this.currentDecisionPage < this.totalDecisionPages) {
      this.currentDecisionPage++;
      this.loadDecisions();
    }
  }

  // Modal controls
  openApprovalModal(): void {
    this.showApprovalModal = true;
    this.approvalNote = '';
  }

  closeApprovalModal(): void {
    this.showApprovalModal = false;
    this.approvalNote = '';
  }

  openRejectionModal(): void {
    this.showRejectionModal = true;
    this.rejectionNote = '';
  }

  closeRejectionModal(): void {
    this.showRejectionModal = false;
    this.rejectionNote = '';
  }

  // Actions
  approveChapter(): void {
    if (!this.moderationId) return;

    this.submitting = true;
    this.moderationService.approvalChapter(this.moderationId, this.approvalNote || 'Thông qua')
      .subscribe({
        next: () => {
          this.submitting = false;
          this.closeApprovalModal();
          this.loadModeration();
          this.refreshDecisions();
        },
        error: (err) => {
          this.submitting = false;
        }
      });
  }

  rejectChapter(): void {
    if (!this.moderationId || !this.rejectionNote?.trim()) return;

    this.submitting = true;
    this.moderationService.rejectChapter(this.moderationId, this.rejectionNote)
      .subscribe({
        next: () => {
          this.submitting = false;
          this.closeRejectionModal();
          this.loadModeration();
          this.refreshDecisions();
        },
        error: (err) => {
          this.submitting = false;
          console.error('Error rejecting chapter:', err);
        }
      });
  }

  protected readonly ApproveStatus = ApproveStatus;
}
