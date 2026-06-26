import { Component } from '@angular/core';
import { AdminBreadcrumbComponent } from "../../../../shared/admin-breadcrumb/admin-breadcrumb.component";
import { NgxLoadingModule } from "ngx-loading";
import { DeviceService } from '../../../../services/device.service';
import { NgFor, NgIf } from '@angular/common';
import { AdminEmptyStateComponent } from '../../../../shared/admin-empty-state/admin-empty-state.component';
import { ArabicDateWithTimePipe } from '../../../../pipes/arabic-date-with-time.pipe';

@Component({
  selector: 'app-customer-device-health-status',
  imports: [AdminBreadcrumbComponent, NgxLoadingModule, NgIf, NgFor, AdminEmptyStateComponent, ArabicDateWithTimePipe],
  templateUrl: './customer-device-health-status.component.html',
  styleUrl: './customer-device-health-status.component.css'
})
export class CustomerDeviceHealthStatusComponent {
  DeviceHealthList: any[] = [];
  ShowLoader = false;

  constructor(private deviceService: DeviceService) { }

  ngOnInit(): void {
    this.GetCustomerDeviceHealthStatus();
  }

  GetCustomerDeviceHealthStatus() {
    this.ShowLoader = true;
    this.deviceService.GetCustomerDeviceHealthStatus().subscribe(data => {
      this.ShowLoader = false;
      this.DeviceHealthList = data;
    });
  }

  isDeviceOnline(lastSeen: string | Date): boolean {
    const lastSeenDate = new Date(lastSeen);
    const now = new Date();
    const diffInMinutes = (now.getTime() - lastSeenDate.getTime()) / (1000 * 60);
    return diffInMinutes <= 2;
  }
}
