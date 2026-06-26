import { Component } from '@angular/core';
import { AdminBreadcrumbComponent } from "../../../../shared/admin-breadcrumb/admin-breadcrumb.component";
import { NgxLoadingModule } from "ngx-loading";
import { DeviceService } from '../../../../services/device.service';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { NgFor, NgIf } from '@angular/common';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormService } from '../../../../services/form.service';
import { AdminGeneralInputComponent } from '../../../../shared/admin-general-input/admin-general-input.component';
import { NotificationSignalrService } from '../../../../services/notification-signalr.service';

@Component({
  selector: 'app-customer-device-refresh-inbox',
  imports: [AdminBreadcrumbComponent, NgxLoadingModule, AdminEmptyStateComponent, NgIf, NgFor, ArabicDateWithTimePipe, AdminGeneralInputComponent, FormsModule, ReactiveFormsModule],
  templateUrl: './customer-device-refresh-inbox.component.html',
  styleUrl: './customer-device-refresh-inbox.component.css'
})
export class CustomerDeviceRefreshInboxComponent {
  DeviceInboxList: any[] = [];
  ShowLoader = false;
  ItemForm: FormGroup;
  formErrors = {
    From: '',
    To: ''
  };

  constructor(private deviceService: DeviceService, private toaster: ToastrService, private modalService: NgbModal, private fb: FormBuilder, private formService: FormService,
    private notificationSignalrService: NotificationSignalrService
  ) { }

  ngOnInit(): void {
    this.FormInit();
    this.GetCustomerDeviceRefreshInbox();
    this.notificationSignalrService.onSystemMessageAdded(() => {
      this.GetCustomerDeviceRefreshInbox();
    });
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      DeviceId: '',
      BranchId: 0,
      From: ['', Validators.required],
      To: ['', Validators.required]
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  openAddItemModal(content: any, item: any) {
    this.ItemForm.reset();
    this.ItemForm.patchValue({
      DeviceId: item.deviceId,
      BranchId: item.branchId
    });
    this.modalService.open(content, {
      size: 'lg',
      scrollable: true,
      centered: true
    });
  }

  GetCustomerDeviceRefreshInbox() {
    this.ShowLoader = true;
    this.deviceService.GetCustomerDeviceRefreshInbox().subscribe(data => {
      this.ShowLoader = false;
      this.DeviceInboxList = data;
      if (this.DeviceInboxList.some(i => i.refreshStatus == 1))
        this.DeviceInboxList.forEach(i => i.isDisabled = true);
      else
        this.DeviceInboxList.forEach(i => i.isDisabled = false);
    });
  }

  validateForm(): boolean {
    this.formService.markFormGroupTouched(this.ItemForm);
    if (this.ItemForm.valid) {
      return true;
    } else {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, false)
      return false;
    }
  }

  AddNewItem() {
    this.ItemForm = this.formService.TrimFormInputValue(this.ItemForm);
    let isValid = this.validateForm();
    if (!isValid)
      return;

    const from = new Date(this.ItemForm.value.From);
    const to = new Date(this.ItemForm.value.To);

    if (to < from) {
      this.toaster.warning('تاريخ النهاية يجب أن يكون أكبر من أو يساوي تاريخ البداية');
      return;
    }

    if (from.getFullYear() !== to.getFullYear() || from.getMonth() !== to.getMonth()) {
      this.toaster.warning('يجب أن يكون تاريخ البداية والنهاية ضمن نفس الشهر');
      return;
    }

    this.deviceService.RefreshDeviceInbox(this.ItemForm.value).subscribe(data => {
      if (data.isSuccess) {
        this.GetCustomerDeviceRefreshInbox();
        this.toaster.success('...جاري التحديث برجاء الانتظار');
        this.modalService.dismissAll();
      } else
        this.toaster.error(data.message);
    });
  }
}
