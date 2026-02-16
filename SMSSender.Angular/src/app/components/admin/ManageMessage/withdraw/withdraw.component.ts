import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { AdminService } from '../../../../services/admin.service';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { FilterModel } from '../../../../models/FilterModel';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-withdraw',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, AdminFilterComponent, NgbModule],
  templateUrl: './withdraw.component.html',
  styleUrl: './withdraw.component.css'
})
export class WithdrawComponent implements OnInit {
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
