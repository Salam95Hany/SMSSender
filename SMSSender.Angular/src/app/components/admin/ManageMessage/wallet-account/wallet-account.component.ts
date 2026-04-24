import { NgClass, NgFor } from '@angular/common';
import { Component } from '@angular/core';
import { AdminBreadcrumbComponent } from '../../../../shared/admin-breadcrumb/admin-breadcrumb.component';

interface WalletCard {
  owner: string;
  provider: string;
  number: string;
  balance: string;
  theme: 'red' | 'blue';
}

@Component({
  selector: 'app-wallet-account',
  standalone: true,
  imports: [NgFor, NgClass, AdminBreadcrumbComponent],
  templateUrl: './wallet-account.component.html',
  styleUrl: './wallet-account.component.css'
})
export class WalletAccountComponent {
  readonly wallets: WalletCard[] = [
    { owner: 'سلام هاني البدوي', provider: 'فودافون كاش', number: '01124564843', balance: '25,450.00 ج.م', theme: 'red' },
    { owner: 'سلام هاني البدوي', provider: 'فودافون كاش', number: '01124564844', balance: '18,220.00 ج.م', theme: 'red' },
    { owner: 'سلام هاني البدوي', provider: 'فودافون كاش', number: '01124564845', balance: '31,900.00 ج.م', theme: 'red' },
    { owner: 'سلام هاني البدوي', provider: 'فودافون كاش', number: '01124564846', balance: '27,115.00 ج.م', theme: 'red' },
    { owner: 'سلام هاني البدوي', provider: 'إنستا باي', number: '01124564847', balance: '11,240.00 ج.م', theme: 'blue' },
    { owner: 'سلام هاني البدوي', provider: 'إنستا باي', number: '01124564848', balance: '16,750.00 ج.م', theme: 'blue' },
  ];
}
