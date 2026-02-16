import { Component, OnInit } from '@angular/core';
import { AdminPaginationComponent } from "../../../../shared/admin-pagination/admin-pagination.component";
import { AdminFilterComponent } from "../../../../shared/admin-filter/admin-filter.component";
import { FilterModel } from '../../../../models/FilterModel';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { AdminService } from '../../../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { NgFor } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-all-message',
  standalone: true,
  imports: [AdminPaginationComponent, NgFor, AdminFilterComponent, NgbModule],
  templateUrl: './all-message.component.html',
  styleUrl: './all-message.component.css'
})
export class AllMessageComponent implements OnInit {
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
