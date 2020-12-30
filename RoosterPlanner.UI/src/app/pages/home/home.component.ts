import {Component, OnInit} from '@angular/core';
import {MatDialog} from "@angular/material/dialog";
import {AddProjectComponent} from "../../components/add-project/add-project.component";
import {Project} from "../../models/project";
import {ProjectService} from "../../services/project.service";
import {Participation} from "../../models/participation";
import {UserService} from "../../services/user.service";
import {User} from "../../models/user";
import {ParticipationService} from "../../services/participation.service";
import {ToastrService} from "ngx-toastr";
import {EntityHelper} from "../../helpers/entity-helper";
import {ChangeProfileComponent} from "../../components/change-profile/change-profile.component";
import {UploadService} from "../../services/upload.service";
import {Document} from "../../models/document";
import * as moment from "moment"
import {AcceptPrivacyPolicyComponent} from "../../components/accept-privacy-policy/accept-privacy-policy.component";
import {JwtHelper} from "../../helpers/jwt-helper";

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {

  loaded: boolean = false;
  participations: Participation[] = [];
  currentUser: User;
  selectedProjects: Project[];
  PP: Document;


  constructor(public dialog: MatDialog,
              private projectService: ProjectService,
              private userService: UserService,
              private participationService: ParticipationService,
              private toastr: ToastrService,
              private uploadService: UploadService
  ) {
  }

  async ngOnInit(): Promise<void> {
    await this.uploadService.getPP().then(res => {
      if (res)
        this.PP = res;
    })

    const idToken = JwtHelper.decodeToken(sessionStorage.getItem('msal.idtoken'));
    await this.userService.getUser(idToken.oid).then(async user => {
      if (user) {
        this.currentUser = user
        this.getParticipations().then(() => this.loaded = true)
      }
    });

    if (this.PP && (!this.currentUser.termsOfUseConsented || moment(this.PP.lastEditDate) > moment(this.currentUser.termsOfUseConsented)))
      this.promptPPAccept();
  }


  promptPPAccept() {
    const dialogRef = this.dialog.open(AcceptPrivacyPolicyComponent, {
      width: '95vw',
      height:'95vh',
      data: this.PP
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result && result === 'true') {
        this.currentUser.termsOfUseConsented = moment().subtract(moment().utcOffset(), "minutes").toDate().toISOString()
        this.userService.updateUser(this.currentUser)
      }
    });
  }


  async getParticipations() {
    await this.participationService.getParticipations(this.currentUser.id).then(response => {
      this.participations = response;
      this.participations.sort((a, b) => a.project.name.toLowerCase() > b.project.name.toLowerCase() ? 1 : -1);
    })
  }

  async addParticipation() {
    let projects: Project[] = [];
    let userCheckedProfile: boolean = false;
    this.toastr.warning("Controleer je profiel en vul deze eventueel aan.")

    const dialogRef = this.dialog.open(ChangeProfileComponent, {
      width: '500px',
      data: this.currentUser
    });
    dialogRef.disableClose = true;

    dialogRef.afterClosed().subscribe(async result => {
      if (result != null) {
        userCheckedProfile = true;
      }

      if (userCheckedProfile) {
        await this.projectService.getActiveProjects().then(async response => {
            projects = response;
            projects.forEach(pro => {
              this.participations.forEach(par => {
                if (pro.id == par.project.id) {
                  projects = projects.filter(obj => obj !== pro);
                }
              })
            })

            let dialogRef = this.dialog.open(AddProjectComponent, {
              data: projects,
              width: '350px',
            });
            dialogRef.disableClose = true;
            dialogRef.afterClosed().subscribe(async result => {

              if (result !== 'false') {
                this.selectedProjects = result;
                for (const project of this.selectedProjects) {
                  let participation: Participation = new Participation();
                  participation.id = EntityHelper.returnEmptyGuid();
                  participation.person = this.currentUser;
                  participation.project = project
                  await this.participationService.postParticipation(participation).then(res => {
                    if (res) {
                      this.participations.push(res)
                    }
                  });
                }
              }
            })
          }
        )
      } else {
        this.toastr.warning("Controleer eerst je profiel")
      }
    })
  }
}
