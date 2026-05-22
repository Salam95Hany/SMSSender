import { CommonModule, NgClass, NgFor, NgIf } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModule, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
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
import { FormsModule } from '@angular/forms';
import { NgxLoadingModule } from 'ngx-loading';
import { FormService } from '../../../../services/form.service';
import { MessageBoxPopupComponent } from '../../../../shared/message-box-popup/message-box-popup.component';
import { NotificationSignalrService } from '../../../../services/notification-signalr.service';

@Component({
  selector: 'app-all-message',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, NgIf, NgClass, AdminFilterComponent, NgbModule, AdminBreadcrumbComponent, AdminEmptyStateComponent, ArabicDateWithTimePipe,
    CommonModule, FormsModule, NgxLoadingModule, MessageBoxPopupComponent
  ],
  templateUrl: './all-message.component.html',
  styleUrl: './all-message.component.css'
})
export class AllMessageComponent implements OnInit {
  MessageList: any[] = [];
  MessageBoxList: any[] = [];
  FilterList: FilterModel[] = [];
  MessageUpdate: any;
  Title = '';
  MessageDetails: any;
  MessageLog: any;
  TransactionId: any;
  ProviderName = '';
  ProviderPhone = '';
  MessageTransactionId: any;
  PagingFilter: PagingFilterModel = { pagesize: 20, currentpage: 1, operationType: 0, filterList: [] };
  TotalCount = 0;
  isFilter = true;
  ShowLoader = false;
  ReloadFilter = 0;

  constructor(private adminService: AdminService, private toaster: ToastrService, private route: ActivatedRoute, private offcanvasService: NgbOffcanvas,
    private formService: FormService, private modalService: NgbModal, private notificationSignalrService: NotificationSignalrService) { }

  ngOnInit(): void {
    const data = this.route.snapshot.data;
    this.PagingFilter.operationType = data['opreationType'];
    this.Title = data['title'];
    this.notificationSignalrService.onMessageCalculated((messageTransactionId: number) => {
      if (this.MessageList.some(m => m.messageTransactionId === messageTransactionId)) {
        let message = this.MessageList.find(m => m.messageTransactionId === messageTransactionId);
        if (message) {
          message.transactionStatus = 1;
        }
      }
    });
    this.GetSmsDataByOperationType();
    this.GetSmsFilterByOperationType();
  }

  OpenMessageBoxModal(content: any, item: any) {
    this.TransactionId = item.transactionId;
    this.GetMessageDetailsById(item);
    this.modalService.open(content, {
      centered: true,
      size: 'sm',
      windowClass: 'messages-modal',
    });
  }

  openSidePanel(content: any, item: any) {
    this.TransactionId = item.transactionId;
    this.MessageDetails = item;
    var obj = {
      messageTransactionId: item.messageTransactionId,
      transactionId: item.transactionId,
      operationType: item.operationType,
      amount: item.amount,
      fromPhone: item.fromPhone,
      senderName: item.senderName,
      balanceAfter: item.balanceAfter,
      commission: item.commission
    };
    this.MessageUpdate = obj;
    this.GetMessageDetailsById();
    this.offcanvasService.open(content, { position: 'end' });
  }

  BuildMessageBoxModel(item: any, details: any): any[] {
    let obj = {
      messageTransactionId: item.messageTransactionId,
      transactionId: item.transactionId,
      provider: item.provider,
      providerName: item.providerName,
      providerPhone: item.providerPhone,
      operationTypeName: item.operationTypeName,
      senderName: item.senderName,
      operationType: item.operationType,
      amount: item.amount,
      fromPhone: item.fromPhone,
      balanceAfter: item.balanceAfter,
      operationServerDateTime: item.operationServerDateTime,
      commission: item.commission,
      message: details.message,
      isCalculated: false,
    }
    let list = [obj];

    return list;
  }

  GetSmsDataByOperationType() {
    this.ShowLoader = true;
    this.adminService.GetSmsDataByOperationType(this.PagingFilter).subscribe((data) => {
      this.ShowLoader = false;
      this.MessageList = data.results;
      this.TotalCount = data.totalCount;
    });
  }

  GetSmsFilterByOperationType() {
    this.adminService.GetSmsFilterByOperationType(this.PagingFilter).subscribe((data) => {
      this.FilterList = data.results;
    });
  }

  GetMessageDetailsById(item = null) {
    this.adminService.GetMessageDetailsById(this.TransactionId).subscribe(data => {
      this.MessageLog = data.results;
      if (item) {
        this.MessageBoxList = this.BuildMessageBoxModel(item, this.MessageLog);
      }
    });
  }

  PageChanged(obj: any) {
    this.PagingFilter.currentpage = obj;
    this.GetSmsDataByOperationType();
  }

  FilterChecked(filterList: FilterModel[]) {
    this.PagingFilter.filterList = filterList;
    this.GetSmsDataByOperationType();
    this.GetSmsFilterByOperationType();
  }

  UpdateMessageTransaction() {
    if (!this.MessageUpdate.amount || this.MessageUpdate.amount <= 0) {
      this.toaster.warning('برجاء ادخال المبلغ');
      return
    }
    if (!this.MessageUpdate.fromPhone) {
      this.toaster.warning('برجاء ادخال رقم تلفون المرسل');
      return
    }
    if (!this.MessageUpdate.balanceAfter || this.MessageUpdate.balanceAfter <= 0) {
      this.toaster.warning('برجاء ادخال رصيد المحفظة');
      return
    }
    if (!this.MessageUpdate.commission || this.MessageUpdate.commission <= 0) {
      this.toaster.warning('برجاء ادخال العمولة');
      return
    }

    this.adminService.UpdateTransactionMessage(this.MessageUpdate).subscribe(data => {
      if (data.isSuccess) {
        this.toaster.success(data.message);
        this.GetSmsDataByOperationType();
        this.offcanvasService.dismiss();
      } else
        this.toaster.error(data.message);
    });
  }

  CorrectionProcess() {
    let obj = {
      messageTransactionId: this.MessageUpdate.messageTransactionId,
      transactionId: this.TransactionId,
      deviceName: this.MessageLog.providerName,
      phoneNumber: this.MessageLog.providerPhone,
      message: this.MessageLog.message,
      providerStr: this.MessageLog.provider,
      sentStamp: this.MessageLog.sentStamp,
      receivedStamp: this.MessageLog.receivedStamp,
      sim: this.MessageLog.sim
    }

    this.adminService.CorrectionProcess(obj).subscribe(data => {
      if (data) {
        this.toaster.success('تم الاصلاح بنجاح');
        this.GetSmsDataByOperationType();
        this.offcanvasService.dismiss();
      } else
        this.toaster.error('لقد حدث خطا')
    });
  }

  OnCalculated() {
    this.GetSmsDataByOperationType();
    this.GetSmsFilterByOperationType();
    this.modalService.dismissAll();
  }

  NumbersOnly(key: any) {
    return this.formService.NumbersOnly(key);
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
