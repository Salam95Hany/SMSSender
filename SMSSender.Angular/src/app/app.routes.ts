import { Routes } from '@angular/router';
import { LoginPageComponent } from './auth/login-page/login-page.component';
import { NotAuthorizedComponent } from './auth/not-authorized/not-authorized.component';
import { AdminLayoutComponent } from './components/admin-layout.component';
import { DashboardComponent } from './components/admin/ManageMessage/dashboard/dashboard.component';
import { AllMessageComponent } from './components/admin/ManageMessage/all-message/all-message.component';
import { DepositComponent } from './components/admin/ManageMessage/deposit/deposit.component';
import { TransformationComponent } from './components/admin/ManageMessage/transformation/transformation.component';
import { WalletAccountComponent } from './components/admin/ManageMessage/wallet-account/wallet-account.component';
import { WalletOperationsComponent } from './components/admin/ManageMessage/wallet-operations/wallet-operations.component';
import { WithdrawComponent } from './components/admin/ManageMessage/withdraw/withdraw.component';
import { DepositReportComponent } from './components/admin/Reports/deposit-report/deposit-report.component';
import { WithdrawReportComponent } from './components/admin/Reports/withdraw-report/withdraw-report.component';
import { TransformationReportComponent } from './components/admin/Reports/transformation-report/transformation-report.component';
import { FactoryComponent } from './components/admin/Settings/factory/factory.component';
import { UserComponent } from './components/admin/Settings/user/user.component';
import { UserProfileComponent } from './components/admin/Settings/user-profile/user-profile.component';
import { CashBoxComponent } from './components/admin/ManageMessage/cash-box/cash-box.component';

export const routes: Routes = [
    { path: '', component: LoginPageComponent },
    { path: 'not-authorized', component: NotAuthorizedComponent },
    {
        path: 'admin',
        component: AdminLayoutComponent,
        // canActivate: [authGuard],
        // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
        children: [
            // Manage Message
            {
                path: 'dashboard',
                component: DashboardComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'all-message',
                component: AllMessageComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'deposit',
                component: DepositComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'transformation',
                component: TransformationComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'wallet-account',
                component: WalletAccountComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'wallet-operation',
                component: WalletOperationsComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'withdraw',
                component: WithdrawComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'cash-box',
                component: CashBoxComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            // Reports
            {
                path: 'deposit-report',
                component: DepositReportComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'withdraw-report',
                component: WithdrawReportComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'transformation-report',
                component: TransformationReportComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            // Settings
            {
                path: 'factory',
                component: FactoryComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'user',
                component: UserComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            {
                path: 'user-profile',
                component: UserProfileComponent,
                // canActivate: [authGuard],
                // data: { roles: ["SupperAdmin", "Admin", "ReadOnly","FollowUpOnly"] },
            },
            { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
        ],
    },
    { path: '', redirectTo: 'login', pathMatch: 'full' },
    { path: '**', redirectTo: 'login', pathMatch: 'full' },

];
