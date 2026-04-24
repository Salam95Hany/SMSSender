import { Component, HostListener } from '@angular/core';
import { RouterOutlet } from "@angular/router";
import { AdminHeaderComponent } from "./main/admin-header/admin-header.component";
import { AdminSideMenuComponent } from "./main/admin-side-menu/admin-side-menu.component";
import { NgClass } from '@angular/common';

@Component({
  selector: 'app-admin-layout',
  imports: [RouterOutlet, AdminHeaderComponent, AdminSideMenuComponent,NgClass],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css'
})
export class AdminLayoutComponent {
  isSidebarCollapsed = false;
  isMobileMenuOpen = false;

  toggleSidebar(): void {
    if (typeof window !== 'undefined' && window.innerWidth < 1200) {
      this.isMobileMenuOpen = !this.isMobileMenuOpen;
      return;
    }

    this.isSidebarCollapsed = !this.isSidebarCollapsed;
  }

  closeMobileMenu(): void {
    this.isMobileMenuOpen = false;
  }

  @HostListener('window:resize')
  onResize(): void {
    if (typeof window !== 'undefined' && window.innerWidth >= 1200) {
      this.isMobileMenuOpen = false;
    }
  }
}
