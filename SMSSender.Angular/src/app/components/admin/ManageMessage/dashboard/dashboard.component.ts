import { NgFor, NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { NgbModal, NgbModule, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { AdminStatsCardComponent } from '../../../../shared/admin-stats-card/admin-stats-card.component';
import { MessageBoxPopupComponent } from '../../../../shared/message-box-popup/message-box-popup.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    AdminPaginationComponent,
    NgFor,
    NgIf,
    AdminFilterComponent,
    NgbModule,
    AdminStatsCardComponent,
    MessageBoxPopupComponent,
    AdminBreadcrumbComponent,
    AdminEmptyStateComponent,
  ],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  OrdersList: any[] = [];
  FilterList: FilterModel[] = [
    {
      categoryDisplayName: 'بحث بالاسم',
      categoryName: 'AdmissionSearchText',
      filterType: 'SearchText',
      filterItems: [{ categoryName: 'AdmissionSearchText', itemId: '', itemKey: '', itemValue: '', filterItems: [] }]
    },
    {
      categoryDisplayName: 'فترة الوصول',
      categoryName: 'AdmissionDate',
      filterType: 'DateRange',
      filterItems: [{ categoryName: 'AdmissionDate', itemId: '', itemKey: '', itemValue: '', filterItems: [] }]
    },
    {
      categoryDisplayName: 'المحافظة',
      categoryName: 'Governorate',
      filterType: 'Checkbox',
      filterItems: [
        { categoryName: 'Governorate', itemId: 'Alexandria', itemKey: 'Alexandria', itemValue: '1', filterItems: [] },
        { categoryName: 'Governorate', itemId: 'Cairo', itemKey: 'Cairo', itemValue: '6', filterItems: [] },
        { categoryName: 'Governorate', itemId: 'Fayoum', itemKey: 'Fayoum', itemValue: '2', filterItems: [] },
      ]
    }
  ];
  UserModel: any;
  filterList: FilterModel[] = [];
  PagingFilter: PagingFilterModel = {
    pagesize: 10,
    currentpage: 1,
    filterList: []
  };
  TotalCount = 0;
  isFilter = false;

  statsInfo = [
    { icon: 'fas fa-envelope', number: 26, text: 'إجمالي الرسائل', status: 'blue' },
    { icon: 'fas fa-cash-register', number: 12, text: 'عمليات الإيداع', status: 'green' },
    { icon: 'fas fa-arrow-right-arrow-left', number: 124, text: 'عمليات التحويل', status: 'orange' },
    { icon: 'fas fa-money-bill-wave', number: 13, text: 'عمليات السحب', status: 'red' },
  ];

  constructor(
    private adminService: AdminService,
    private offcanvasService: NgbOffcanvas,
    private toaster: ToastrService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') ?? 'null');
  }

  openMessageBoxModal(content: unknown): void {
    this.modalService.open(content, {
      centered: true,
      size: 'xl',
      windowClass: 'messages-modal',
    });
  }

  pageChanged(page: number): void {}
}
