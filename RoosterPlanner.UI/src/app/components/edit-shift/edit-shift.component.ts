import {Component, Inject, OnInit} from '@angular/core';
import {Task} from "../../models/task";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {TaskService} from "../../services/task.service";
import {Shift} from "../../models/shift";
import {ShiftService} from "../../services/shift.service";

@Component({
  selector: 'app-edit-shift',
  templateUrl: './edit-shift.component.html',
  styleUrls: ['./edit-shift.component.scss']
})
export class EditShiftComponent implements OnInit {
  checkoutForm: any;

  taskControl: FormControl;
  startTimeControl: FormControl;
  endTimeControl: FormControl;
  participantsRequiredControl: FormControl;

  shift: Shift;
  tasks: Task[];
  projectGuid: string;


  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<EditShiftComponent>,
              private taskService: TaskService,
              private shiftService: ShiftService) {
    data.shift != null ? this.shift = data.shift : this.shift = new Shift();
    data.projectGuid != null ? this.projectGuid = data.projectGuid : this.projectGuid = null;

    this.taskControl = new FormControl('', Validators.required);
    this.startTimeControl = new FormControl(this.shift.startTime != null ? this.shift.startTime : '', Validators.required);
    this.endTimeControl = new FormControl(this.shift.endTime != null ? this.shift.endTime : '', Validators.required);
    this.participantsRequiredControl = new FormControl(this.shift.participantsRequired != null ? this.shift.participantsRequired : '', Validators.required);

    this.checkoutForm = this.formBuilder.group({
      id: [this.shift != null ? this.shift.id : null],
      task: this.taskControl,
      start: this.startTimeControl,
      end: this.endTimeControl,
      par: this.participantsRequiredControl,
    })
  }


  ngOnInit(): void {
    this.taskService.getAllProjectTasks(this.projectGuid).then(res => {
      this.tasks = res;
      if (this.shift.task) {
        this.taskControl.setValue(this.tasks.find(t => t.name == this.shift.task.name))
      }
    })
  }

  timeBefore(startTime: string, endTime: string): boolean {
    let start: Date = new Date(null, null, null, parseInt(startTime.substring(0, 2)), parseInt(startTime.substring(3, 5)));
    let end: Date = new Date(null, null, null, parseInt(endTime.substring(0, 2)), parseInt(endTime.substring(3, 5)));
    return end.valueOf() - start.valueOf() >= 0
  }

  save(value: any) {
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error("Niet allevelden zijn correct ingevuld")
    } else {
      if (!this.timeBefore(value.start, value.end)) {
        this.toastr.error("Starttijd moet voor eindtijd komen")
        return;
      }
      if (value.par <= 0) {
        this.toastr.error("Een shift heeft minimaal 1 vrijwilliger nodig")
        return;
      }
      this.shift.participantsRequired = value.par;
      this.shift.startTime = value.start;
      this.shift.endTime = value.end;
      this.shift.task = value.task;

      this.shiftService.updateShift(this.shift).then(res => {
        if (res) {
          this.dialogRef.close(res)
        } else {
          this.toastr.error("Fout tijdens het updaten van de shift")
        }
      })
    }
  }

  close() {
    this.dialogRef.close('false')
  }

  adjustZindex(input: number) {
    const element = document.getElementsByClassName("cdk-overlay-container") as HTMLCollectionOf<HTMLElement>;
    const overlay = element[0]
    if (element) {
      if (input === 0) {
        overlay.style.zIndex = "0";
      }
      if (input === 1) {
        overlay.style.zIndex = "1";
      }
    }
  }
}
