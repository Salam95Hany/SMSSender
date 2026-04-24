import { NgFor, NgIf } from '@angular/common';
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

@Component({
  selector: 'app-wallet-operations',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, NgIf, AdminFilterComponent, NgbModule, AdminBreadcrumbComponent, AdminEmptyStateComponent],
  templateUrl: './wallet-operations.component.html',
  styleUrl: './wallet-operations.component.css'
})
export class WalletOperationsComponent implements OnInit {
  OrdersList: any[] = [];
  FilterList: FilterModel[] = [];
  UserModel: any;
  PagingFilter: PagingFilterModel = { pagesize: 10, currentpage: 1, filterList: [] };
  TotalCount = 0;
  isFilter = true;

  constructor(private adminService: AdminService, private toaster: ToastrService) {}

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') ?? 'null');
  }

  pageChanged(page: number): void {}
}
