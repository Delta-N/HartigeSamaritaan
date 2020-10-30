import {Component, OnInit, Input} from '@angular/core';
import {UserService} from "../../services/user.service";
import {ParticipationService} from "../../services/participation.service";
import {Participation} from "../../models/participation";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.scss']
})
export class ProjectCardComponent implements OnInit {
  isAdmin: boolean = false;

  constructor(private toastr: ToastrService,
              private userService: UserService,
              private participationService: ParticipationService) {
  }

  @Input() participation: Participation;

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
  }

  removeParticipation(participation: Participation) {
    this.participationService.deleteParticipation(participation).then(
      response => {
        if (response.body !== null) {
          window.location.reload();
        }
      }
    );
  }

  todo() {
    this.toastr.warning("Deze functie moet nog geschreven worden")
  }
}
