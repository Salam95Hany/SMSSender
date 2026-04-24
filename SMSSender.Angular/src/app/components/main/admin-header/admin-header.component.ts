import { Component, EventEmitter, Input, Output, inject } from '@angular/core';
import { NgClass, NgIf } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router, RouterLink } from '@angular/router';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';
import { filter } from 'rxjs';
import { AuthService } from '../../../auth/auth.service';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [NgClass, NgbDropdownModule, RouterLink],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.css'
})
export class AdminHeaderComponent {
  @Input() isSidebarCollapsed = false;
  @Input() isMobileMenuOpen = false;
  @Output() menuToggle = new EventEmitter<void>();

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);

  userModel: any = null;
  userName = 'مشرف النظام';
  companyName = 'SMS Sender';
  roleName = 'Administrator';
  pageTitle = 'واجهة الإدارة';

  ngOnInit(): void {
    this.refreshUser();
    this.syncRouteMeta();

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => {
        this.refreshUser();
        this.syncRouteMeta();
      });
  }

  get userInitials(): string {
    const source = this.userName || this.userModel?.userName || 'SM';
    const parts = source
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((part: string) => part[0]);

    return parts.join('').toUpperCase() || 'SM';
  }

  private refreshUser(): void {
    this.userModel = this.authService.UserModel;
    this.userName = this.userModel?.fullName || this.userModel?.userName || 'مشرف النظام';
    this.companyName = this.userModel?.companyName || 'SMS Sender';
    this.roleName = this.userModel?.role || 'Administrator';
  }

  private syncRouteMeta(): void {
    let currentRoute = this.route.firstChild;

    while (currentRoute?.firstChild) {
      currentRoute = currentRoute.firstChild;
    }
  }

  logOut(): void {
    localStorage.removeItem('UserModel');
    this.router.navigate(['/login']);
  }
}
