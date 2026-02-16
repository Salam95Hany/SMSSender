import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../../../../services/admin.service';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { FilterModel } from '../../../../models/FilterModel';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { NgFor } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-transformation-report',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, AdminFilterComponent, NgbModule],
  templateUrl: './transformation-report.component.html',
  styleUrl: './transformation-report.component.css'
})
export class TransformationReportComponent implements OnInit {
  OrdersList: any[] = [];
  FilterList: FilterModel[] = [];
  UserModel: any;
  PagingFilter: PagingFilterModel = {
    pagesize: 10,
    currentpage: 1,
    filterList: []
  };
  TotalCount = 0;
  isFilter = true;

  constructor(private adminService: AdminService, private toaster: ToastrService) { }

  ngOnInit(): void {
    this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
  }


  pageChanged(obj: any) {

  }
}
