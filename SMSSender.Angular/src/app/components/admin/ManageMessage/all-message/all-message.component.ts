import { CommonModule, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { ActivatedRoute } from '@angular/router';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';

@Component({
  selector: 'app-all-message',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, NgIf, NgClass, AdminFilterComponent, NgbModule, AdminBreadcrumbComponent, AdminEmptyStateComponent,ArabicDateWithTimePipe,
    CommonModule
  ],
  templateUrl: './all-message.component.html',
  styleUrl: './all-message.component.css'
})
export class AllMessageComponent implements OnInit {
  MessageList: any[] = [];
  FilterList: FilterModel[] = [];
  Title = '';
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };
  TotalCount = 0;
  isFilter = true;

  constructor(private adminService: AdminService, private toaster: ToastrService, private route: ActivatedRoute) { }

  ngOnInit(): void {
    debugger;
    const data = this.route.snapshot.data;
    this.PagingFilter.operationType = data['opreationType'];
    this.Title = data['title'];
    this.GetSmsDataByOperationType();
    this.GetSmsFilterByOperationType();
  }

  GetSmsDataByOperationType() {
    this.adminService.GetSmsDataByOperationType(this.PagingFilter).subscribe((data) => {
      this.MessageList = data.results;
      this.TotalCount = data.totalCount;
    });
  }

  GetSmsFilterByOperationType() {
    this.adminService.GetSmsFilterByOperationType(this.PagingFilter).subscribe((data) => {
      this.FilterList = data.results;
    });
  }

  PageChanged(obj: any) {
    this.PagingFilter.currentpage = obj.page;
    this.GetSmsDataByOperationType();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetSmsDataByOperationType();
    this.GetSmsFilterByOperationType();
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
}
