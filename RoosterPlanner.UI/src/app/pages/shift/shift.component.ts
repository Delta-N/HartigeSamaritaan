import {Component, OnInit} from '@angular/core';
import {FormBuilder, FormControl, Validators} from "@angular/forms";
import { Task } from 'src/app/models/task';
import {TaskService} from "../../services/task.service";
import {ActivatedRoute, ParamMap} from "@angular/router";

@Component({
  selector: 'app-shift',
  templateUrl: './shift.component.html',
  styleUrls: ['./shift.component.scss']
})
export class ShiftComponent implements OnInit {
  guid: string;
  loaded: boolean = false;
  title: string = "Shift toevoegen";
  taskControl: FormControl;
  tasks:Task[];
  checkoutForm;
  requiredParticipants: number;
  model: any;


  constructor(
    private taskService:TaskService,
    private route: ActivatedRoute,
    private formBuilder: FormBuilder,
  ) {

    this.taskControl=new FormControl('',Validators.required);
    this.checkoutForm = this.formBuilder.group({
      task: this.taskControl,
    })
  }

  ngOnInit(): void {
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.taskService.getAllProjectTasks(this.guid).then(tasks=>{
      this.tasks=tasks;
      this.loaded=true;
    });
  }

}
