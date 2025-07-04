import {Component, OnInit} from '@angular/core';
import {GenreService} from '../../services/genre.service';
import {ActivatedRoute, Router} from '@angular/router';
import {GenreViewModel, CreateGenre} from '../../models/genres.model';
import {ToastService} from '../../../../shared/components/toast/toast.service';
import {CommonModule} from '@angular/common';
import {FormsModule} from '@angular/forms';
import {ImageUploadComponent} from '../../../../shared/components/image-upload/image-upload.component';

@Component({
  selector: 'app-detail-resources',
  imports: [CommonModule, FormsModule, ImageUploadComponent],
  standalone: true,
  templateUrl: './detail-resources.component.html',
  styleUrl: './detail-resources.component.css'
})
export class DetailResourcesComponent implements OnInit {
  genre: GenreViewModel | null = null;
  genreId!: string;
  isEditing = false;
  isLoading = false;
  isSaving = false;
  isUploading = false;

  // Form data for editing
  editForm: CreateGenre = {
    name: '',
    description: '',
    avatarUrl: ''
  };

  constructor(
    private genreService: GenreService,
    private toastService: ToastService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit(): void {
    const genreId = this.route.snapshot.params['id'];
    if (genreId) {
      this.genreId = genreId;
      this.loadGenre();
    } else {
      this.toastService.error("Có lỗi xảy ra");
    }
  }

  loadGenre(): void {
    this.isLoading = true;
    this.genreService.getGenreById(this.genreId)
      .subscribe({
        next: result => {
          if (result) {
            this.genre = result;
            this.initializeEditForm();
          } else {
            this.genre = null;
            this.toastService.error("Không tìm thấy thể loại");
          }
          this.isLoading = false;
        },
        error: err => {
          this.genre = null;
          this.isLoading = false;
          console.log(err);
          this.toastService.error("Có lỗi xảy ra khi tải dữ liệu");
        }
      });
  }

  initializeEditForm(): void {
    if (this.genre) {
      this.editForm = {
        name: this.genre.name,
        description: this.genre.description,
        avatarUrl: this.genre.avatarUrl
      };
    }
  }

  toggleEdit(): void {
    this.isEditing = !this.isEditing;
    console.log(this.isEditing);
    if (this.isEditing) {
      this.initializeEditForm();
    }
  }

  cancelEdit(): void {
    this.isEditing = false;
    this.initializeEditForm();
  }

  saveChanges(): void {
    if (!this.editForm.name.trim()) {
      this.toastService.error("Tên thể loại không được để trống");
      return;
    }

    this.isSaving = true;
    this.genreService.updateGenres(this.genreId, this.editForm)
      .subscribe({
        next: success => {
          if (success) {
            this.toastService.success("Cập nhật thành công");
            this.isEditing = false;
            this.loadGenre(); // Reload to get updated data
          } else {
            this.toastService.error("Cập nhật thất bại");
          }
          this.isSaving = false;
        },
        error: err => {
          console.log(err);
          this.toastService.error("Có lỗi xảy ra khi cập nhật");
          this.isSaving = false;
        }
      });
  }

  toggleActive(): void {
    this.genreService.changeActive(this.genreId)
      .subscribe({
        next: success => {
          if (success) {
            this.toastService.success("Đã thay đổi trạng thái");
            this.loadGenre(); // Reload to get updated data
          } else {
            this.toastService.error("Thay đổi trạng thái thất bại");
          }
        },
        error: err => {
          console.log(err);
          this.toastService.error("Có lỗi xảy ra khi thay đổi trạng thái");
        }
      });
  }

  goBack(): void {
    this.router.navigate(['resources/genres']);
  }
  starUploadFile() {
    this.isUploading = true;
  }
  getImageUpload(fileInfo: any) {
    const url = fileInfo['secure_url'];

    if (url) {
      this.editForm.avatarUrl = url;
    }
  }
  endUploadFile() {
    this.isUploading = false;
  }
}
