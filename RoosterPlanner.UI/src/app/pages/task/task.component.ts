import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, ParamMap, Router, RouterLink} from "@angular/router";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {TaskService} from "../../services/task.service";
import {Task} from "../../models/task";
import {UploadService} from "../../services/upload.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {AddRequirementComponent} from "../../components/add-requirement/add-requirement.component";
import {Requirement} from "../../models/requirement";
import {faPlusCircle, faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';
import {MatCard} from "@angular/material/card";
import {MaterialModule} from "../../modules/material/material.module";
import {AdminModule} from "../../modules/admin/admin.module";


@Component({
  selector: 'app-task',
  templateUrl: './task.component.html',
  standalone: true,
  imports: [
    MaterialModule,
    RouterLink,
    AdminModule
  ],
  styleUrls: ['./task.component.scss']
})
export class TaskComponent implements OnInit {
  circleIcon = faPlusCircle
  editIcon = faEdit
  deleteIcon = faTrashAlt
  guid: string | null;
  task: Task;
  isAdmin: boolean = false;
  loaded: boolean = false;

  displayRequirements: Requirement[];
  requirementCardStyle = 'card';
  requirementExpandbtnDisabled: boolean = true;
  itemsPerCard = 5;
  reasonableMaxInteger = 10000;


  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private userService: UserService,
    private taskService: TaskService,
    private router: Router,
    private uploadService: UploadService,
    private breadcrumbService: BreadcrumbService) {

    this.breadcrumbService.backcrumb()
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');

      this.getTask();
    });

  }

  getTask() {
    this.taskService.getTask(this.guid).then(response => {
      if(response) {
        this.task = response;
        this.displayRequirements = this.task.requirements.slice(0, this.itemsPerCard)
        if (this.task.requirements.length >= 5) {
          this.requirementExpandbtnDisabled = false;
        }
      }
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
      if (result && result !== 'false') {
        this.toastr.success(result.name + " is gewijzigd")
        this.task = result;

      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je deze taak wilt verwijderen?  Dit heeft veel gevolgen voor shiften. Je zult zelf handmatig een taak moeten toewijzen aan elke(!) shift."
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput", null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult === true) {
        this.taskService.deleteTask(this.guid).then(response => {
          if (response === true) {
            this.router.navigate(['admin/tasks']).then();
          }
        })
      }
    })
  }

  expandRequirementCard() {
    let leftElement = document.getElementById("left")
    let rightElement = document.getElementById("right")
    let expandedCard = document.getElementById("expanded-card");

    if (this.requirementCardStyle === 'expanded-card') {
      if (leftElement)
        leftElement.hidden = false;
      if (rightElement)
        rightElement.hidden = false;
      if (expandedCard)
        expandedCard.hidden = true;

      this.requirementCardStyle = 'card';
      this.itemsPerCard = 5;
      this.displayRequirements = this.task.requirements.slice(0, this.itemsPerCard);
    } else if (this.requirementCardStyle === 'card') {
      if (leftElement)
        leftElement.hidden = true;
      if (rightElement)
        rightElement.hidden = true;
      if (expandedCard)
        expandedCard.hidden = false;
      this.requirementCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.displayRequirements = this.task.requirements;
    }
  }

  modRequirement(modifier: string) {
    let requirement: Requirement = new Requirement();
    requirement.task = this.task
    const dialogRef = this.dialog.open(AddRequirementComponent, {
      width: '500px',
      data: {
        modifier: modifier,
        requirement: requirement
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(res => {
      if (res && res !== "false") {
        setTimeout(() => {
          this.getTask();
        }, 500);
      }
    })
  }
}
