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

  connect() {
    return new EventSource(this.Url + 'SMSReader/stream');
  }

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

  GetMessageDetailsById(TransactionId: any) {
    return this.http.get<ApiResponseModel<any>>(this.Url + 'Message/GetMessageDetailsById?TransactionId=' + TransactionId);
  }

  UpdateTransactionMessage(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Message/UpdateTransactionMessage', Model);
  }

  CorrectionProcess(Model: any) {
    return this.http.post<any>(this.Url + 'Message/CorrectionProcess', Model);
  }

  GetMessageNotification() {
    return this.http.get<ApiResponseModel<any[]>>(this.Url + 'Message/GetMessageNotification');
  }

  MakeMessageAsRead(MessageTransactionId: number) {
    return this.http.get<ApiResponseModel<any[]>>(this.Url + 'Message/MakeMessageAsRead?MessageTransactionId=' + MessageTransactionId);
  }

  MakeMessageAsDelayed(MessageTransactionId: number) {
    return this.http.get<ApiResponseModel<any[]>>(this.Url + 'Message/MakeMessageAsDelayed?MessageTransactionId=' + MessageTransactionId);
  }

  // ============================== CashBox ==============================

  GetCashBoxData(PagingFilter: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.Url + 'CashBox/GetCashBoxData', PagingFilter);
  }

  GetCashBoxFilters(PagingFilter: PagingFilterModel) {
    return this.http.post<ApiResponseModel<any[]>>(this.Url + 'CashBox/GetCashBoxFilters', PagingFilter);
  }

  AddNewCashBox(Model: any) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'CashBox/AddNewCashBox', Model);
  }

  DeleteCashBox(CashBoxId: Number, UserId: any) {
    return this.http.get<ApiResponseModel<any>>(this.Url + 'CashBox/DeleteCashBox?CashBoxId=' + CashBoxId + '&UserId=' + UserId);
  }
}
