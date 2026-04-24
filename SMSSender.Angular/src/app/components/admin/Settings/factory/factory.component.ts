import { CommonModule, NgIf } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';

@Component({
  selector: 'app-factory',
  standalone: true,
  imports: [CommonModule, NgIf, FormsModule, RouterLink, AdminBreadcrumbComponent],
  templateUrl: './factory.component.html',
  styleUrl: './factory.component.css'
})
export class FactoryComponent implements OnDestroy {
  screen: 'initial' | 'countdown' | 'progress' | 'success' = 'initial';
  isConfirmed = false;
  countdown = 3;
  countdownInterval?: ReturnType<typeof setInterval>;
  progress = 0;
  statusText = 'جارٍ تحضير العملية...';
  progressInterval?: ReturnType<typeof setInterval>;
  steps = [
    { percent: 30, text: 'جارٍ حذف الملفات والمستندات...' },
    { percent: 60, text: 'جارٍ مسح قواعد البيانات...' },
    { percent: 75, text: 'جارٍ إعادة تعيين الإعدادات...' },
    { percent: 100, text: 'جارٍ إنهاء العملية...' }
  ];

  startCountdown(): void {
    this.screen = 'countdown';
    this.countdown = 3;
    this.clearIntervals();

    this.countdownInterval = setInterval(() => {
      this.countdown--;

      if (this.countdown === 0) {
        this.clearIntervals();
        this.startReset();
      }
    }, 1000);
  }

  cancelCountdown(): void {
    this.clearIntervals();
    this.screen = 'initial';
  }

  startReset(): void {
    this.screen = 'progress';
    let stepIndex = 0;

    this.progressInterval = setInterval(() => {
      if (stepIndex < this.steps.length) {
        this.progress = this.steps[stepIndex].percent;
        this.statusText = this.steps[stepIndex].text;
        stepIndex++;
        return;
      }

      this.clearIntervals();
      this.showSuccess();
    }, 1500);
  }

  showSuccess(): void {
    this.screen = 'success';
  }

  goBack(): void {
    window.history.back();
  }

  ngOnDestroy(): void {
    this.clearIntervals();
  }

  private clearIntervals(): void {
    if (this.countdownInterval) {
      clearInterval(this.countdownInterval);
    }

    if (this.progressInterval) {
      clearInterval(this.progressInterval);
    }
  }
}
