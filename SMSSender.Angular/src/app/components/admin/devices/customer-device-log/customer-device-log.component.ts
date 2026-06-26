import { Component } from '@angular/core';
import { AdminBreadcrumbComponent } from "../../../../shared/admin-breadcrumb/admin-breadcrumb.component";
import { NgxLoadingModule } from "ngx-loading";
import { DeviceService } from '../../../../services/device.service';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';
import { NgFor, NgIf } from '@angular/common';

@Component({
  selector: 'app-customer-device-log',
  imports: [AdminBreadcrumbComponent, NgxLoadingModule,ArabicDateWithTimePipe,NgIf,NgFor],
  templateUrl: './customer-device-log.component.html',
  styleUrl: './customer-device-log.component.css'
})
export class CustomerDeviceLogComponent {
  DeviceLogList: any[] = [];
  ShowLoader = false;

  constructor(private deviceService: DeviceService) { }

  ngOnInit(): void {
    this.GetCustomerDeviceChangeLog();
  }

  GetCustomerDeviceChangeLog() {
    this.ShowLoader = true;
    this.deviceService.GetCustomerDeviceChangeLog().subscribe(data => {
      this.ShowLoader = false;
      this.DeviceLogList = data;
    });
  }
}
