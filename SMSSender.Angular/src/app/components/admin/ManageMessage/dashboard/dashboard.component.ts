import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { AdminService } from '../../../../services/admin.service';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { AdminStatsCardComponent } from '../../../../shared/admin-stats-card/admin-stats-card.component';
import { MessageBoxPopupComponent } from '../../../../shared/message-box-popup/message-box-popup.component';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    NgFor,
    NgIf,
    NgbModule,
    AdminStatsCardComponent,
    MessageBoxPopupComponent,
    AdminBreadcrumbComponent,
    AdminEmptyStateComponent,
    ArabicDateWithTimePipe,
    CommonModule
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  MessageList: any[] = [];
  statsInfo = [
    { icon: 'fas fa-envelope', number: 0, text: 'إجمالي الرسائل', status: 'blue', key: 'totalCount' },
    { icon: 'fas fa-cash-register', number: 0, text: 'عمليات الإيداع', status: 'green', key: 'depositCount' },
    { icon: 'fas fa-money-bill-wave', number: 0, text: 'عمليات السحب', status: 'orange', key: 'withdrawCount' },
    { icon: 'fas fa-exchange-alt', number: 0, text: 'عمليات السيولة', status: 'red', key: 'cashWithdrawalCount' },
  ];

  constructor(
    private adminService: AdminService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.GetOperationCountDashboardSummary();
    this.GetTodayLatestTransactions();
  }

  GetOperationCountDashboardSummary() {
    this.adminService.GetOperationCountDashboardSummary().subscribe((data) => {
      this.statsInfo.forEach(stat => {
        stat.number = data.results[0][stat.key] || 0;
      })
    });
  }

  GetTodayLatestTransactions() {
    this.adminService.GetTodayLatestTransactions().subscribe((data) => {
      this.MessageList = data.results || [];
    });
  }

   getOperationClass(type: number): string {
    switch (type) {
      case 1: return 'badge-success';
      case 2: return 'badge-brown';
      case 3: return 'badge-gold';
      case 4: return 'badge-purple';
      case 5: return 'badge-secondary';
      default: return 'badge-light';
    }
  }

  openMessageBoxModal(content: unknown): void {
    this.modalService.open(content, {
      centered: true,
      size: 'xl',
      windowClass: 'messages-modal',
    });
  }
}
