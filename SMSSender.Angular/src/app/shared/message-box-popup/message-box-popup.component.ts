import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, EventEmitter, Input, OnChanges, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TimeAgoTodayPipe } from '../../pipes/time-ago-today.pipe';
import { AdminService } from '../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from '../../auth/auth.service';
import { NgxLoadingModule } from 'ngx-loading';

@Component({
  selector: 'app-message-box-popup',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeAgoTodayPipe, NgIf, NgFor, NgxLoadingModule],
  templateUrl: './message-box-popup.component.html',
  styleUrl: './message-box-popup.component.css'
})
export class MessageBoxPopupComponent implements OnInit, OnChanges {
  @Input() MessageList: any[] = [];
  @Output() OnCalculated = new EventEmitter<any>();
  searchTerm = '';
  ShowLoader = false;

  constructor(private modalService: NgbModal, private adminService: AdminService, private toaster: ToastrService, private authService: AuthService) { }

  ngOnInit(): void {
   
  }

  ngOnChanges(): void {
    this.MessageList.forEach(i => {
      if (i.senderName)
        i.firstLetter = i.senderName?.trim().charAt(0);
      else
        i.firstLetter = '-';
      i.originalCustomerReceive = i.amount;
      i.commissionUpdated = i.commission;
      i.customerReceive = i.amount;
      if (i.operationType == 1)
        i.customerReceive = i.amount + i.commission;
    });
  }

  CalculateCustomerReceive(item: any, isSelected: boolean) {
    if (item.operationType == 2 || item.operationType == 3) {
      if (isSelected)
        item.customerReceive = item.amount - item.commissionUpdated;
      else
        item.customerReceive = item.amount;
    }
  }

  CalculateCamission(item: any) {
    const originalAmount = Number(item.originalCustomerReceive);
    const commission = Number(item.commissionUpdated || 0);
    if (item.operationType == 1) {
      item.customerReceive = originalAmount + commission;
    } else if (item.operationType == 2 || item.operationType == 3) {
      if (item.isChecked)
        item.customerReceive = originalAmount - commission;
    }


  }

  dismissModal(): void {
    this.modalService.dismissAll();
  }

  AddNewCashBox(item: any) {
    let model = {
      MessageTransactionId: item.messageTransactionId,
      TransactionType: item.operationType,
      TransactionAmount: item.customerReceive,
      InsertUser: this.authService.userId,
      ProviderPhone: item.providerPhone,
      Commission: item.commissionUpdated,
      IsIncludeCommission: item.isChecked ?? false
    }
    this.ShowLoader = true;
    this.adminService.AddNewCashBox(model).subscribe(data => {
      this.ShowLoader = false;
      if (data.isSuccess) {
        let obj = this.MessageList.find(i => i.messageTransactionId == item.messageTransactionId);
        if (obj) {
          this.MessageList = this.MessageList.filter(i => i.messageTransactionId != item.messageTransactionId);
          this.OnCalculated.emit();
        }

        this.toaster.success(`${item.operationType == 1 || item.operationType == 3 ? 'تم الاستلام' : 'تم التسليم'} بنجاح `);
      } else
        this.toaster.error('لقد حدث خطا');
    });
  }

  MakeMessageAsDelayed(messageTransactionId: any) {
    this.ShowLoader = true;
    this.adminService.MakeMessageAsDelayed(messageTransactionId).subscribe(data => {
      this.ShowLoader = false;
      if (data.isSuccess) {
        let obj = this.MessageList.find(i => i.messageTransactionId == messageTransactionId);
        if (obj) {
          this.MessageList = this.MessageList.filter(i => i.messageTransactionId != messageTransactionId);
          this.OnCalculated.emit();
        }

        this.toaster.success(data.message);
      } else
        this.toaster.error(data.message);
    });
  }
}
