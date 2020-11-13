import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, ParamMap, Router} from "@angular/router";
import {MatDialog} from "@angular/material/dialog";
import {ToastrService} from "ngx-toastr";
import {UserService} from "../../services/user.service";
import {ConfirmDialogComponent, ConfirmDialogModel} from "../../components/confirm-dialog/confirm-dialog.component";
import {CategoryService} from "../../services/category.service";
import {Category} from "../../models/category";
import {AddCategoryComponent} from "../../components/add-category/add-category.component";

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {

  guid: string;
  category: Category;
  isAdmin: boolean = false;
  loaded: boolean = false;


  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private userService: UserService,
    private categoryService: CategoryService,
    private router: Router) {
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.categoryService.getCategory(this.guid).then(response => {
      this.category = response;
      this.loaded = true;
    })
  }

  edit() {
    const dialogRef = this.dialog.open(AddCategoryComponent, {
      width: '500px',
      data: {
        modifier: 'wijzigen',
        category: this.category,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result.status == 200) {
        this.toastr.success(result.body.name + " is gewijzigd")
        this.category = result.body;
      }
    });
  }

  delete() {
    const message = "Weet je zeker dat je deze category wilt verwijderen? Hiermee verwijder je ook alle taken die hieraan hangen" //set null instellen
    const dialogData = new ConfirmDialogModel("Bevestig verwijderen", message, "ConfirmationInput");
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: "400px",
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(dialogResult => {
      if (dialogResult === true) {
        this.categoryService.deleteCategory(this.guid).then(response => {
          if (response.status == 200) {
            this.router.navigateByUrl("/tasks")
          }
        })
      }
    })
  }
}
