import { CommonModule, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterLink } from "@angular/router";

@Component({
  selector: 'app-factory',
  standalone: true,
  imports: [NgIf, FormsModule, RouterLink],
  templateUrl: './factory.component.html',
  styleUrl: './factory.component.css'
})
export class FactoryComponent {
  screen: 'initial' | 'countdown' | 'progress' | 'success' = 'initial';
  isConfirmed = false;
  countdown = 3;
  countdownInterval?: any;
  progress = 0;
  statusText = 'جاري تحضير العملية...';
  progressInterval?: any;
  steps = [
    { percent: 30, text: 'جاري حذف الملفات والمستندات...' },
    { percent: 60, text: 'جاري مسح قواعد البيانات...' },
    { percent: 75, text: 'جاري إعادة تعيين الإعدادات...' },
    { percent: 100, text: 'جاري إنهاء العملية...' }
  ];

  startCountdown() {
    this.screen = 'countdown';
    this.countdown = 3;
    this.countdownInterval = setInterval(() => {
      this.countdown--;
      if (this.countdown === 0) {
        clearInterval(this.countdownInterval);
        this.startReset();
      }
    }, 1000);
  }

  cancelCountdown() {
    clearInterval(this.countdownInterval);
    this.screen = 'initial';
  }

  startReset() {
    this.screen = 'progress';
    let stepIndex = 0;
    this.progressInterval = setInterval(() => {
      if (stepIndex < this.steps.length) {
        this.progress = this.steps[stepIndex].percent;
        this.statusText = this.steps[stepIndex].text;
        stepIndex++;
      } else {
        clearInterval(this.progressInterval);
        this.showSuccess();
      }
    }, 1500);
  }

  showSuccess() {
    this.screen = 'success';
  }

  restartSystem() {
    location.reload();
  }

  goBack() {
    window.history.back();
  }
}
