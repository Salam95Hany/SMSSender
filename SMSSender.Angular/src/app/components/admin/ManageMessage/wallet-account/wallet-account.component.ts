import { CommonModule, NgFor } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';
import { AdminService } from '../../../../services/admin.service';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { FilterModel } from '../../../../models/FilterModel';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';
import { FormService } from '../../../../services/form.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { RoleCheckerDirective } from '../../../../directives/role-checker.directive';

@Component({
  selector: 'app-wallet-account',
  standalone: true,
  imports: [NgFor, AdminBreadcrumbComponent, CommonModule, ArabicDateWithTimePipe, ReactiveFormsModule,
    RoleCheckerDirective
  ],
  templateUrl: './wallet-account.component.html',
  styleUrl: './wallet-account.component.css'
})
export class WalletAccountComponent implements OnInit {
  WalletList: any[] = [];
  FilterList: FilterModel[] = [];
  ItemForm: FormGroup;
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };

  constructor(private adminService: AdminService, private formService: FormService, private fb: FormBuilder, private modalService: NgbModal, private toaster: ToastrService) {

  }

  ngOnInit(): void {
    this.FormInit();
    this.GetWalletAccountSummary();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      walletDetailId: 0,
      amount: [null, [Validators.required, Validators.min(0.0001)]]
    });
  }

  OpenUpdateInstaWalletAmountModal(content: any, item: any) {
    this.ItemForm.reset();
    this.ItemForm.get('walletDetailId').setValue(item.walletDetailId);
    this.ItemForm.get('amount').setValue(item?.availableBalance ?? 0);
    this.modalService.open(content, {
      size: 'sm',
      scrollable: true,
      centered: true
    });
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

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
  }

  UpdateInstaWalletAmount() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.ItemForm.valid;
    if (!isValid) {
      return;
    }

    this.adminService.UpdateInstaWalletAmount(this.ItemForm.get('walletDetailId').value, this.ItemForm.get('amount').value).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetWalletAccountSummary();
        this.modalService.dismissAll();
      } else {
        this.toaster.error(data.message);
      }
    });
  }
}
