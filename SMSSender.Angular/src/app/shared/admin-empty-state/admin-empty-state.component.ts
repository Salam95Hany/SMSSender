import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';
import { NgIf } from '@angular/common';

@Component({
  selector: 'app-admin-empty-state',
  standalone: true,
  imports: [NgIf],
  templateUrl: './admin-empty-state.component.html',
  styleUrl: './admin-empty-state.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminEmptyStateComponent {
  @Input() icon = 'fas fa-inbox';
  @Input() title = 'لا توجد بيانات للعرض';
  @Input() description = 'ستظهر النتائج هنا فور توفر عناصر مطابقة للبحث أو للصفحة الحالية.';
  @Input() actionLabel = '';
  @Output() actionTriggered = new EventEmitter<void>();
}
