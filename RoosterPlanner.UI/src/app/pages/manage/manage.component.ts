import {Component, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {UserService} from "../../services/user.service";
import {Manager} from "../../models/manager";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.scss']
})
export class ManageComponent implements OnInit {
  loaded: boolean = false;
  projects: Project[] = [];

  constructor(private userService: UserService,
              private toastr: ToastrService) {
  }

  async ngOnInit(): Promise<void> {
    let userId: string = this.userService.getCurrentUserId();
    await this.userService.getProjectsManagedBy(userId).then(res => {
      if (res != null) {
        res.forEach(m => this.projects.push(m.project))
      }
      this.loaded = true;
    })
  }

  todo() {
    this.toastr.warning("Deze functie moet nog geschreven worden")
  }
}
