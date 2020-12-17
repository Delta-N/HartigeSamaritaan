import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {HomeComponent} from "./pages/home/home.component";
import {NotFoundComponent} from "./pages/not-found/not-found.component";
import {AdminComponent} from "./pages/admin/admin.component";
import {ProfileComponent} from "./pages/profile/profile.component";
import {ShiftComponent} from "./pages/shift/shift.component";
import {AuthorizationGuard} from "./guards/authorization.guard";
import {ProjectComponent} from "./pages/project/project.component";
import {MsalGuard} from "./msal";
import {TaskComponent} from "./pages/task/task.component";
import {AllTasksComponent} from "./pages/all-tasks/all-tasks.component";
import {CategoryComponent} from "./pages/category/category.component";
import {AddShiftsComponent} from "./components/add-shifts/add-shifts.component";
import {ShiftOverviewComponent} from "./pages/shift-overview/shift-overview.component";
import {ManageComponent} from "./pages/manage/manage.component";
import {ManageGuard} from "./guards/manage.guard";
import {AvailabilityComponent} from "./pages/availability/availability.component";
import {PlanComponent} from "./pages/plan/plan.component";
import {PlanShiftComponent} from "./pages/plan-shift/plan-shift.component";

const routes: Routes = [
  {
    path: '',
    component: HomeComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'home',
    component: HomeComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'admin',
    component: AdminComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ],
  },
  {
    path: 'availability/:id',
    component: AvailabilityComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'plan/shift/:id',
    component: PlanShiftComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'plan/:id/:date',
    component: PlanComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'plan/:id',
    component: PlanComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },


  {
    path: 'profile/:id',
    component: ProfileComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'shifts/:id',
    component: ShiftOverviewComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'shift/:id',
    component: ShiftComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'addshifts/:id',
    component: AddShiftsComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'project/:id',
    component: ProjectComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'task/:id',
    component: TaskComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'tasks',
    component: AllTasksComponent,
    canActivate: [
      MsalGuard,
      AuthorizationGuard
    ]
  },
  {
    path: 'category/:id',
    component: CategoryComponent,
    canActivate: [
      MsalGuard
    ]
  },
  {
    path: 'manage',
    component: ManageComponent,
    canActivate: [
      ManageGuard
    ]
  },
  {
    // Needed for hash routing
    path: 'code',
    redirectTo: 'home',
    canActivate: [
      MsalGuard
    ]
  },

  {
    path: '**',
    component: NotFoundComponent
  }
];
const isIframe = window !== window.parent && !window.opener;

@NgModule({
  imports: [RouterModule.forRoot(routes, {
    useHash: false,
    // Don't perform initial navigation in iframes
    initialNavigation: !isIframe ? 'enabled' : 'disabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
