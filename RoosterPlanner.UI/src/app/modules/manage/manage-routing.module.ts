import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {ManageComponent} from "../../pages/manage/manage.component";
import {ManageGuard} from "../../guards/manage.guard";
import {AddShiftsComponent} from "../../components/add-shifts/add-shifts.component";
import {MsalGuard} from "../../msal";
import {ShiftOverviewComponent} from "../../pages/shift-overview/shift-overview.component";
import {PlanShiftComponent} from "../../pages/plan-shift/plan-shift.component";
import {PlanComponent} from "../../pages/plan/plan.component";
import {ShiftComponent} from "../../pages/shift/shift.component";

const routes: Routes = [
  {
    path: '',
    component: ManageComponent,
    canActivate: [
      ManageGuard,
      ManageGuard
    ]
  },
  {
    path: 'shifts/addshifts/:id',
    component: AddShiftsComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'shifts/:id', //manage
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
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'plan/shift/:id', //manage
    component: PlanShiftComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'plan/:id/:date', //mange
    component: PlanComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  {
    path: 'plan/:id', //mange
    component: PlanComponent,
    canActivate: [
      MsalGuard,
      ManageGuard
    ]
  },
  ];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule { }
