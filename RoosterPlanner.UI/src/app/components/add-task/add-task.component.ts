import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import {ToastrService} from "ngx-toastr";
import {EntityHelper} from "../../helpers/entity-helper";
import {Task} from "../../models/task";
import {Category} from "../../models/category";
import {TaskService} from "../../services/task.service";

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.scss']
})
export class AddTaskComponent implements OnInit {
  categoryControl = new FormControl('', Validators.required);
  task: Task;
  updatedTask: Task;

  checkoutForm;
  modifier: string = 'toevoegen';
  categories: any = [];


  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<AddTaskComponent>,
              private taskService:TaskService) {
    data.task != null ? this.task = data.task : this.task=new Task();

    this.modifier = data.modifier;
    let category1:Category=new Category()
    let category2:Category=new Category()
    let category3:Category=new Category()
    category1.code='KOK'
    category1.name='Koken'
    category2.code='BED'
    category2.name='Bediening'
    category3.code='OVR'
    category3.name='Overige'
    this.categories.push(category1)
    this.categories.push(category2)
    this.categories.push(category3)

    this.checkoutForm = this.formBuilder.group({
      id: [this.task.id != null ? this.task.id : EntityHelper.returnEmptyGuid()],
      name: [this.task.name != null ? this.task.name : '', Validators.required],
      category: this.categoryControl,
      description: [this.task.description != null ? this.task.description : '']
    })
  }

  ngOnInit(): void {
  }

  saveTask(value: Task) {
    this.updatedTask = value;
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error("Niet alle velden zijn correct ingevuld")
    } else {
      if (this.modifier === 'toevoegen') {
         this.taskService.postTask(this.updatedTask).then(response => {
        this.dialogRef.close(response)
        });
      } else if (this.modifier === 'wijzigen') {
         this.taskService.updateTask(this.updatedTask).then(response => {
        this.dialogRef.close(response)
        });

      }
    }
  }

  close() {
    this.dialogRef.close('false')
  }

  uploadInstructions() {

  }
}
