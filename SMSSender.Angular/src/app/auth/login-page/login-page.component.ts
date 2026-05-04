import { Component, OnInit } from '@angular/core';
import { CommonModule, NgIf } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../auth.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';

@Component({
  selector: 'app-login-page',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule,NgIf],
  templateUrl: './login-page.component.html',
  styleUrl: './login-page.component.css'
})
export class LoginPageComponent implements OnInit {
  loginForm: FormGroup;
  showPassword = false;
  showAlert = false;
  alertMessage = '';
  isLoading = false;
  returnUrl = '/';

  constructor(private fb: FormBuilder, private authService: AuthService, private router: Router, private route: ActivatedRoute) {
    this.loginForm = this.fb.group({
      userName: ['', [Validators.required]],
      password: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    this.returnUrl = this.authService.resolveReturnUrl(this.route.snapshot.queryParamMap.get('returnUrl'));
  }

  togglePassword(): void {
    this.showPassword = !this.showPassword;
  }

  Login() {
    if (this.loginForm.invalid) {
      this.showAlert = true;
      this.alertMessage = 'يرجى إدخال البريد الإلكتروني وكلمة المرور بشكل صحيح';
      return;
    }

    this.isLoading = true;
    this.authService.AdminLogin(this.loginForm.value).subscribe(data => {
      this.isLoading = false;
      if (data.isSuccess) {
        localStorage.setItem('UserModel', JSON.stringify(data.results));
        this.router.navigateByUrl(this.returnUrl);
      } else {
        this.showAlert = true;
        this.alertMessage = data.message;
      }
    });
  }
}
