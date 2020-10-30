import {Component, OnDestroy, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {ActivatedRoute, ParamMap} from "@angular/router";
import {Project} from "../../models/project";
import {DateConverter} from "../../helpers/date-converter";
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {ToastrService} from "ngx-toastr";
import {MatDialog} from "@angular/material/dialog";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-project',
  templateUrl: './project.component.html',
  styleUrls: ['./project.component.scss']
})
export class ProjectComponent implements OnInit {
  guid: string;
  project: Project;
  viewProject:Project;
  loaded: boolean = false;
  title: string;
  closeButtonText: string;
  isAdmin:boolean=false;


  constructor(private userService: UserService, private projectService: ProjectService, private route: ActivatedRoute, private toastr: ToastrService, public dialog: MatDialog) {
  }

  async ngOnInit(): Promise<void> {
    this.isAdmin= this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.getProject().then();

  }

  async getProject() {
    await this.projectService.getProject(this.guid).then(project => {
      if (project[0] != null) {
        this.project = project[0];
        this.viewProject=DateConverter.formatProjectDateReadable(this.project)
        this.title = this.viewProject.name;
        this.viewProject.closed?this.closeButtonText="Project openen":this.closeButtonText="Project sluiten";
        if (this.viewProject.closed) {
          this.title += " DIT PROJECT IS GESLOTEN"
        }
      }
      this.loaded = true;
    })
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

  async closeProject() {
    this.project.closed = !this.project.closed;
    await this.projectService.updateProject(this.project).then(response => {
      this.getProject().then();
      if(this.project.closed){
      this.toastr.success("Het project is gesloten");
      }else{this.toastr.success("Het project is geopend");}
    }, Error => this.toastr.error("Fout tijdens het sluiten van het project"));
  }
}
