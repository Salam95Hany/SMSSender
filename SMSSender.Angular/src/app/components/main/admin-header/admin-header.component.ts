import { Component, Input } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../auth/auth.service';
import { NgClass } from '@angular/common';
import { NgbDropdownModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [NgClass,NgbDropdownModule,RouterLink],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.css'
})
export class AdminHeaderComponent {
  defaultImage = 'http://themes.iamabdus.com/dealsy/1.0/img/user/user-thumb.jpg'
  @Input() isMenuCollapse!: boolean;
  toggleMenu = false;
  UserName: any;
  CompanyName: any;
  UserModel: any;
  constructor(private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.UserModel = this.authService.UserModel;
    this.UserName = this.UserModel?.fullName;
    this.CompanyName = this.UserModel?.companyName;
  }

  onToggle() {

  }

  getImageEvent(imageSrc: any) {
  }

  LogOut() {
    localStorage.removeItem('UserModel');
    this.router.navigateByUrl('');
  }
}
