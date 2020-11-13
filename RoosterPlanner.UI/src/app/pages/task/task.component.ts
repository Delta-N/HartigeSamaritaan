import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, ParamMap} from "@angular/router";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";

@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  guid: string;
  task: any; //aanpassen zodra task model eris
  isAdmin: boolean = false;


  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private userService: UserService) {
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    //placeholder
    let task: any = {};
    task.id = "12"
    task.name = "Sous chef"
    task.category = "Koken"
    task.instruction = null
    task.instruction = "http://test.com"
    task.description = "Het is belangrijk dat deze persoon niet alleen verstand heeft van koken maar ook leiding geven. Deze persoon werkt direct onder de chef en is eigenlijk..."

    this.task = task;
    //placeholder^
  }

  edit() {
    const dialogRef = this.dialog.open(AddTaskComponent, {
      width: '500px',
      height: '500px',
      data: {
        modifier: 'wijzigen',
        task: this.task,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
        this.toastr.success(result.name + " is gewijzigd")
        //this.task=result();
        console.log(result)
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
        /*    await this.taskService.deleteTask(this.guid).then(response=>{
           handle response
          })*/
        console.log("taak is verwijderd")
      }
    })
  }
}
