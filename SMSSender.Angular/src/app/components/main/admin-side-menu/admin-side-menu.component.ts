import { NgClass, NgFor, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { ADMIN_NAVIGATION } from '../../../core/navigation/admin-navigation';

@Component({
  selector: 'app-admin-side-menu',
  standalone: true,
  imports: [NgClass, NgFor, NgIf, RouterLink, RouterLinkActive],
  templateUrl: './admin-side-menu.component.html',
  styleUrl: './admin-side-menu.component.css'
})
export class AdminSideMenuComponent {
  @Input() isCollapsed = false;
  @Input() isMobileOpen = false;
  @Output() collapseToggle = new EventEmitter<void>();
  @Output() closeMobile = new EventEmitter<void>();

  readonly navigation = ADMIN_NAVIGATION;
  userModel: any = null;

  ngOnInit(): void {
    this.userModel = JSON.parse(localStorage.getItem('UserModel') ?? 'null');
  }

  get userInitials(): string {
    const source = this.userModel?.fullName || this.userModel?.userName || 'SM';
    return source
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((part: string) => part[0])
      .join('')
      .toUpperCase();
  }

  onLinkClick(): void {
    if (typeof window !== 'undefined' && window.innerWidth < 1200) {
      this.closeMobile.emit();
    }
  }
}
