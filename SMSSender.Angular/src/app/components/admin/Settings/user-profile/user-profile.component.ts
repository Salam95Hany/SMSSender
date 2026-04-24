import { NgIf } from '@angular/common';
import { Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../../auth/auth.service';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { AdminGeneralInputComponent } from '../../../../shared/admin-general-input/admin-general-input.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    NgIf,
    RouterLink,
    AdminBreadcrumbComponent,
    AdminEmptyStateComponent,
    AdminGeneralInputComponent,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {
  private readonly authService = inject(AuthService);

  get userModel(): any {
    return this.authService.UserModel;
  }

  get initials(): string {
    const source = this.userModel?.fullName || this.userModel?.userName || 'SM';
    return source
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((part: string) => part[0])
      .join('')
      .toUpperCase();
  }

  get sessionState(): string {
    return this.authService.isAuthenticated() ? 'الجلسة نشطة' : 'لا توجد جلسة مصادقة';
  }
}
