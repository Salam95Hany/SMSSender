import { CommonModule, NgClass, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminService } from '../../../../services/admin.service';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { FilterModel } from '../../../../models/FilterModel';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';

@Component({
  selector: 'app-wallet-account',
  standalone: true,
  imports: [NgFor, AdminBreadcrumbComponent, CommonModule, ArabicDateWithTimePipe],
  templateUrl: './wallet-account.component.html',
  styleUrl: './wallet-account.component.css'
})
export class WalletAccountComponent implements OnInit {
  WalletList: any[] = [];
  FilterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };

  constructor(private adminService: AdminService) {

  }

  ngOnInit(): void {
    this.GetWalletAccountSummary();
  }

  GetWalletAccountSummary() {
    this.adminService.GetWalletAccountSummary(this.PagingFilter).subscribe(data => {
      this.WalletList = data.results;
      this.WalletList.forEach(i => {
        if (i.dailyDepositUsagePercent > 100)
          i.dailyDepositUsagePercent = 100;
        if (i.dailyWithdrawalUsagePercent > 100)
          i.dailyWithdrawalUsagePercent = 100;
        if (i.monthlyDepositUsagePercent > 100)
          i.monthlyDepositUsagePercent = 100;
        if (i.monthlyWithdrawalUsagePercent > 100)
          i.monthlyWithdrawalUsagePercent = 100;
      })
    });
  }
}
