import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { ApiResponseModel } from '../models/ApiResponseModel';
import { PagingFilterModel } from '../models/PagingFilterModel';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AdminService {
  Url = environment.apiUrl;

  constructor(private http: HttpClient) { }

  // ============================== Message ==============================

  GetSmsDataByOperationType(PagingFilter: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Message/GetSmsDataByOperationType', PagingFilter);
  }

  GetSmsFilterByOperationType(PagingFilter: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Message/GetSmsFilterByOperationType', PagingFilter);
  }

  GetOperationCountDashboardSummary() {
    return this.http.get<ApiResponseModel<any>>(this.Url + 'Message/GetOperationCountDashboardSummary');
  }

  GetTodayLatestTransactions() {
    return this.http.get<ApiResponseModel<any>>(this.Url + 'Message/GetTodayLatestTransactions');
  }
}
