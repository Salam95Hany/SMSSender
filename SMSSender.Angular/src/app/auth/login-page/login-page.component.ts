import { Component, inject } from '@angular/core';
import { NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AuthService } from '../auth.service';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [NgIf, RouterLink],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent {
  private readonly authService = inject(AuthService);

  get userModel(): any {
    return this.authService.UserModel;
  }

  get hasSession(): boolean {
    return !!this.userModel;
  }

  get displayName(): string {
    return this.userModel?.fullName || this.userModel?.userName || 'فريق التشغيل';
  }
}
