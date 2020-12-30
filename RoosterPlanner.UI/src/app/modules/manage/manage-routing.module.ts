import {NgModule} from '@angular/core';
import {Routes, RouterModule} from '@angular/router';
import {ManageComponent} from "../../pages/manage/manage.component";
import {AddShiftsComponent} from "../../components/add-shifts/add-shifts.component";
import {ShiftOverviewComponent} from "../../pages/shift-overview/shift-overview.component";
import {PlanShiftComponent} from "../../pages/plan-shift/plan-shift.component";
import {PlanComponent} from "../../pages/plan/plan.component";
import {ShiftComponent} from "../../pages/shift/shift.component";
import {ScheduleManagerComponent} from "../../pages/schedule-manager/schedule-manager.component";
import {ProfileComponent} from "../../pages/profile/profile.component";

const routes: Routes = [
  {path: '', component: ManageComponent,},
  {path: 'shifts/addshifts/:id', component: AddShiftsComponent,},
  {path: 'shifts/:id', component: ShiftOverviewComponent,},
  {path: 'shift/:id', component: ShiftComponent,},
  {path: 'plan/shift/:id', component: PlanShiftComponent,},
  {path: 'plan/:id/:date', component: PlanComponent,},
  {path: 'plan/:id', component: PlanComponent,},
  {path: 'schedule/:id', component: ScheduleManagerComponent,},
  {path: 'profile/:id', component: ProfileComponent},

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ManageRoutingModule {
}
