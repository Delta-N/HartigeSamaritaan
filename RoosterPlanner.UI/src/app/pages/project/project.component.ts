import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Project} from "../../models/project";
import {DateConverter} from "../../helpers/date-converter";
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {
  guid: string;
  project: Project;
  loaded: boolean = false;
  startDate: string;
  endDate: string;


  constructor(private projectService: ProjectService, private route: ActivatedRoute, private toastr: ToastrService, public dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    await this.projectService.getProject(this.guid).then(listOfProjects => {
      if (listOfProjects[0] != null) {
        this.project = listOfProjects[0];
        this.project.startDate != null ? this.startDate = DateConverter.toReadableString(this.project.startDate.toString()) : this.startDate = null;
        this.project.endDate != null ? this.endDate = DateConverter.toReadableString(this.project.endDate.toString()) : this.endDate = null;
        this.loaded = true;
        console.log(this.project.websiteUrl)
      }
    })
  }

  async getProject() {
    await this.projectService.getProject(this.guid).then(project=>this.project=project[0])
  }


  editProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
      data: {
        createProject: false,
        project: this.project
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
        if (result === 'closed') {
          setTimeout(x => {
            this.toastr.success("Dit project is gesloten")
            this.getProject().then();
          }, 500);
        } else {
          setTimeout(x => {
            this.toastr.success(result + " is gewijzigd")
            this.getProject().then();
          }, 500);
        }

      }
    });
  }

  closeProject() {
  }
}
