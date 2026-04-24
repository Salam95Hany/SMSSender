import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { PaginationModule } from 'ngx-bootstrap/pagination';

@Component({
  selector: 'app-admin-pagination',
  standalone: true,
  imports: [CommonModule, FormsModule, PaginationModule],
  templateUrl: './admin-pagination.component.html',
  styleUrl: './admin-pagination.component.css'
})
export class AdminPaginationComponent {
  @Input() currentPage = 1;
  @Input() pageSize = 10;
  @Input() totalCount = 0;
  @Output() pageChanged = new EventEmitter<number>();

  readonly maxSize = 3;
  showingStr = '0-0';

  ngOnInit(): void {
    this.resetShowingStr();
  }

  ngOnChanges(): void {
    this.resetShowingStr();
  }

  private resetShowingStr(): void {
    if (!this.totalCount || !this.pageSize) {
      this.showingStr = '0-0';
      return;
    }

    const safeCurrentPage = this.currentPage || 1;
    const start = ((safeCurrentPage - 1) * this.pageSize) + 1;
    const end = Math.min(safeCurrentPage * this.pageSize, this.totalCount);

    this.showingStr = `${start}-${end}`;
  }

  pageChangeEvent(event: { page: number }): void {
    this.currentPage = event.page;
    this.pageChanged.emit(event.page);
    this.resetShowingStr();
  }
}
