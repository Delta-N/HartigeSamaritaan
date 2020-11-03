import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, Validators} from '@angular/forms';
import {Validator} from "../../helpers/validators"
import {User} from "../../models/user";
import {MAT_DIALOG_DATA} from "@angular/material/dialog";
import {UserService} from "../../services/user.service";

@Component({
  selector: 'app-change-profile',
  templateUrl: './change-profile.component.html',
  styleUrls: ['./change-profile.component.scss']
})
export class ChangeProfileComponent implements OnInit {
  user: User;
  updateUser: User;
  checkoutForm;

  constructor(@Inject(MAT_DIALOG_DATA) public data: any, private formBuilder: FormBuilder, private userService: UserService) {
    this.user = data;
    this.checkoutForm = this.formBuilder.group({
      id: this.user.id,
      firstName: [this.user.firstName, Validators.required],
      lastName: [this.user.lastName, Validators.required],
      dateOfBirth: [this.user.dateOfBirth, Validators.compose([Validators.required, Validator.date])],
      streetAddress: [this.user.streetAddress, Validators.required],
      postalCode: [this.user.postalCode, Validator.postalCode],
      city: [this.user.city, Validators.required],
      phoneNumber: [this.user.phoneNumber, Validator.phoneNumber],
    })
  }

  ngOnInit(): void {
  }

  saveProfile(value: User) {
    this.updateUser = value
    if (this.checkoutForm.status === 'INVALID') {
      window.alert("Not all fields are correct");
    } else {
      this.userService.updateUser(this.updateUser).then();
    }
  }

}
