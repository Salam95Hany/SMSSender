import { CommonModule, NgFor, NgIf } from '@angular/common';
import { Component, ElementRef, Input, input, OnInit, ViewChild } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { TimeAgoTodayPipe } from '../../pipes/time-ago-today.pipe';
import { PagingFilterModel } from '../../models/PagingFilterModel';
import { AdminService } from '../../services/admin.service';
import { NgxLoadingModule } from "ngx-loading";

@Component({
  selector: 'app-notification-box-popup',
  standalone: true,
  imports: [NgIf, NgFor, CommonModule, TimeAgoTodayPipe, NgxLoadingModule],
  templateUrl: './notification-box-popup.component.html',
  styleUrl: './notification-box-popup.component.css'
})
export class NotificationBoxPopupComponent implements OnInit {
  @Input() NotificationList: any[] = [];
  @Input() TotalCount: number = 0;
  @ViewChild('observer', { static: false }) observer!: ElementRef;
  Loading = false;
  hasMore = true;
  ShowLoader = false;
  PagingFilter: PagingFilterModel = { pagesize: 10, currentpage: 1, operationType: 0, filterList: [] };

  constructor(private modalService: NgbModal,private adminService: AdminService) { }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.initObserver();
  }

  initObserver() {
    const options = {
      root: document.querySelector('#notifList'),
      threshold: 1.0
    };

    const observer = new IntersectionObserver((entries) => {
      if (entries[0].isIntersecting) {
        this.loadMore();
      }
    }, options);

    observer.observe(this.observer.nativeElement);
  }

  loadMore() {
  if (this.Loading || !this.hasMore) return;

  this.Loading = true;
  this.ShowLoader = true;

  this.PagingFilter.currentpage++;

  this.adminService.GetMessageNotification(this.PagingFilter).subscribe({
      next: (res) => {
        if (res.results.length === 0) {
          this.hasMore = false;
        } else {
          this.NotificationList = [
            ...this.NotificationList,
            ...res.results
          ];
        }

        this.Loading = false;
        this.ShowLoader = false;
      },
      error: () => {
        this.Loading = false;
        this.ShowLoader = false;
      }
    });
}

  dismissModal(): void {
    this.modalService.dismissAll();
  }

  getProviderClass(type: string): string {
    switch (type) {
      case 'VodafoneCash': return 'c-red';
      case 'InstaPay': return 'c-blue';
      default: return 'c-orange';
    }
  }

}
