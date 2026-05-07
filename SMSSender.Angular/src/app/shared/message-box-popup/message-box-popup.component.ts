import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TimeAgoTodayPipe } from '../../pipes/time-ago-today.pipe';

interface MessagePreview {
  senderName: string;
  senderNumber: string;
  preview: string;
  time: string;
  unread: boolean;
  accent: string;
  initial: string;
}

@Component({
  selector: 'app-message-box-popup',
  standalone: true,
  imports: [CommonModule, FormsModule, TimeAgoTodayPipe, NgIf, NgFor],
  templateUrl: './message-box-popup.component.html',
  styleUrl: './message-box-popup.component.css'
})
export class MessageBoxPopupComponent implements OnInit {
  @Input() MessageList: any[] = [];
  searchTerm = '';

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
    this.MessageList = this.MessageList.filter(i => i.operationType == 1 || i.operationType == 2)
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
    if (item.operationType == 2) {
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
    } else if (item.operationType == 2) {
      if (item.isChecked)
        item.customerReceive = originalAmount - commission;
    }


  }

  dismissModal(): void {
    this.modalService.dismissAll();
  }
}
