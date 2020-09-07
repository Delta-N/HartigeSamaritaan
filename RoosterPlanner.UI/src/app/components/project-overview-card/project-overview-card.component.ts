import { Component, OnInit, Input} from '@angular/core';

@Component({
  selector: 'app-project-overview-card',
  templateUrl: './project-overview-card.component.html',
  styleUrls: ['./project-overview-card.component.scss']
})
export class ProjectOverviewCardComponent implements OnInit {
  @Input() projects:any;

  constructor() { }

  ngOnInit(): void {
  }

  addProject() {
  window.alert("Deze functie moet nog geschreven worden...")
  }
}
