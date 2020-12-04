import {Component, OnInit} from '@angular/core';
import {Task} from "../../models/task";
import {MatDialog} from "@angular/material/dialog";
import {Router} from "@angular/router";
import {ToastrService} from "ngx-toastr";
import {TaskService} from "../../services/task.service";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {Category} from "../../models/category";
import {CategoryService} from "../../services/category.service";
import {AddCategoryComponent} from "../../components/add-category/add-category.component";
import {BreadcrumbService} from "../../services/breadcrumb.service";
import {Breadcrumb} from "../../models/breadcrumb";

@Component({
  selector: 'app-all-tasks',
  templateUrl: './all-tasks.component.html',
  styleUrls: ['./all-tasks.component.scss']
})
export class AllTasksComponent implements OnInit {
  loaded: boolean = false;
  itemsPerCard: number = 5;

  reasonableMaxInteger: number = 10000; //aanpassen na 10k projecten/admins ;)
  tasks: Task[] = [];
  taskCardStyle = 'card';
  tasksElementHeight: number;

  categoryCardStyle = 'card';
  categoryElementHeight: number;
  categories: Category[];
  tasksExpandbtnDisabled: boolean = true;
  categoryExpandbtnDisabled: boolean = true;


  constructor(public dialog: MatDialog,
              private router: Router,
              private toastr: ToastrService,
              private taskService: TaskService,
              private categoryService: CategoryService,
              private breadcrumbService: BreadcrumbService) {
    let tempcrumb:Breadcrumb= this.breadcrumbService.takencrumb;
    tempcrumb.url=null;
    let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.admincrumb,tempcrumb ];
    this.breadcrumbService.replace(array);
  }

  ngOnInit(): void {
    this.getCategories(0, this.itemsPerCard).then()
    this.getTasks(0, this.itemsPerCard).then(() => this.loaded = true)
  }

  async getTasks(offset: number, pageSize: number) {
    await this.taskService.getAllTasks(offset, pageSize).then(tasks => {
      this.tasks = tasks;
      this.tasks.sort((a, b) => a.name > b.name ? 1 : -1);
      if (this.tasks.length >= 5) {
        this.tasksExpandbtnDisabled = false;
      }
    })

  }

  async getCategories(offset: number, pageSize: number) {
    await this.categoryService.getAllCategory().then(categories => {
      this.categories = categories;
      this.categories.sort((a, b) => a.name > b.name ? 1 : -1)
      if (this.categories.length >= 5) {
        this.categoryExpandbtnDisabled = false;
      }
      this.categories = this.categories.splice(offset, pageSize)
    })
  }


  modTask(modifier: string) {
    if (modifier === 'add') {
      const dialogRef = this.dialog.open(AddTaskComponent, {
        width: '500px',
        height: '600px',
        data: {
          modifier: 'toevoegen',
        }
      });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(async result => {
        if (result !== 'false') {
          await this.getTasks(0, this.itemsPerCard).then()
          this.toastr.success(result.name + " is toegevoegd")
        }
      });
    }
  }

  modCategory(modifier: string) {
    if (modifier === 'add') {
      const dialogRef = this.dialog.open(AddCategoryComponent, {
        width: '500px',
        data: {
          modifier: 'toevoegen',
        }
      });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        if (result !== 'false') {
          this.getCategories(0, this.itemsPerCard).then()
          this.toastr.success(result.body.name + " is toegevoegd")
        }
      });
    }
  }

  expandTaskCard() {
    if (this.taskCardStyle == 'expanded-card') {
      document.getElementById("categoryCard").hidden = false;
      this.taskCardStyle = 'card';
      this.itemsPerCard = 5;
      this.tasks = this.tasks.slice(0, this.itemsPerCard);
    } else {
      document.getElementById("categoryCard").hidden = true;
      this.taskCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getTasks(0, this.itemsPerCard).then(() => {
        this.tasksElementHeight = (this.tasks.length * 48);
      })

    }
  }

  expandCategoryCard() {
    if (this.categoryCardStyle == 'expanded-card') {
      document.getElementById("taskCard").hidden = false;
      this.categoryCardStyle = 'card';
      this.itemsPerCard = 5;
      this.categories = this.categories.slice(0, this.itemsPerCard);
    } else {
      document.getElementById("taskCard").hidden = true;
      this.categoryCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getCategories(0, this.itemsPerCard).then(() => {
        this.categoryElementHeight = (this.categories.length * 48);
      })
    }
  }
}
