import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { ApiResponseModel } from '../models/ApiResponseModel';

@Injectable({
  providedIn: 'root'
})
export class DeviceService {
  Url = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // ============================== Device ==============================

  GetCustomerDevices() {
    return this.http.get<any[]>(this.Url + 'Device/GetCustomerDevices');
  }

  GetCustomerDeviceChangeLog() {
    return this.http.get<any[]>(this.Url + 'Device/GetCustomerDeviceChangeLog');
  }

  GetCustomerDeviceHealthStatus() {
    return this.http.get<any[]>(this.Url + 'Device/GetCustomerDeviceHealthStatus');
  }

  GetCustomerDeviceRefreshInbox() {
    return this.http.get<any[]>(this.Url + 'Device/GetCustomerDeviceRefreshInbox');
  }

  AddNewDevice(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Device/AddNewDevice', Model);
  }

  UpdateDevice(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Device/UpdateDevice', Model);
  }

  DeleteDevice(DeviceId: number, IsActive: boolean) {
    return this.http.get<ApiResponseModel<any>>(this.Url + 'Device/DeleteDevice?DeviceId=' + DeviceId + '&IsActive=' + IsActive);
  }

   RefreshDeviceInbox(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Device/RefreshDeviceInbox', Model);
  }
}
