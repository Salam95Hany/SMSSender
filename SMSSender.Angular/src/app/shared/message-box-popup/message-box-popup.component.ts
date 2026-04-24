import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';

interface MessagePreview {
  senderName: string;
  senderNumber: string;
  preview: string;
  time: string;
  unread: boolean;
  accent: string;
  initial: string;
}

@Component({
  selector: 'app-message-box-popup',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './message-box-popup.component.html',
  styleUrl: './message-box-popup.component.css'
})
export class MessageBoxPopupComponent {
  searchTerm = '';
  activeFilter: 'all' | 'unread' = 'all';

  readonly messages: MessagePreview[] = [
    {
      senderName: 'أحمد محمد',
      senderNumber: '+966 50 123 4567',
      preview: 'يرجى تأكيد موعد الاجتماع القادم. هل يناسبك يوم الأحد الساعة 10 صباحا؟',
      time: 'منذ 5 دقائق',
      unread: true,
      accent: 'linear-gradient(135deg, #0f766e, #14b8a6)',
      initial: 'أ',
    },
    {
      senderName: 'سارة أحمد',
      senderNumber: '+966 55 987 6543',
      preview: 'شكرا على سرعة المتابعة. تم استلام كل الملفات المطلوبة ويمكننا إغلاق الطلب.',
      time: 'منذ ساعة',
      unread: false,
      accent: 'linear-gradient(135deg, #2563eb, #38bdf8)',
      initial: 'س',
    },
    {
      senderName: 'محمد علي',
      senderNumber: '+966 54 456 7890',
      preview: 'تم إرسال الملفات عبر البريد الإلكتروني. يرجى المراجعة وإبلاغي بأي ملاحظات.',
      time: 'منذ 3 ساعات',
      unread: false,
      accent: 'linear-gradient(135deg, #9333ea, #ec4899)',
      initial: 'م',
    },
    {
      senderName: 'نور الدين',
      senderNumber: '+966 56 789 0123',
      preview: 'أحتاج دعما سريعا لحل المشكلة التقنية التي ظهرت أثناء تنفيذ العملية.',
      time: 'منذ 6 ساعات',
      unread: true,
      accent: 'linear-gradient(135deg, #f97316, #fb7185)',
      initial: 'ن',
    },
    {
      senderName: 'خالد إبراهيم',
      senderNumber: '+966 53 234 5678',
      preview: 'تذكير بموعد التسليم النهائي للمشروع يوم الخميس القادم. الرجاء الالتزام بالخطة.',
      time: 'أمس',
      unread: false,
      accent: 'linear-gradient(135deg, #0f172a, #334155)',
      initial: 'خ',
    },
  ];

  constructor(private modalService: NgbModal) {}

  get unreadCount(): number {
    return this.messages.filter((message) => message.unread).length;
  }

  get filteredMessages(): MessagePreview[] {
    const normalizedQuery = this.searchTerm.trim().toLowerCase();

    return this.messages.filter((message) => {
      const matchesFilter = this.activeFilter === 'all' || message.unread;
      const matchesQuery =
        !normalizedQuery ||
        message.senderName.toLowerCase().includes(normalizedQuery) ||
        message.senderNumber.toLowerCase().includes(normalizedQuery) ||
        message.preview.toLowerCase().includes(normalizedQuery);

      return matchesFilter && matchesQuery;
    });
  }

  setFilter(filter: 'all' | 'unread'): void {
    this.activeFilter = filter;
  }

  dismissModal(): void {
    this.modalService.dismissAll();
  }
}
