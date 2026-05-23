import { Routes } from '@angular/router';
import { authGuard } from './auth/auth.guard';
import { guestGuard } from './auth/guest.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('./auth/login-page/login-page.component').then((m) => m.LoginPageComponent),
    canActivate: [guestGuard],
    data: {
      title: 'بوابة الدخول',
    },
  },
  {
    path: 'not-authorized',
    loadComponent: () => import('./auth/not-authorized/not-authorized.component').then((m) => m.NotAuthorizedComponent),
    data: {
      title: 'غير مصرح',
    },
  },
  {
    path: 'admin',
    loadComponent: () => import('./components/admin-layout.component').then((m) => m.AdminLayoutComponent),
    canActivate: [authGuard],
    data: { roles: ['Admin', 'Manager', 'Cashier'] },
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/admin/ManageMessage/dashboard/dashboard.component').then((m) => m.DashboardComponent),
        canActivate: [authGuard],
        data: {
          section: 'إدارة الرسائل',
          title: 'لوحة التحكم',
          description: 'متابعة سريعة لحركة الرسائل والعمليات اليومية مع وصول مباشر لأهم الإجراءات.',
          roles: ['Admin', 'Manager', 'Cashier']
        },
      },
      {
        path: 'all-message',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        canActivate: [authGuard],
        data: {
          section: 'سجل الرسائل',
          title: 'الرسائل',
          description: 'استعرض الرسائل الواردة ونفذ عمليات المتابعة من واجهة واضحة وقابلة للتصفية.',
          opreationType: 0,
          roles: ['Admin', 'Manager', 'Cashier']
        },
      },
      {
        path: 'deposit',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        canActivate: [authGuard],
        data: {
          section: 'عمليات الإيداع',
          title: 'الإيداع',
          description: 'راقب عمليات الإيداع وحدد النتائج بسرعة على مختلف أحجام الشاشات.',
          opreationType: 1,
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'withdraw',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        canActivate: [authGuard],
        data: {
          section: 'عمليات السحب',
          title: 'السحب',
          description: 'راجع العمليات الدورية مع أدوات تصفية سريعة وتجربة قراءة مناسبة للهاتف والكمبيوتر.',
          opreationType: 2,
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'cash-withdrawal',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        canActivate: [authGuard],
        data: {
          section: 'إدارة الرسائل',
          title: 'السيولة',
          description: 'راجع العمليات الدورية مع أدوات تصفية سريعة وتجربة قراءة مناسبة للهاتف والكمبيوتر.',
          opreationType: 3,
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'balance-inquiry',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        canActivate: [authGuard],
        data: {
          section: 'إدارة الرسائل',
          title: 'الرصيد',
          description: 'راجع العمليات الدورية مع أدوات تصفية سريعة وتجربة قراءة مناسبة للهاتف والكمبيوتر.',
          opreationType: 5,
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'wallet-account',
        loadComponent: () => import('./components/admin/ManageMessage/wallet-account/wallet-account.component').then((m) => m.WalletAccountComponent),
        canActivate: [authGuard],
        data: {
          section: 'إدارة الرسائل',
          title: 'رصيد المحافظ',
          description: 'لوحة بطاقات مرنة لعرض المحافظ والأرصدة الحالية بتوزيع متجاوب وواضح.',
          roles: ['Admin', 'Manager','Cashier']
        },
      },
      {
        path: 'cash-box',
        loadComponent: () => import('./components/admin/ManageMessage/cash-box/cash-box.component').then((m) => m.CashBoxComponent),
        canActivate: [authGuard],
        data: {
          section: 'إدارة الرسائل',
          title: 'الصندوق',
          description: 'شاشة تشغيل عملية لإدارة الصندوق مع وصول مباشر للإجراءات والبحث والتصفية.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'all-message-report',
        loadComponent: () => import('./components/admin/Reports/all-message-report/all-message-report.component').then((m) => m.AllMessageReportComponent),
        canActivate: [authGuard],
        data: {
          section: 'التقارير',
          title: 'تقرير العمليات',
          opreationType: 0,
          description: 'عرض مؤشرات وتقارير العمليات ضمن بنية قابلة للتوسعة وملائمة للشاشات الكبيرة والصغيرة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'deposit-report',
        loadComponent: () => import('./components/admin/Reports/all-message-report/all-message-report.component').then((m) => m.AllMessageReportComponent),
        canActivate: [authGuard],
        data: {
          section: 'التقارير',
          title: 'تقرير الإيداع',
          opreationType: 1,
          description: 'عرض مؤشرات وتقارير الإيداع ضمن بنية قابلة للتوسعة وملائمة للشاشات الكبيرة والصغيرة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'withdrawal-report',
        loadComponent: () => import('./components/admin/Reports/all-message-report/all-message-report.component').then((m) => m.AllMessageReportComponent),
        canActivate: [authGuard],
        data: {
          section: 'التقارير',
          title: 'تقرير السحب',
          opreationType: 2,
          description: 'عرض مؤشرات وتقارير السحب ضمن بنية قابلة للتوسعة وملائمة للشاشات الكبيرة والصغيرة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'cash-withdrawal-report',
        loadComponent: () => import('./components/admin/Reports/all-message-report/all-message-report.component').then((m) => m.AllMessageReportComponent),
        canActivate: [authGuard],
        data: {
          section: 'التقارير',
          title: 'تقرير السيولة',
          opreationType: 3,
          description: 'عرض مؤشرات وتقارير السيولة ضمن بنية قابلة للتوسعة وملائمة للشاشات الكبيرة والصغيرة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'wallet-profit-report',
        loadComponent: () => import('./components/admin/Reports/wallet-profit-report/wallet-profit-report.component').then((m) => m.WalletProfitReportComponent),
        canActivate: [authGuard],
        data: {
          section: 'التقارير',
          title: 'تقرير الأرباح',
          description: 'استخرج قراءات واضحة لحركة الأرباح مع تجربة تصفح أكثر هدوءا وتركيزا.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'factory',
        loadComponent: () => import('./components/admin/Settings/factory/factory.component').then((m) => m.FactoryComponent),
        canActivate: [authGuard],
        data: {
          section: 'الإعدادات',
          title: 'إعادة ضبط المصنع',
          description: 'سير عمل واضح وآمن للعمليات الحساسة مع إبراز المخاطر والخطوات المطلوبة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'user',
        loadComponent: () => import('./components/admin/Settings/user/user.component').then((m) => m.UserComponent),
        canActivate: [authGuard],
        data: {
          section: 'الإعدادات',
          title: 'المستخدمون',
          description: 'إدارة المستخدمين والأدوار من خلال جداول مرنة وأدوات تنظيم واضحة.',
          roles: ['Admin', 'Manager']
        },
      },
      {
        path: 'user-profile',
        loadComponent: () => import('./components/admin/Settings/user-profile/user-profile.component').then((m) => m.UserProfileComponent),
        canActivate: [authGuard],
        data: {
          section: 'الإعدادات',
          title: 'الملف الشخصي',
          description: 'عرض منظم لبيانات الحساب والجلسة الحالية مع بطاقات مختصرة سهلة القراءة.',
          roles: ['Admin', 'Manager', 'Cashier']
        },
      },
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      { path: '**', redirectTo: 'dashboard', pathMatch: 'full' },
    ],
  },
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: '**', redirectTo: 'login', pathMatch: 'full' },
];
