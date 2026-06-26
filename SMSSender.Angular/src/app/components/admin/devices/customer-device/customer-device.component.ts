import { Component } from '@angular/core';
import { AdminBreadcrumbComponent } from "../../../../shared/admin-breadcrumb/admin-breadcrumb.component";
import { NgxLoadingModule } from "ngx-loading";
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { DeviceService } from '../../../../services/device.service';
import { ToastrService } from 'ngx-toastr';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormService } from '../../../../services/form.service';
import { AdminDropDownComponent } from '../../../../shared/admin-drop-down/admin-drop-down.component';
import { AdminGeneralInputComponent } from '../../../../shared/admin-general-input/admin-general-input.component';
import { NgFor, NgIf } from '@angular/common';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';

@Component({
  selector: 'app-customer-device',
  imports: [AdminBreadcrumbComponent, NgxLoadingModule, AdminDropDownComponent, AdminGeneralInputComponent, FormsModule, NgIf, NgFor, ReactiveFormsModule, AdminEmptyStateComponent],
  templateUrl: './customer-device.component.html',
  styleUrl: './customer-device.component.css'
})
export class CustomerDeviceComponent {
  DeviceList: any[] = [];
  ItemForm: FormGroup;
  UserModel: any;
  ShowLoader = false;
  IsActive: boolean;
  DeviceId: any;
  Branches: any[] = [
    { id: 1, name: 'سنتر الشامي 1' },
    { id: 2, name: 'سنتر الشامي 2' }
  ];
  formErrors = {
    DeviceUniqueId: '',
    BranchId: '',
    DeviceName: '',
    BaseUrl: '',
    Username: '',
    Password: '',
    Sim1Number: '',
    Sim1Name: ''
  };

  constructor(private deviceService: DeviceService, private toaster: ToastrService, private modalService: NgbModal, private fb: FormBuilder, private formService: FormService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel') ?? 'null');
    this.FormInit();
    this.GetCustomerDevices();
  }

  FormInit() {
    this.ItemForm = this.fb.group({
      DeviceId: 0,
      DeviceUniqueId: ['', Validators.required],
      BranchId: ['', Validators.required],
      DeviceName: ['', Validators.required],
      BaseUrl: ['http://192.168.1.', [Validators.required, Validators.pattern(
        /^https?:\/\/(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)\/?$/
      )]],
      Username: ['', Validators.required],
      Password: ['', Validators.required],
      Sim1Number: ['', Validators.required],
      Sim2Number: [''],
      Sim1Name: ['', Validators.required],
      Sim2Name: [''],
      IsActive: true
    });

    this.ItemForm.valueChanges.subscribe((data) => {
      this.formErrors = this.formService.validateForm(this.ItemForm, this.formErrors, true);
    });
  }

  FillEditForm(item: any) {
    this.ItemForm.patchValue({
      DeviceId: item.deviceId,
      DeviceUniqueId: item.deviceUniqueId,
      BranchId: item.branchId,
      DeviceName: item.deviceName,
      BaseUrl: item.baseUrl,
      Username: item.username,
      Password: item.password,
      Sim1Number: item.sim1Number,
      Sim2Number: item.sim2Number,
      Sim1Name: item.sim1Name,
      Sim2Name: item.sim2Name,
      IsActive: item.isActive
    });
  }

  ResetForm() {
    this.ItemForm.reset({
      DeviceId: 0,
      BaseUrl: '192.168.1.',
      IsActive: true,
      Sim2Number: '',
      Sim2Name: ''
    });
  }

  openAddItemModal(content: any, item: any) {
    this.ResetForm();
    if (item) {
      this.FillEditForm(item);
    }
    this.modalService.open(content, {
      size: 'xl',
      scrollable: true,
      centered: true
    });
  }

  openDeleteItemModal(content: any, deviceId: any) {
    this.DeviceId = deviceId;
    this.IsActive = this.DeviceList.find(x => x.deviceId == deviceId)?.isActive;
    this.modalService.open(content, {
      size: 'md',
      scrollable: true,
      centered: true
    });
  }

  GetCustomerDevices() {
    this.ShowLoader = true;
    this.deviceService.GetCustomerDevices().subscribe(data => {
      this.ShowLoader = false;
      this.DeviceList = data;
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

    let deviceId = this.ItemForm.value.DeviceId;
    if (!deviceId || deviceId == 0) {
      this.deviceService.AddNewDevice(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.GetCustomerDevices();
          this.toaster.success(data.message);
          this.modalService.dismissAll();
        } else
          this.toaster.error(data.message);
      });
    } else {
      this.deviceService.UpdateDevice(this.ItemForm.value).subscribe(data => {
        if (data.isSuccess) {
          this.GetCustomerDevices();
          this.toaster.success(data.message);
          this.modalService.dismissAll();
        } else
          this.toaster.error(data.message);
      });
    }

  }

  DeleteItem() {
    this.IsActive = !this.IsActive;
    this.deviceService.DeleteDevice(this.DeviceId, this.IsActive).subscribe(data => {
      if (data.isSuccess) {
        this.GetCustomerDevices();
        this.toaster.success(data.message);
        this.modalService.dismissAll();
      } else
        this.toaster.error(data.message);
    })
  }
}
