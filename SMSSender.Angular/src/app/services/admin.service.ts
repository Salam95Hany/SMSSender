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

  // ============================== Admissions ==============================

  GetAllAdmissionData(PagingFilter: PagingFilterModel, PatientId: number) {
    return this.http.post<ApiResponseModel<any>>(this.Url + 'Admissions/GetAllAdmissionData?PatientId=' + PatientId, PagingFilter);
  }
}
