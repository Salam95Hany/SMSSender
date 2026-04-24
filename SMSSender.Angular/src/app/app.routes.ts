import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', pathMatch: 'full', redirectTo: 'login' },
  {
    path: 'login',
    loadComponent: () => import('./auth/login-page/login-page.component').then((m) => m.LoginPageComponent),
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
    children: [
      {
        path: 'dashboard',
        loadComponent: () => import('./components/admin/ManageMessage/dashboard/dashboard.component').then((m) => m.DashboardComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'لوحة التحكم',
          description: 'متابعة سريعة لحركة الرسائل والعمليات اليومية مع وصول مباشر لأهم الإجراءات.',
        },
      },
      {
        path: 'all-message',
        loadComponent: () => import('./components/admin/ManageMessage/all-message/all-message.component').then((m) => m.AllMessageComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'الرسائل',
          description: 'استعرض الرسائل الواردة ونفذ عمليات المتابعة من واجهة واضحة وقابلة للتصفية.',
        },
      },
      {
        path: 'deposit',
        loadComponent: () => import('./components/admin/ManageMessage/deposit/deposit.component').then((m) => m.DepositComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'الإيداعات',
          description: 'راقب عمليات الإيداع وحدد النتائج بسرعة على مختلف أحجام الشاشات.',
        },
      },
      {
        path: 'transformation',
        loadComponent: () => import('./components/admin/ManageMessage/transformation/transformation.component').then((m) => m.TransformationComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'التحويلات',
          description: 'تحكم في التحويلات وتابع مؤشرات الأداء اليومية ضمن صفحة مركزة وسهلة القراءة.',
        },
      },
      {
        path: 'wallet-account',
        loadComponent: () => import('./components/admin/ManageMessage/wallet-account/wallet-account.component').then((m) => m.WalletAccountComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'رصيد المحافظ',
          description: 'لوحة بطاقات مرنة لعرض المحافظ والأرصدة الحالية بتوزيع متجاوب وواضح.',
        },
      },
      {
        path: 'wallet-operation',
        loadComponent: () => import('./components/admin/ManageMessage/wallet-operations/wallet-operations.component').then((m) => m.WalletOperationsComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'العمليات الشهرية',
          description: 'راجع العمليات الدورية مع أدوات تصفية سريعة وتجربة قراءة مناسبة للهاتف والكمبيوتر.',
        },
      },
      {
        path: 'withdraw',
        loadComponent: () => import('./components/admin/ManageMessage/withdraw/withdraw.component').then((m) => m.WithdrawComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'السحوبات',
          description: 'إدارة السحوبات بواجهة موحدة تسهل الفحص والمراجعة واتخاذ الإجراء المناسب.',
        },
      },
      {
        path: 'cash-box',
        loadComponent: () => import('./components/admin/ManageMessage/cash-box/cash-box.component').then((m) => m.CashBoxComponent),
        data: {
          section: 'إدارة الرسائل',
          title: 'الصندوق',
          description: 'شاشة تشغيل عملية لإدارة الصندوق مع وصول مباشر للإجراءات والبحث والتصفية.',
        },
      },
      {
        path: 'deposit-report',
        loadComponent: () => import('./components/admin/Reports/deposit-report/deposit-report.component').then((m) => m.DepositReportComponent),
        data: {
          section: 'التقارير',
          title: 'تقرير الإيداعات',
          description: 'اعرض مؤشرات وتقارير الإيداعات ضمن بنية قابلة للتوسعة وملائمة للشاشات الكبيرة والصغيرة.',
        },
      },
      {
        path: 'withdraw-report',
        loadComponent: () => import('./components/admin/Reports/withdraw-report/withdraw-report.component').then((m) => m.WithdrawReportComponent),
        data: {
          section: 'التقارير',
          title: 'تقرير السحوبات',
          description: 'استخرج قراءات واضحة لحركة السحوبات مع تجربة تصفح أكثر هدوءا وتركيزا.',
        },
      },
      {
        path: 'transformation-report',
        loadComponent: () => import('./components/admin/Reports/transformation-report/transformation-report.component').then((m) => m.TransformationReportComponent),
        data: {
          section: 'التقارير',
          title: 'تقرير التحويلات',
          description: 'مؤشرات التحويلات في واجهة أكثر وضوحا، مع بطاقات ملخص وجداول قابلة للتصفح على الهاتف.',
        },
      },
      {
        path: 'factory',
        loadComponent: () => import('./components/admin/Settings/factory/factory.component').then((m) => m.FactoryComponent),
        data: {
          section: 'الإعدادات',
          title: 'إعادة ضبط المصنع',
          description: 'سير عمل واضح وآمن للعمليات الحساسة مع إبراز المخاطر والخطوات المطلوبة.',
        },
      },
      {
        path: 'user',
        loadComponent: () => import('./components/admin/Settings/user/user.component').then((m) => m.UserComponent),
        data: {
          section: 'الإعدادات',
          title: 'المستخدمون',
          description: 'إدارة المستخدمين والأدوار من خلال جداول مرنة وأدوات تنظيم واضحة.',
        },
      },
      {
        path: 'user-profile',
        loadComponent: () => import('./components/admin/Settings/user-profile/user-profile.component').then((m) => m.UserProfileComponent),
        data: {
          section: 'الإعدادات',
          title: 'الملف الشخصي',
          description: 'عرض منظم لبيانات الحساب والجلسة الحالية مع بطاقات مختصرة سهلة القراءة.',
        },
      },
      { path: '', pathMatch: 'full', redirectTo: 'dashboard' },
    ],
  },
  { path: '**', redirectTo: 'login' },
];
