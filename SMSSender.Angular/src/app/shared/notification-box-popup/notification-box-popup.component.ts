import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-notification-box-popup',
  standalone: true,
  imports: [],
  templateUrl: './notification-box-popup.component.html',
  styleUrl: './notification-box-popup.component.css'
})
export class NotificationBoxPopupComponent implements OnInit {

  constructor(private modalService: NgbModal) { }

  ngOnInit(): void {
  }

  dismissModal(): void {
    this.modalService.dismissAll();
  }

}
