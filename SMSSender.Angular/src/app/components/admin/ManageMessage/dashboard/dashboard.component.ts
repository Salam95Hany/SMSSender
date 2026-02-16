import { Component } from '@angular/core';
import { FilterModel } from '../../../../models/FilterModel';
import { AdminService } from '../../../../services/admin.service';
import { NgbModal, NgbModule, NgbOffcanvas } from '@ng-bootstrap/ng-bootstrap';
import { ToastrService } from 'ngx-toastr';
import { AdminPaginationComponent } from "../../../../shared/admin-pagination/admin-pagination.component";
import { CommonModule, NgFor, NgIf } from '@angular/common';
import { AdminFilterComponent } from "../../../../shared/admin-filter/admin-filter.component";
import { AdminStatsCardComponent } from "../../../../shared/admin-stats-card/admin-stats-card.component";
import { MessageBoxPopupComponent } from "../../../../shared/message-box-popup/message-box-popup.component";
import { PagingFilterModel } from '../../../../models/PagingFilterModel';

@Component({
	selector: 'app-dashboard',
	standalone: true,
	imports: [AdminPaginationComponent, NgFor, AdminFilterComponent, NgbModule, AdminStatsCardComponent, MessageBoxPopupComponent],
	templateUrl: './dashboard.component.html',
	styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
	OrdersList: any[] = [];
	FilterList: FilterModel[] = [
		{
			"categoryDisplayName": "بالاسم",
			"categoryName": "AdmissionSearchText",
			"filterType": "SearchText",
			"filterItems": [
				{
					"categoryName": "AdmissionSearchText",
					"itemId": "",
					"itemKey": "",
					"itemValue": "",
					"filterItems": []
				}
			]
		},
		{
			"categoryDisplayName": "Admission Date",
			"categoryName": "AdmissionDate",
			"filterType": "DateRange",
			"filterItems": [
				{
					"categoryName": "AdmissionDate",
					"itemId": "",
					"itemKey": "",
					"itemValue": "",
					"filterItems": []
				}
			]
		},
		{
			"categoryDisplayName": "Governorate",
			"categoryName": "Governorate",
			"filterType": "Checkbox",
			"filterItems": [
				{
					"categoryName": "Governorate",
					"itemId": "Alexandria",
					"itemKey": "Alexandria",
					"itemValue": "1",
					"filterItems": []
				},
				{
					"categoryName": "Governorate",
					"itemId": "Cairo",
					"itemKey": "Cairo",
					"itemValue": "6",
					"filterItems": []
				},
				{
					"categoryName": "Governorate",
					"itemId": "Fayoum",
					"itemKey": "Fayoum",
					"itemValue": "2",
					"filterItems": []
				}
			]
		},
		{
			"categoryDisplayName": "Comorbidities",
			"categoryName": "Comorbidities",
			"filterType": "Checkbox",
			"filterItems": [
				{
					"categoryName": "Comorbidities",
					"itemId": "Diabetes",
					"itemKey": "Diabetes",
					"itemValue": "1",
					"filterItems": []
				},
				{
					"categoryName": "Comorbidities",
					"itemId": "Hypertension",
					"itemKey": "Hypertension",
					"itemValue": "2",
					"filterItems": []
				},
				{
					"categoryName": "Comorbidities",
					"itemId": "Renal insufficiency",
					"itemKey": "Renal insufficiency",
					"itemValue": "1",
					"filterItems": []
				}
			]
		}
	];

	UserModel: any;
	filterList: FilterModel[] = [];
	PagingFilter: PagingFilterModel = {
		pagesize: 10,
		currentpage: 1,
		filterList: []
	};
	TotalCount = 0;
	isFilter = false;

	statsInfo = [
		{ icon: 'fas fa-envelope', number: 26, text: 'إجمالي الرسائل', status: 'blue' },
		{ icon: 'fas fa-cash-register', number: 12, text: 'عمليات الايداع', status: 'green' },
		{ icon: 'fas fa-exchange-alt', number: 124, text: 'عمليات التحويل', status: 'orange' },
		{ icon: 'fas fa-money-bill-wave', number: 13, text: 'عمليات السحب', status: 'red' },
	];

	constructor(private adminService: AdminService, private offcanvasService: NgbOffcanvas,
		private toaster: ToastrService, private modalService: NgbModal) { }

	ngOnInit(): void {
		this.UserModel = JSON.parse(localStorage.getItem('UserModel'));
	}

	OpenMessageBoxModal(content) {
		this.modalService.open(content, { centered: true, size: 'xl' });
	}


	pageChanged(obj: any) {

	}
}
