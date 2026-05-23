import { Component } from '@angular/core';
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { ActivatedRoute } from '@angular/router';
import { NgxLoadingModule } from 'ngx-loading';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';
import { DateRangePickerComponent } from "../../../../shared/date-range-picker/date-range-picker.component";
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../../../auth/auth.service';

@Component({
  selector: 'app-wallet-profit-report',
  imports: [NgxLoadingModule, AdminBreadcrumbComponent, AdminFilterComponent, NgbModule, NgIf, NgFor, CommonModule, ArabicDateWithTimePipe, DateRangePickerComponent],
  templateUrl: './wallet-profit-report.component.html',
  styleUrl: './wallet-profit-report.component.css'
})
export class WalletProfitReportComponent {
  MessageList: any[] = [];
  ClosedPeriods: any[] = [];
  SummaryData: any;
  ProfitObj: any;
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
  selectedRange: { from: Date, to: Date } | null = null;
  Title = '';
  Description = '';
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };
  TotalCount = 0;
  isFilter = true;
  ShowLoader = false;

  constructor(private route: ActivatedRoute, private adminService: AdminService, private modalService: NgbModal, private toaster: ToastrService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    const data = this.route.snapshot.data;
    this.PagingFilter.operationType = data['opreationType'];
    this.Title = data['title'];
    this.Description = data['description'];
    this.GetWalletProfitReport();
    this.GetProfitPeriodClosings();
  }

  OpenDatePickerModal(content: any) {
    this.selectedRange = null;
    this.ProfitObj = null;
    this.modalService.open(content, {
      size: 'lg',
      centered: true,
      windowClass: 'messages-modal-sm',
    });
  }

  GetProfitPeriodClosings() {
    this.adminService.GetProfitPeriodClosings().subscribe((data) => {
      this.ClosedPeriods = data.results;
    });
  }

  GetProfitClosingByDate() {
    this.adminService.GetProfitClosingByDate(this.formatDate(this.selectedRange.from), this.formatDate(this.selectedRange.to)).subscribe((data) => {
      this.ProfitObj = data.results;
    });
  }

  GetWalletProfitReportSummary() {
    this.adminService.GetWalletProfitReportSummary(this.PagingFilter).subscribe((data) => {
      this.SummaryData = data.results[0];
      this.MessageList.forEach(i => {
        this.SummaryData.totalWalletBalance += i.finalBalance;
      });
    });
  }

  GetWalletProfitReport() {
    this.ShowLoader = true;
    this.adminService.GetWalletProfitReport(this.PagingFilter).subscribe((data) => {
      this.ShowLoader = false;
      this.MessageList = data.results;
      this.TotalCount = data.totalCount;
      this.GetWalletProfitReportSummary();
    });
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetWalletProfitReport();
  }

  ProfitPeriodClosings() {
    if (!this.selectedRange || !this.selectedRange.from || !this.selectedRange.to) {
      this.toaster.error('يرجى اختيار فترة صحيحة');
      return;
    }

    if (this.ProfitObj?.totalProfit == 0 || this.ProfitObj?.netProfit == 0) {
      this.toaster.error('لا يمكن قفل الفترة لأن الربح يساوي صفر');
      return;
    }

    const Model = {
      fromDate: this.formatDate(this.selectedRange.from),
      toDate: this.formatDate(this.selectedRange.to),
      totalProfit: this.ProfitObj?.totalProfit,
      netProfit: this.ProfitObj?.netProfit,
      closedBy: this.authService.userId
    };

    this.ShowLoader = true;
    this.adminService.ProfitPeriodClosings(Model).subscribe((data) => {
      this.ShowLoader = false;
      if (data.isSuccess) {
        this.toaster.success('تم قفل الفترة بنجاح');
        this.modalService.dismissAll();
        window.location.reload();
      } else {
        this.toaster.error('حدث خطأ أثناء قفل الفترة');
      }
    });
  }

  onRangeSelected(range: any) {
    this.selectedRange = range;
    this.GetProfitClosingByDate();
  }

  formatDate(date: Date): string {
    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0');
    const day = String(date.getDate()).padStart(2, '0');

    return `${year}-${month}-${day}`;
  }
}
