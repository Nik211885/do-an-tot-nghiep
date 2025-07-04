import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { GenreService } from '../../services/genre.service';
import { NgForm } from '@angular/forms';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import {CreateGenre} from '../../models/genres.model';
import {ImageUploadComponent} from '../../../../shared/components/image-upload/image-upload.component';

@Component({
  selector: 'app-create-genre',
  standalone: true,
  imports: [FormsModule, CommonModule, ImageUploadComponent],
  templateUrl: './create-genre.component.html',
  styleUrl: './create-genre.component.css'
})
export class CreateGenreComponent implements OnInit {
  genre: CreateGenre = {
    name: '',
    description: '',
    avatarUrl: ''
  };

  isEditMode = false;
  isSubmitting = false;
  isUploading = false;
  showSuccessMessage = false;
  showErrorMessage = false;
  genreId: string | null = null;

  constructor(
    private genreService: GenreService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.genreId = this.route.snapshot.paramMap.get('id');

    if (this.genreId) {
      this.isEditMode = true;
      this.loadGenreData(this.genreId);
    }
  }
  starUploadFile() {
    this.isUploading = true;
  }
  getImageUpload(fileInfo: any) {
    const url = fileInfo['secure_url'];

    if (url) {
      this.genre.avatarUrl = url;
    }
  }
  endUploadFile() {
    this.isUploading = false;
  }
  loadGenreData(id: string): void {
  }

  onSubmit(form: NgForm): void {
    if (form.valid && !this.isSubmitting) {
      this.isSubmitting = true;
      if (this.isEditMode && this.genreId) {
        // Update existing genre
        this.genreService.updateGenres(this.genreId, this.genre).subscribe({
          next: (success) => {
            this.isSubmitting = false;
            if (success) {
              this.showSuccess();
              // Stay on the same page for continued editing
            } else {
              this.showError();
            }
          },
          error: (error) => {
            this.isSubmitting = false;
            console.error('Error updating genre:', error);
            this.showError();
          }
        });
      } else {
        // Create new genre
        this.genreService.createGenre(this.genre).subscribe({
          next: (success) => {
            this.isSubmitting = false;
            if (success) {
              this.isEditMode = true;
              this.genreId = success.id;
              this.showSuccess();
            } else {
              this.showError();
            }
          },
          error: (error) => {
            this.isSubmitting = false;
            console.error('Error creating genre:', error);
            this.showError();
          }
        });
      }
    }
  }

  onCancel(): void {
    this.router.navigate(['resources/genres']);
  }

  showSuccess(): void {
    this.showSuccessMessage = true;
    this.showErrorMessage = false;

    // Hide success message after 3 seconds
    setTimeout(() => {
      this.showSuccessMessage = false;
    }, 3000);
  }

  showError(): void {
    this.showErrorMessage = true;
    this.showSuccessMessage = false;

    // Hide error message after 3 seconds
    setTimeout(() => {
      this.showErrorMessage = false;
    }, 3000);
  }
}
