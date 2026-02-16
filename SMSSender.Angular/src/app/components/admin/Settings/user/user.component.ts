import { Component, OnInit } from '@angular/core';
import { AdminService } from '../../../../services/admin.service';
import { ToastrService } from 'ngx-toastr';
import { PagingFilterModel } from '../../../../models/PagingFilterModel';
import { FilterModel } from '../../../../models/FilterModel';
import { AdminPaginationComponent } from '../../../../shared/admin-pagination/admin-pagination.component';
import { AdminFilterComponent } from '../../../../shared/admin-filter/admin-filter.component';
import { NgFor } from '@angular/common';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-user',
    standalone: true,
  imports: [AdminPaginationComponent, NgFor, AdminFilterComponent, NgbModule],
  templateUrl: './user.component.html',
  styleUrl: './user.component.css'
})
export class UserComponent implements OnInit {
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
