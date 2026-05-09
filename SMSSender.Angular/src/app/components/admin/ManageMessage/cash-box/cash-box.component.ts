import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { FormService } from '../../../../services/form.service';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';
import { NgxLoadingModule } from 'ngx-loading';

@Component({
  selector: 'app-cash-box',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, NgIf, AdminFilterComponent, NgbModule, AdminBreadcrumbComponent, AdminEmptyStateComponent, ReactiveFormsModule,
    ArabicDateWithTimePipe, CommonModule,NgxLoadingModule
  ],
  templateUrl: './cash-box.component.html',
  styleUrl: './cash-box.component.css'
})
export class CashBoxComponent implements OnInit {
  CashList: any[] = [];
  FilterList: FilterModel[] = [];
  ItemForm: FormGroup;
  UserModel: any;
  PagingFilter: PagingFilterModel = { pagesize: 10, currentpage: 1, filterList: [] };
  TotalCount = 0;
  CashBoxId: any;
  isFilter = true;
  ShowLoader = false;

  constructor(private adminService: AdminService, private toaster: ToastrService, private modalService: NgbModal, private fb: FormBuilder,
    private formService: FormService
  ) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') ?? 'null');
    this.FormInit();
    this.GetCashBoxData();
    this.GetCashBoxFilters();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      transactionType: ['Deposit', Validators.required],
      transactionAmount: [null, [Validators.required, Validators.min(0.0001)]],
      insertUser: null
    });
  }

  ResetForm() {
    this.ItemForm.reset();
    this.ItemForm.get('transactionType').setValue('Deposit');
    this.ItemForm.get('insertUser').setValue(this.UserModel?.userId);
  }

  openAddItemModal(content: any) {
    this.ResetForm();
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, cashBoxId: any) {
    this.CashBoxId = cashBoxId;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  GetCashBoxData() {
    this.ShowLoader = true;
    this.adminService.GetCashBoxData(this.PagingFilter).subscribe(data => {
      this.ShowLoader = false;
      this.CashList = data.results;
      this.TotalCount = data.totalCount;
    });
  }

  GetCashBoxFilters() {
    this.adminService.GetCashBoxFilters(this.PagingFilter).subscribe(data => {
      this.FilterList = data.results;
    });
  }

  PageChanged(obj: any) {
    this.PagingFilter.currentpage = obj;
    this.GetCashBoxData();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetCashBoxData();
    this.GetCashBoxFilters();
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    if (!isValid) {
      return;
    }

    this.adminService.AddNewCashBox(this.ItemForm.value).subscribe(data => {
      if (data.isSuccess) {
        this.GetCashBoxData();
        this.GetCashBoxFilters();
        this.toaster.success(data.message);
        this.modalService.dismissAll();
      } else
        this.toaster.error(data.message);
    });
  }

  DeleteItem() {
    this.adminService.DeleteCashBox(this.CashBoxId,this.UserModel?.userId).subscribe(data => {
      if (data.isSuccess) {
        this.GetCashBoxData();
        this.GetCashBoxFilters();
        this.toaster.success(data.message);
        this.modalService.dismissAll();
      } else
        this.toaster.error(data.message);
    })
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
