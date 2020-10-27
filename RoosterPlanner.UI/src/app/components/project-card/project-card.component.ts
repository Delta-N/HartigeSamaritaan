import { Component, OnInit , Input} from '@angular/core';
import {Project} from "../../models/project";
import {ToastrService} from "ngx-toastr";

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.scss']
})
export class ProjectCardComponent implements OnInit {

  constructor(private toastr:ToastrService) { }

  @Input() project: Project;

  ngOnInit(): void {
  }
  removeProject(id: any) {
    this.toastr.warning("Deze functie moet nog gemaakt worden ")
  }
}
