import {Component, OnInit} from '@angular/core';
import {ActivatedRoute, ParamMap, Router} from '@angular/router';
import {MatDialog} from '@angular/material/dialog';
import {ToastrService} from 'ngx-toastr';
import {UserService} from '../../services/user.service';
import {ConfirmDialogComponent, ConfirmDialogModel} from '../../components/confirm-dialog/confirm-dialog.component';
import {CategoryService} from '../../services/category.service';
import {Category} from '../../models/category';
import {AddCategoryComponent} from '../../components/add-category/add-category.component';
import {BreadcrumbService} from '../../services/breadcrumb.service';
import {Breadcrumb} from '../../models/breadcrumb';
import {faEdit, faTrashAlt} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss']
})
export class CategoryComponent implements OnInit {
  editIcon = faEdit;
  deleteIcon = faTrashAlt;
  guid: string;
  category: Category;
  isAdmin = false;
  loaded = false;


  constructor(
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private toastr: ToastrService,
    private userService: UserService,
    private categoryService: CategoryService,
    private router: Router,
    private breadcrumbService: BreadcrumbService) {

    const breadcrumb: Breadcrumb = new Breadcrumb('Categorie', null);
    const takencrumb: Breadcrumb = new Breadcrumb('Taken', 'admin/tasks');

    const array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb,
      this.breadcrumbService.admincrumb, takencrumb, breadcrumb];

    this.breadcrumbService.replace(array);
  }

  ngOnInit(): void {
    this.isAdmin = this.userService.userIsAdminFrontEnd();
    this.route.paramMap.subscribe((params: ParamMap) => {
      this.guid = params.get('id');
    });
    this.categoryService.getCategory(this.guid).then(response => {
      this.category = response;
      this.loaded = true;
    });
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
    dialogRef.afterClosed().subscribe(async result => {
      if (result && result !== 'false') {
        this.category = result;
        this.toastr.success(result.name + ' is gewijzigd');
      }
    });
  }

  delete() {
    const message = 'Weet je zeker dat je deze category wilt verwijderen?';
    const dialogData = new ConfirmDialogModel('Bevestig verwijderen', message, 'ConfirmationInput', null);
    const dialogRef = this.dialog.open(ConfirmDialogComponent, {
      maxWidth: '400px',
      data: dialogData
    });
    dialogRef.afterClosed().subscribe(async dialogResult => {
      if (dialogResult === true) {
        await this.categoryService.deleteCategory(this.guid).then(async response => {
          if (response) {
            this.router.navigate(['admin/tasks']).then();
          }
        });
      }
    });
  }
}
