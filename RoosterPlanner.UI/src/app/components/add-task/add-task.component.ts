import {Component, Inject, OnInit} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialogRef} from '@angular/material/dialog';
import {FormBuilder, FormControl, Validators} from '@angular/forms';
import {ToastrService} from 'ngx-toastr';
import {EntityHelper} from '../../helpers/entity-helper';
import {Task} from '../../models/task';
import {Category} from '../../models/category';
import {TaskService} from '../../services/task.service';
import {CategoryService} from '../../services/category.service';
import {UploadService} from '../../services/upload.service';
import {Document} from '../../models/document';
import {TextInjectorService} from '../../services/text-injector.service';

@Component({
  selector: 'app-add-task',
  templateUrl: './add-task.component.html',
  styleUrls: ['./add-task.component.scss']
})
export class AddTaskComponent implements OnInit {
  categoryControl: FormControl;
  colorControl: FormControl;
  task: Task;
  updatedTask: Task;

  checkoutForm;
  modifier = 'toevoegen';
  categories: Category[] = [];
  colors: string[] = TextInjectorService.colors;
  files: FileList;


  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<AddTaskComponent>,
              private taskService: TaskService,
              private categoryService: CategoryService,
              private uploadService: UploadService) {
    data.task != null ? this.task = data.task : this.task = new Task();

    this.modifier = data.modifier;
    this.categoryControl = new FormControl('', Validators.required);
    this.colorControl = new FormControl(this.task.color != null ? this.task.color : '', Validators.required);

    this.checkoutForm = this.formBuilder.group({
      id: [this.task.id != null ? this.task.id : EntityHelper.returnEmptyGuid()],
      name: [this.task.name != null ? this.task.name : '', Validators.required],
      category: this.categoryControl,
      color: this.colorControl,
      description: [this.task.description != null ? this.task.description : '']
    });
  }

  ngOnInit(): void {
    this.categoryService.getAllCategory().then(response => {
      this.categories = response;
      if (this.task.category != null) {
        this.categoryControl.setValue(this.categories.find(c => c.name == this.task.category.name));
      }
    });
  }

  async saveTask(value: Task) {
    this.updatedTask = value;
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error('Niet alle velden zijn correct ingevuld');
    } else {

      if (this.files && this.files[0]) {
        const formData = new FormData();
        formData.append(this.files[0].name, this.files[0]);


        let uri: string = null;
        await this.uploadService.uploadInstruction(formData).then(url => {
          if (url && url.path && url.path.trim().length > 0) {
            uri = url.path.trim();
          }
        });

        if (this.task.instruction) {
          await this.uploadService.deleteIfExists(this.task.instruction.documentUri).then(); // delete blob if exists
          this.task.instruction.documentUri = uri;
          await this.uploadService.updateDocument(this.task.instruction).then(res => {
            if (res) {
              this.updatedTask.instruction = res;
            }
          });
        } else {
          const document = new Document();
          document.name = 'instruction';
          document.documentUri = uri;
          await this.uploadService.postDocument(document).then(res => {
            if (res) {
              this.updatedTask.instruction = res;
            }
          });
        }

      } else {
        this.updatedTask.instruction = this.task.instruction;
      }


      if (this.modifier === 'toevoegen') {
        await this.taskService.postTask(this.updatedTask).then(response => {
          this.dialogRef.close(response);
        });
      } else if (this.modifier === 'wijzigen') {
        this.updatedTask.lastEditBy = this.task.lastEditBy;
        this.updatedTask.lastEditDate = this.task.lastEditDate;
        this.updatedTask.rowVersion = this.task.rowVersion;
        await this.taskService.updateTask(this.updatedTask).then(response => {
          this.dialogRef.close(response);
        });

      }
    }
  }

  close() {
    this.dialogRef.close('false');
  }

  uploadInstructions(files: FileList) {
    let correctExtention = true;
    for (let i = 0; i < files.length; i++) {
      if (files[i].name.substring(files[i].name.lastIndexOf('.') + 1) !== 'pdf') {
        this.toastr.warning('Er mogen alleen PDF instructies geupload worden');
        correctExtention = false;
      }
    }
    if (correctExtention) {
      this.files = files;
    }
  }
}
