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
  imports: [NgFor, NgClass, AdminBreadcrumbComponent,CommonModule,ArabicDateWithTimePipe],
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
      console.log(this.WalletList);
    });
  }

}
