import { Component } from '@angular/core';
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { ActivatedRoute } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';

@Component({
  selector: 'app-wallet-profit-report',
  imports: [NgxLoadingModule, AdminBreadcrumbComponent, AdminFilterComponent, NgbModule, NgIf, NgFor, CommonModule,ArabicDateWithTimePipe],
  templateUrl: './wallet-profit-report.component.html',
  styleUrl: './wallet-profit-report.component.css'
})
export class WalletProfitReportComponent {
MessageList: any[] = [];
  SummaryData: any;
  FilterList: FilterModel[] = [
    {
      categoryDisplayName: 'باسم ,رقم المحفظة',
      categoryName: 'SearchText',
      filterType: 'SearchText',
    },
    {
      categoryDisplayName: 'تاريخ',
      categoryName: 'DateRange',
      filterType: 'DateRange',
    }
  ];
  Title = '';
  Description = '';
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };
  TotalCount = 0;
  isFilter = true;
  ShowLoader = false;

  constructor(private route: ActivatedRoute, private adminService: AdminService) { }

  ngOnInit(): void {
    const data = this.route.snapshot.data;
    this.PagingFilter.operationType = data['opreationType'];
    this.Title = data['title'];
    this.Description = data['description'];
    this.GetWalletProfitReport();
    this.GetWalletProfitReportSummary();
  }

  GetWalletProfitReportSummary() {
    this.adminService.GetWalletProfitReportSummary(this.PagingFilter).subscribe((data) => {
      this.SummaryData = data.results[0];
    });
  }

  GetWalletProfitReport() {
    this.ShowLoader = true;
    this.adminService.GetWalletProfitReport(this.PagingFilter).subscribe((data) => {
      this.ShowLoader = false;
      this.MessageList = data.results;
      this.TotalCount = data.totalCount;
    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetWalletProfitReport();
    this.GetWalletProfitReportSummary();
  }
}
