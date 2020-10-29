import {Component, OnInit, Input} from '@angular/core';
import {Project} from "../../models/project";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {ProjectService} from "../../services/project.service";

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.scss']
})
export class ProjectCardComponent implements OnInit {
  isAdmin: boolean = false;

  constructor(private toastr: ToastrService, private userService: UserService, private projectService: ProjectService) {
  }

  @Input() project: Project;

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
  }

  removeParticipation(guid: string) {
    this.toastr.warning("Deze functie moet nog gemaakt worden ")
  }
}
