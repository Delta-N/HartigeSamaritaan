import {Component, OnInit} from '@angular/core';
import {Project} from "../../models/project";
import {UserService} from "../../services/user.service";
import {ToastrService} from "ngx-toastr";
import {EmailService} from "../../services/email.service";

@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.scss']
})
export class ManageComponent implements OnInit {
  loaded: boolean = false;
  projects: Project[] = [];

  constructor(private userService: UserService,
              private toastr: ToastrService,
              private emailService: EmailService) {
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

  async requestAvailability(id: string) {
    if (id) {
      this.toastr.warning("Berichten aan het versturen...")
      await this.emailService.requestAvailability(id).then(res=>{
        if(res){
          this.toastr.success("Berichten verzonden")
        }
      })
    }
  }

  async sendSchedule(id: string) {
    if (id) {
      this.toastr.warning("Berichten aan het versturen...")
      await this.emailService.sendSchedule(id).then(res => {
        if (res) {
          this.toastr.success("Rooster verzonden")
        }
      })
    }
  }
}
