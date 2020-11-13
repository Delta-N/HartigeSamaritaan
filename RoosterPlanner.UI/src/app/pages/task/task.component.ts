import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {TaskService} from "../../services/task.service";
import {Task} from "../../models/task";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  guid: string;
  task: Task;
  isAdmin: boolean = false;
  loaded: boolean = false;


  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private userService: UserService,
    private taskService: TaskService,
    private router:Router) {
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.taskService.getTask(this.guid).then(response => {
      this.task = response;
      this.loaded = true;
    })
  }

  edit() {
    const dialogRef = this.dialog.open(AddTaskComponent, {
      width: '500px',
      height: '600px',
      data: {
        modifier: 'wijzigen',
        task: this.task,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result.status == 200) {
        this.toastr.success(result.body.name + " is gewijzigd")
        this.task = result.body;

      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je deze taak wilt verwijderen?"
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput");
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult === true) {
        this.taskService.deleteTask(this.guid).then(response => {
          if(response.status==200){
            this.router.navigateByUrl("/admin")
          }
        })
      }
    })
  }
}
