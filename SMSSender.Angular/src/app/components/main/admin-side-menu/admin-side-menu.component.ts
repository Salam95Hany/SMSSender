import { NgClass } from '@angular/common';
import { Component, EventEmitter, Output } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router';
import { NgbAccordionConfig, NgbAccordionModule } from "@ng-bootstrap/ng-bootstrap";

@Component({
  selector: 'app-admin-side-menu',
  standalone: true,
  imports: [NgClass,NgbAccordionModule,RouterLink,RouterLinkActive],
  templateUrl: './admin-side-menu.component.html',
  styleUrl: './admin-side-menu.component.css'
})
export class AdminSideMenuComponent {
@Output() toggleMenuEvent = new EventEmitter<boolean>();
  isMenuCollapse = false;
  isSmMenuCollapse = false;
  UserModel: any;

  constructor(config: NgbAccordionConfig) {
    // config.closeOthers = true;
    config.type = 'info';
  }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }

  onToggle() {
    this.isMenuCollapse = !this.isMenuCollapse;
    this.toggleMenuEvent.emit(this.isMenuCollapse);
  }

  onSmallToggle() {
    this.isSmMenuCollapse = !this.isSmMenuCollapse
  }
}
