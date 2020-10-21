import { Component, OnInit , Input} from '@angular/core';
import {Project} from "../../models/project";

@Component({
  selector: 'app-project-card',
  templateUrl: './project-card.component.html',
  styleUrls: ['./project-card.component.scss']
})
export class ProjectCardComponent implements OnInit {

  constructor() { }

  @Input() project: Project;

  ngOnInit(): void {
  }
  removeProject(id: any) {
    //todo
    window.alert("Deze functie moet nog gemaakt worden "+ id)
  }
}
