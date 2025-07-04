import {Component, NgModule, OnInit} from '@angular/core';
import {FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators} from '@angular/forms';
import {UserProfileModel, UserProfileUpdateModel} from '../../models/user-profile.model';
import {UserProfileService} from '../../services/user-profile.service';
import {AuthService} from '../../../../../core/auth/auth.service';
import {CommonModule, Location} from '@angular/common';
import {Router} from '@angular/router';
import {ToastService} from '../../../../../shared/components/toast/toast.service';
import {DialogService} from '../../../../../shared/components/dialog/dialog.component.service';

@Component({
  selector: 'app-profile',
  imports: [
    ReactiveFormsModule,
    FormsModule,
    CommonModule,
  ],
  standalone: true,
  templateUrl: './profile.component.html',
  styleUrl: './profile.component.css'
})
export class ProfileComponent implements OnInit {
  profileForm!: FormGroup;
  isSubmitted = false;
  isLoading = false;

  constructor(private fb: FormBuilder,
              private authService: AuthService,
              private toastService: ToastService,
              private router: Router,
              private location: Location,
              private dialogService: DialogService,
              private userProfileService: UserProfileService) {
  }

  ngOnInit(): void {
    this.initForm();
    this.loadUserProfile();
  }

  private initForm(): void {
    this.profileForm = this.fb.group({
      // Read-only fields from auth service
      username: [{value: '', disabled: true}],
      email: [{value: '', disabled: true}],

      // Editable fields
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      bio: ['', [Validators.maxLength(500)]],
    });
  }

  private loadUserProfile(): void {
    this.isLoading = true;
    this.authService.initialize().subscribe({
      next: result => {
        if(result){
          this.authService.loadUserProfile().subscribe({
            next: (authProfile) => {
              this.profileForm.patchValue({
                username: authProfile.username,
                email: authProfile.email,
                firstName: authProfile.firstName,
                lastName: authProfile.lastName,
              });
              this.isLoading = false;
            },
            error: (error) => {
              console.error('Error loading auth profile:', error);
              this.isLoading = false;
            }
          });
          // Load user profile data
          this.userProfileService.getUserProfile().subscribe({
            next: (profile) => {
              if (profile) {
                this.profileForm.patchValue({
                  bio: profile.bio,
                });
              }
            },
            error: (error) => {
              console.error('Error loading user profile:', error);
            }
          });
        }
      },
      error: (error) => {
        console.error('Error loading user profile:', error);
      }
    })

    // Load auth user data (username, email, firstName, lastName)
  }

  getInputClasses(fieldName: string): string {
    const field = this.profileForm.get(fieldName);
    const baseClasses = 'w-full px-4 py-3 border rounded-lg transition-all duration-200';

    // Read-only fields styling
    if (field?.disabled) {
      return `${baseClasses} bg-gray-100 border-gray-300 text-gray-600 cursor-not-allowed focus:outline-none`;
    }

    // Regular field styling
    const validClasses = 'border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-blue-500';
    const invalidClasses = 'border-red-300 focus:ring-2 focus:ring-red-500 focus:border-red-500 bg-red-50';

    return `${baseClasses} ${this.isFieldInvalid(fieldName) ? invalidClasses : validClasses}`;
  }

  getTextareaClasses(fieldName: string): string {
    const baseClasses = 'w-full px-4 py-3 border rounded-lg transition-all duration-200 resize-none';
    const validClasses = 'border-gray-300 focus:ring-2 focus:ring-blue-500 focus:border-blue-500';
    const invalidClasses = 'border-red-300 focus:ring-2 focus:ring-red-500 focus:border-red-500 bg-red-50';

    return `${baseClasses} ${this.isFieldInvalid(fieldName) ? invalidClasses : validClasses}`;
  }

  isFieldInvalid(fieldName: string): boolean {
    const field = this.profileForm.get(fieldName);
    return !!(field && field.invalid && (field.dirty || field.touched || this.isSubmitted));
  }

  getErrorMessage(fieldName: string): string {
    const field = this.profileForm.get(fieldName);
    if (field?.errors) {
      if (field.errors['required']) {
        const fieldDisplayNames: { [key: string]: string } = {
          'firstName': 'Họ',
          'lastName': 'Tên',
          'bio': 'Giới thiệu bản thân'
        };
        return `${fieldDisplayNames[fieldName]} bắt buộc nhập`;
      }
    }
    return '';
  }

  onSubmit(): void {
    this.isSubmitted = true;

    if (this.profileForm.valid) {
      this.isLoading = true;

      const formData = {
        firstName: this.profileForm.get('firstName')?.value,
        lastName: this.profileForm.get('lastName')?.value,
        bio: this.profileForm.get('bio')?.value,
      };
      const userUpdate = {
        bio: this.profileForm.get('bio')?.value,
        firstName: this.profileForm.get('firstName')?.value,
        lastName: this.profileForm.get('lastName')?.value,
      } as UserProfileUpdateModel;
      this.userProfileService.updateUserProfile(userUpdate).subscribe({
        next: (authProfile) => {
          if(authProfile) {
            this.toastService.success("Cập nhật thông tin thành công")
          }
          else{
            this.toastService.error("Có lỗi trong quá trình cập nhật thông tin")
          }
        },
        error: (error) => {
          console.error(error);
          this.toastService.error("Có lỗi trong quá trình cập nhật thông tin")
        }
      })
      this.isLoading= false;
    }
  }

  onBack(): void {
    this.location.back();
  }
  async onResetPassword(){
    const resetPasswordResult = await this.dialogService
      .open('Đặt lại mật khẩu', 'Bạn có chắc chắn muốn đặt lại mật khẩu của mình');
    if(resetPasswordResult.isSuccess){
      await this.dialogService.open('Đặt lại mật khẩu thành công', `
                Chúng tôi sẻ gửi email đặt lại mật khẩu cho bạn,
                 hãy kiểm tra lại email của bạn. nếu không được hãy chọn lại
              `)
      this.userProfileService.resetPasswordByEmail()
        .subscribe({
          next:  async (authProfile) => {
            if(authProfile) {

            }
            else{
              this.toastService.error("Có lỗi trong quá trình đặt lại mật khẩu vui lòng thử lại sau");
            }
          },
          error: (error) => {
            console.error(error);
            this.toastService.error("Có lỗi trong quá trình đặt lại mật khẩu vui lòng thử lại sau");
          }
        })
    }
  }
}
