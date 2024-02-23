import {Component, OnInit} from '@angular/core';
import {Shift} from '../../models/shift';
import {ShiftService} from '../../services/shift.service';
import {ActivatedRoute, ParamMap} from '@angular/router';
import {ConfirmDialogComponent, ConfirmDialogModel} from '../../components/confirm-dialog/confirm-dialog.component';
import {MatDialog} from '@angular/material/dialog';
import {Location} from '@angular/common';
import {EditShiftComponent} from '../../components/edit-shift/edit-shift.component';
import {ToastrService} from 'ngx-toastr';
import {UserService} from '../../services/user.service';
import {BreadcrumbService} from '../../services/breadcrumb.service';
import {Breadcrumb} from '../../models/breadcrumb';

@Component({
  selector: 'app-shift',
  templateUrl: './shift.component.html',
  styleUrls: ['./shift.component.scss'],
})
export class ShiftComponent implements OnInit {
  guid: string;
  loaded = false;
  shift: Shift;
  authorized: boolean;

  constructor(private route: ActivatedRoute,
              private shiftService: ShiftService,
              public dialog: MatDialog,
              private _location: Location,
              private toastr: ToastrService,
              private userService: UserService,
              private breadcrumbService: BreadcrumbService) {


    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
  }


  async ngOnInit(): Promise<void> {
    this.authorized = this.userService.userIsProjectAdminFrontEnd();
    await this.shiftService.getShift(this.guid).then(shift => {
      if (shift) {
        this.shift = shift;

        const previous: Breadcrumb = new Breadcrumb('Shift overzicht', '/manage/shifts/' + this.shift.project.id);
        const current: Breadcrumb = new Breadcrumb('Shift', null);

        const breadcrumbs: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.managecrumb, previous, current];
        this.breadcrumbService.replace(breadcrumbs);
        this.loaded = true;
      }
    });
  }

  delete() {
    const message = 'Weet je zeker dat je deze shift wilt verwijderen?';
    const dialogData = new ConfirmDialogModel('Bevestig verwijderen', message, 'ConfirmationInput', null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult === true) {
        this.shiftService.deleteShift(this.guid).then(res => {
          if (res) {
            this._location.back();
          }
        });
      }
    });
  }

  edit() {
    const dialogRef = this.dialog.open(EditShiftComponent, {
      width: '500px',
      height: '600px',
      data: {
        shift: this.shift,
        projectGuid: this.shift.project.id
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res !== 'false') {
        this.toastr.success('Shift is gewijzigd');
        this.shift = res;
      }
    });
  }
}
