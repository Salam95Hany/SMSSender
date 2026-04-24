import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-admin-empty-state',
  standalone: true,
  imports: [],
  templateUrl: './admin-empty-state.component.html',
  styleUrl: './admin-empty-state.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEmptyStateComponent {
}
