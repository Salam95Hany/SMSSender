import { ChangeDetectionStrategy, Component, Input } from '@angular/core';
import { NgFor, NgIf } from '@angular/common';
import { RouterLink } from '@angular/router';

interface BreadcrumbItem {
  label: string;
  route?: string;
}

@Component({
  selector: 'app-admin-breadcrumb',
  standalone: true,
  imports: [NgFor, NgIf, RouterLink],
  templateUrl: './admin-breadcrumb.component.html',
  styleUrl: './admin-breadcrumb.component.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AdminBreadcrumbComponent {
  @Input() title = '';
  @Input() subtitle = '';
  @Input() icon = 'fas fa-layer-group';
  @Input() items: BreadcrumbItem[] = [];
}
