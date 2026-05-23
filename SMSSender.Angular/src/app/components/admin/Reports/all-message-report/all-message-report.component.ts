import { Component, OnInit } from '@angular/core';
import { NgxLoadingModule } from "ngx-loading";
import { AdminBreadcrumbComponent } from "../../../../shared/admin-breadcrumb/admin-breadcrumb.component";
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { ActivatedRoute } from '@angular/router';
import { AdminService } from '../../../../services/admin.service';
import { AdminFilterComponent } from "../../../../shared/admin-filter/admin-filter.component";
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule, NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-all-message-report',
  imports: [NgxLoadingModule, AdminBreadcrumbComponent, AdminFilterComponent, NgbModule, NgIf, NgFor, CommonModule],
  templateUrl: './all-message-report.component.html',
  styleUrl: './all-message-report.component.css'
})
export class AllMessageReportComponent implements OnInit {
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
    this.GetWalletsReportByOperationType();
  }

  GetWalletsReportSummaryByOperationType() {
    this.adminService.GetWalletsReportSummaryByOperationType(this.PagingFilter).subscribe((data) => {
      this.SummaryData = data.results[0];
      this.MessageList.forEach(i => {
        this.SummaryData.totalWalletBalance += i.finalBalance;
      });
    });
  }

  GetWalletsReportByOperationType() {
    this.ShowLoader = true;
    this.adminService.GetWalletsReportByOperationType(this.PagingFilter).subscribe((data) => {
      this.ShowLoader = false;
      this.MessageList = data.results;
      this.TotalCount = data.totalCount;
      this.GetWalletsReportSummaryByOperationType();
    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetWalletsReportByOperationType();
  }
}
