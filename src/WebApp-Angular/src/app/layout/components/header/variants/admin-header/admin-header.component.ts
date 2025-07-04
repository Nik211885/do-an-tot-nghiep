import { Component, OnInit, signal } from '@angular/core';
import { LayoutService } from '../../../../services/layout.service';
import { HeaderComponent } from '../../header.component';
import { CommonModule } from '@angular/common';
import { UserModel } from '../../../../../core/models/user.model';
import { AuthService } from '../../../../../core/auth/auth.service';
import {Router, RouterLink} from '@angular/router';

@Component({
  standalone: true,
  selector: 'app-admin-header',
  imports: [HeaderComponent, CommonModule, RouterLink],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.css'
})
export class AdminHeaderComponent implements OnInit {
  showNotifications = false;
  showUserDropdown = false;
  userModel = signal<UserModel | null>(null);

  notifications = [
    {
      id: 1,
      title: 'New user registered',
      description: 'A new user has registered to the platform',
      time: '5 minutes ago',
      read: false
    },
    {
      id: 2,
      title: 'System update',
      description: 'System will be updated today at 22:00',
      time: '1 hour ago',
      read: false
    },
    {
      id: 3,
      title: 'Database backup completed',
      description: 'Daily database backup has been completed successfully',
      time: 'Yesterday',
      read: true
    }
  ];

  constructor(private layoutService: LayoutService,
    private authService: AuthService,
              private router: Router) {}

  ngOnInit(): void {
    this.authService.initialize().subscribe((authenticated) => {
      if(authenticated){
        this.authService.loadUserProfile().subscribe((user) => {
          this.userModel.set(user);
        });
      }
    });
  }

  toggleSidebar(): void {
    this.layoutService.toggleSidebar();
  }

  toggleNotifications(): void {
    this.showNotifications = !this.showNotifications;
    if (this.showNotifications) {
      this.showUserDropdown = false;
    }
  }

  toggleUserDropdown(): void {
    this.showUserDropdown = !this.showUserDropdown;
    if (this.showUserDropdown) {
      this.showNotifications = false;
    }
  }
  markAsRead(notification: any): void {
    notification.read = true;
  }

  markAllAsRead(): void {
    this.notifications.forEach(notification => {
      notification.read = true;
    });
  }

  viewAllNotifications(): void {
    // Navigate to notifications page
    console.log('Viewing all notifications...');
    // You can add your navigation logic here
    // For example: this.router.navigate(['/admin/notifications']);
    this.showNotifications = false;
  }

  logout(): void {
    this.authService.logout();
  }

  // Close dropdowns when clicking outside
  closeDropdowns(): void {
    this.showNotifications = false;
    this.showUserDropdown = false;
  }
  hasUnreadNotifications() : boolean{
    return this.notifications.some(n => !n.read)
  }

}
