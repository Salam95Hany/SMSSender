export interface AdminNavigationItem {
  label: string;
  icon: string;
  route: string;
  exact?: boolean;
}

export interface AdminNavigationSection {
  id: string;
  label: string;
  items: AdminNavigationItem[];
}

export const ADMIN_NAVIGATION: AdminNavigationSection[] = [
  {
    id: 'message-management',
    label: 'إدارة الرسائل',
    items: [
      { label: 'لوحة التحكم', icon: 'fas fa-chart-pie', route: '/admin/dashboard', exact: true },
      { label: 'الرسائل', icon: 'fas fa-envelope-open-text', route: '/admin/all-message', exact: true },
      { label: 'الإيداعات', icon: 'fas fa-cash-register', route: '/admin/deposit', exact: true },
      { label: 'التحويلات', icon: 'fas fa-arrow-right-arrow-left', route: '/admin/transformation', exact: true },
      { label: 'السحوبات', icon: 'fas fa-money-bill-wave', route: '/admin/withdraw', exact: true },
      { label: 'رصيد المحافظ', icon: 'fas fa-wallet', route: '/admin/wallet-account', exact: true },
      { label: 'العمليات الشهرية', icon: 'fas fa-list-check', route: '/admin/wallet-operation', exact: true },
      { label: 'الصندوق', icon: 'fas fa-vault', route: '/admin/cash-box', exact: true },
    ],
  },
  {
    id: 'reports',
    label: 'التقارير',
    items: [
      { label: 'تقرير الإيداعات', icon: 'fas fa-file-invoice-dollar', route: '/admin/deposit-report', exact: true },
      { label: 'تقرير التحويلات', icon: 'fas fa-chart-line', route: '/admin/transformation-report', exact: true },
      { label: 'تقرير السحوبات', icon: 'fas fa-chart-column', route: '/admin/withdraw-report', exact: true },
    ],
  },
  {
    id: 'settings',
    label: 'الإعدادات',
    items: [
      { label: 'المستخدمون', icon: 'fas fa-users-cog', route: '/admin/user', exact: true },
      { label: 'الملف الشخصي', icon: 'fas fa-id-card', route: '/admin/user-profile', exact: true },
      { label: 'إعادة ضبط المصنع', icon: 'fas fa-rotate-left', route: '/admin/factory', exact: true },
    ],
  },
];
