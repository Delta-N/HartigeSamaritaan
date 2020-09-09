import {Component, OnInit, Inject} from '@angular/core';
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {MatCheckboxChange} from "@angular/material/checkbox";

@Component({
  selector: 'app-add-project',
  templateUrl: './add-project.component.html',
  styleUrls: ['./add-project.component.scss']
})
export class AddProjectComponent implements OnInit {

  constructor(@Inject(MAT_DIALOG_DATA) public data: any) {
  }

  ngOnInit(): void {

  }

  selectedProjects: string[] = [];

  OnChange(event: MatCheckboxChange) {
    if (event.checked) {
      this.addToArray(event.source.id)
    }
    if (!event.checked) {
      this.removeFromArray(event.source.id)
    }
  }

  addToArray(parameter: string) {
    var present = false;
    for (var i = 0; i < this.selectedProjects.length; i++) {
      if (this.selectedProjects[i] === parameter) {
        present = true;
      }
    }
    if (!present) {
      this.selectedProjects.push(parameter)
    }
  }

  removeFromArray(parameter: string) {
    const index = this.selectedProjects.indexOf(parameter)
    this.selectedProjects.splice(index, 1)
  }

  printArray() {
    for (var i = 0; i < this.selectedProjects.length; i++) {
      console.log(this.selectedProjects[i])
    }
  }


}
