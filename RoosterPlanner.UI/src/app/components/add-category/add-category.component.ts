import {Component, Inject, OnInit} from '@angular/core';
import {FormBuilder, Validators} from "@angular/forms";
import {Category} from "../../models/category";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {CategoryService} from "../../services/category.service";
import {EntityHelper} from "../../helpers/entity-helper";

@Component({
  selector: 'app-add-category',
  templateUrl: './add-category.component.html',
  styleUrls: ['./add-category.component.scss']
})
export class AddCategoryComponent implements OnInit {
  category: Category;
  updatedCategory: Category;

  checkoutForm;
  modifier: string = 'toevoegen';

  constructor(@Inject(MAT_DIALOG_DATA) public data: any,
              private formBuilder: FormBuilder,
              private toastr: ToastrService,
              public dialogRef: MatDialogRef<AddCategoryComponent>,
              private categoryService: CategoryService) {
    data.category != null ? this.category = data.category : this.category = new Category();

    this.modifier = data.modifier;

    this.checkoutForm = this.formBuilder.group({
      id: [this.category.id != null ? this.category.id : EntityHelper.returnEmptyGuid()],
      code: [this.category.code != null ? this.category.code : '', Validators.required],
      name: [this.category.name != null ? this.category.name : '', Validators.required],
    })
  }

  ngOnInit(): void {
  }

  saveCategory(value: Category) {
    this.updatedCategory = value;
    if (this.checkoutForm.status === 'INVALID') {
      this.toastr.error("Niet alle velden zijn correct ingevuld")
    } else {
      if (this.modifier === 'toevoegen') {
        this.categoryService.postCategory(this.updatedCategory).then(response => {
          this.dialogRef.close(response)
        });
      } else if (this.modifier === 'wijzigen') {
        this.categoryService.updateCategory(this.updatedCategory).then(response => {
          this.dialogRef.close(response)
        });

      }
    }
  }

  close() {
    this.dialogRef.close('false')
  }
}
