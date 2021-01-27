import {Component, OnInit} from '@angular/core';
import {Requirement} from "../../models/requirement";
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {RequirementService} from "../../services/requirement.service";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";
import {UserService} from "../../services/user.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {AddRequirementComponent} from "../../components/add-requirement/add-requirement.component";
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-requirement',
  templateUrl: './requirement.component.html',
  styleUrls: ['./requirement.component.scss']
})
export class RequirementComponent implements OnInit {
  deleteIcon = faTrashAlt;
  editIcon = faEdit;
  guid: string;
  requirement: Requirement;
  isAdmin: boolean = false;
  loaded: boolean = false;

  constructor(private route: ActivatedRoute,
              public dialog: MatDialog,
              private toastr: ToastrService,
              private requirementService: RequirementService,
              private router: Router,
              private breadcrumbService: BreadcrumbService,
              private userService: UserService) {


  }

  async ngOnInit(): Promise<void> {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe(async (params: ParamMap) => {
      this.guid = params.get('id');

      await this.requirementService.getRequirement(this.guid).then(res => {
        if (res) {
          this.requirement = res;

          let breadcrumb: Breadcrumb = new Breadcrumb('Requirement', null);
          let takencrumb: Breadcrumb = new Breadcrumb(this.requirement.task.name, "task/" + this.requirement.task.id);
          let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb,
            takencrumb, breadcrumb];

          this.breadcrumbService.replace(array);
          this.loaded = true;
        }
      })
    });
  }

  edit() {
    const dialogRef = this.dialog.open(AddRequirementComponent, {
      width: '500px',
      data: {
        modifier: 'wijzigen',
        requirement: this.requirement,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result && result !== 'false') {
        this.toastr.success("De requirement is gewijzigd")
        this.loaded = false;
        this.requirementService.getRequirement(this.guid).then(res => {
          if (res)
            this.requirement = res;
        })
        this.loaded = true;
      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je deze requirement wilt verwijderen?"
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput", null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult === true) {
        this.requirementService.deleteRequirement(this.guid).then(response => {
          if (response === true) {
            window.history.back()
          }
        })
      }
    })
  }
}
