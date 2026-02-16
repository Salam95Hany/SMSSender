import { NgClass } from '@angular/common';
import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-admin-stats-card',
  standalone: true,
  imports: [NgClass],
  templateUrl: './admin-stats-card.component.html',
  styleUrl: './admin-stats-card.component.css'
})
export class AdminStatsCardComponent {
  @Input() statsInfo: { icon: string, number: number, text: string, status: string };
}
