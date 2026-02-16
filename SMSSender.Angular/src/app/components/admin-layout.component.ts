import { Component } from '@angular/core';
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
isMenuCollapse: boolean;
}
