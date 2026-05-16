import { Component, EventEmitter, Input, Output, TemplateRef, ViewChild, inject } from '@angular/core';
import { NgClass } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { NgbDropdownModule, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { AuthService } from '../../../auth/auth.service';
import { MessageBoxPopupComponent } from '../../../shared/message-box-popup/message-box-popup.component';
import { AdminService } from '../../../services/admin.service';
import { NotificationSoundService } from '../../../services/notification-sound.service';
import { NotificationBoxPopupComponent } from '../../../shared/notification-box-popup/notification-box-popup.component';
import { NotificationSignalrService } from '../../../services/notification-signalr.service';
import { PagingFilterModel } from '../../../models/PagingFilterModel';

@Component({
  selector: 'app-admin-header',
  standalone: true,
  imports: [NgClass, NgbDropdownModule, RouterLink, MessageBoxPopupComponent, NotificationBoxPopupComponent],
  templateUrl: './admin-header.component.html',
  styleUrl: './admin-header.component.css'
})
export class AdminHeaderComponent {
  @ViewChild('MessageBoxModal') MessageBoxModal: TemplateRef<any>;
  @ViewChild('NotificationBoxModal') NotificationBoxModal: TemplateRef<any>;
  @Input() isSidebarCollapsed = false;
  @Input() isMobileMenuOpen = false;
  @Output() menuToggle = new EventEmitter<void>();
  private es!: EventSource;
  isModalOpen = false;
  MessageList: any[] = [];
  NotificationList: any[] = [];
  TotalCount = 0;
  PagingFilter: PagingFilterModel = { pagesize: 10, currentpage: 1, operationType: 0, filterList: [] };

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly authService = inject(AuthService);
  private readonly modalService = inject(NgbModal);
  private readonly adminService = inject(AdminService);
  private readonly notificationSound = inject(NotificationSoundService);
  private readonly notificationSignalrService = inject(NotificationSignalrService);

  userModel: any = null;
  userName = 'مشرف النظام';
  companyName = 'SMS Sender';
  roleName = 'Administrator';
  pageTitle = 'واجهة الإدارة';

  ngOnInit(): void {
    this.notificationSound.unlockAudio();
    this.notificationSignalrService.startConnection();
    this.refreshUser();
    this.notificationSignalrService.onMessageAdded(() => {
      this.notificationSound.play();
      this.GetMessageNotification();
      this.GetMessageBoxTodayData(true);
    });

    this.GetMessageNotification();
    this.GetMessageBoxTodayData(false);
  }

  OpenMessageBoxModal(): void {
    if (this.isModalOpen) return;

    this.isModalOpen = true;

    const modalRef = this.modalService.open(this.MessageBoxModal, {
      centered: true,
      size: 'sm',
      windowClass: 'messages-modal',
    });

    modalRef.result.finally(() => {
      this.isModalOpen = false;
    });
  }

  OpenNotificationBoxModal() {
    this.modalService.open(this.NotificationBoxModal, {
      centered: true,
      size: 'sm',
      windowClass: 'messages-modal',
    });
  }

  GetMessageNotification() {
    this.adminService.GetMessageNotification(this.PagingFilter).subscribe(data => {
      this.NotificationList = data.results;
      this.TotalCount = data.totalCount;
    });
  }

  GetMessageBoxTodayData(openModal: boolean) {
    this.adminService.GetMessageBoxTodayData().subscribe(data => {
      this.MessageList = data.results;
      if (openModal)
        this.OpenMessageBoxModal();
    });
  }

  get userInitials(): string {
    const source = this.userName || this.userModel?.userName || 'SM';
    const parts = source
      .split(' ')
      .filter(Boolean)
      .slice(0, 2)
      .map((part: string) => part[0]);

    return parts.join('').toUpperCase() || 'SM';
  }

  private refreshUser(): void {
    this.userModel = this.authService.UserModel;
    this.userName = this.userModel?.fullName || this.userModel?.userName || 'مشرف النظام';
    this.companyName = this.userModel?.companyName || 'SMS Sender';
    this.roleName = this.userModel?.role || 'Administrator';
  }

  logOut(): void {
    localStorage.removeItem('UserModel');
    this.router.navigate(['/login']);
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

  RedirectToMessagesPage(messageTransactionId: number) {
    this.adminService.MakeMessageAsRead(messageTransactionId).subscribe(data => {
      if (data.isSuccess) {
        const checked = this.MessageList.find(i => i.messageTransactionId === messageTransactionId);
        if (checked) checked.isRead = true;

        if (this.router.url.includes('/admin/all-message')) {
          this.router.navigate([], {
            relativeTo: this.route,
            queryParams: { id: messageTransactionId },
            queryParamsHandling: 'merge'
          });
        } else {
          this.router.navigate(['/admin/all-message'], {
            queryParams: { id: messageTransactionId }
          });
        }
      }
    })
  }

  ngOnDestroy() {
    if (this.es) {
      this.es.close();
    }
  }
}
