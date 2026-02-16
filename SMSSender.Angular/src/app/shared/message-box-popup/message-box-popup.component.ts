import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-message-box-popup',
  standalone: true,
  imports: [],
  templateUrl: './message-box-popup.component.html',
  styleUrl: './message-box-popup.component.css'
})
export class MessageBoxPopupComponent implements OnInit {

  constructor(private modalService: NgbModal) {
    
  }

  ngOnInit(): void {
  }

  DismissModal() {
    this.modalService.dismissAll();
  }

}
