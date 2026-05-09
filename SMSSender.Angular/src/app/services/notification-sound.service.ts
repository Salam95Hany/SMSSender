import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationSoundService {
  private audio: HTMLAudioElement;
  private isUnlocked = false;
  private pendingPlay = false;

  constructor() {
    this.audio = new Audio('/bell-172780.mp3');
    this.audio.load();

    document.addEventListener('click',() => {
        this.unlockAudio();
      },
      { once: true }
    );
  }

  unlockAudio() {
    if (this.isUnlocked) return;

    this.audio.play()
      .then(() => {
        this.audio.pause();
        this.audio.currentTime = 0;
        this.isUnlocked = true;

        if (this.pendingPlay) {
          this.pendingPlay = false;
          this.play();
        }
      })
      .catch(err => {
        console.log('Audio unlock failed:', err);
      });
  }

  play() {
    if (!this.isUnlocked) {
      this.pendingPlay = true;
      return;
    }

    this.audio.currentTime = 0;
    this.audio.play().catch(err => {
      console.log('Audio play failed:', err);
    });
  }
}
