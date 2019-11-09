import { Component, OnInit } from '@angular/core';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { QuestionairAnswers } from '../../models/questionair-answers';

@Component({
  selector: 'app-questions',
  templateUrl: './questions.component.html',
  styleUrls: ['./questions.component.less']
})
export class QuestionsComponent implements OnInit {
  firstFormGroup: FormGroup;
  secondFormGroup: FormGroup;
  answers = new QuestionairAnswers();

  constructor(private formBuilder: FormBuilder) {}

  ngOnInit() {
    this.createForm();
  }

  private createForm() {
    this.firstFormGroup = this.formBuilder.group({
      refugee: ['1', Validators.required],
      azc: ['1', Validators.required],
      internship: ['1', Validators.required],
      driver: ['1', Validators.required]
    });
    this.secondFormGroup = this.formBuilder.group({
      barStaff: ['1', Validators.required],
      staff: ['1', Validators.required],
      host: ['1', Validators.required],
      cook: ['1', Validators.required]
    });
  }
}
