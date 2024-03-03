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
import {AddCertificatetypeComponent} from "../../components/add-certificatetype/add-certificatetype.component";
import {CertificateService} from "../../services/certificate.service";
import {CertificateType} from "../../models/CertificateType";
import {faPlusCircle,} from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-all-tasks',
  templateUrl: './all-tasks.component.html',
  styleUrls: ['./all-tasks.component.scss']
})
export class AllTasksComponent implements OnInit {
  circleIcon = faPlusCircle
  loaded: boolean = false;
  itemsPerCard: number = 5;

  reasonableMaxInteger: number = 10000; //aanpassen na 10k projecten/admins ;)
  tasks: Task[] | null = [];
  taskCardStyle = 'card';
  tasksExpandbtnDisabled: boolean = true;


  categoryCardStyle = 'card';
  categories: Category[];
  categoryExpandbtnDisabled: boolean = true;

  certificateCardStyle = 'card';
  certificateExpandbtnDisabled: boolean = true;
  certificatesTypes: CertificateType[];


  constructor(public dialog: MatDialog,
              private router: Router,
              private toastr: ToastrService,
              private taskService: TaskService,
              private categoryService: CategoryService,
              private certificiateService: CertificateService,
              private breadcrumbService: BreadcrumbService) {
    let takencrumb: Breadcrumb = new Breadcrumb("Taken", null);

    let array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb, this.breadcrumbService.admincrumb, takencrumb];
    this.breadcrumbService.replace(array);
  }

  ngOnInit(): void {
    this.getCategories(0, this.itemsPerCard).then()
    this.getCertificates(0, this.itemsPerCard).then()
    this.getTasks(0, this.itemsPerCard).then(() => this.loaded = true)
  }

  async getTasks(offset: number, pageSize: number) {
    await this.taskService.getAllTasks(offset, pageSize).then(tasks => {
      this.tasks = tasks;
      this.tasks?.sort((a, b) => a.name > b.name ? 1 : -1);
      let length = this.tasks?.length ?? 0;
      if (length >= 5) {
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

  async getCertificates(offset: number, pageSize: number) {
    await this.certificiateService.getAllCertificateTypes().then(res => {
      if (res)
        this.certificatesTypes = res;
      this.certificatesTypes.sort((a, b) => a.name > b.name ? 1 : -1)
      if (this.certificatesTypes.length >= 5) {
        this.certificateExpandbtnDisabled = false;
      }
      this.certificatesTypes = this.certificatesTypes.splice(offset, pageSize)
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

  modCertificate(modifier: string) {
    if (modifier === 'add') {
      const dialogRef = this.dialog.open(AddCertificatetypeComponent, {
        width: '400px',
        data: {
          modifier: 'toevoegen',
        }
      });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        if (result !== 'false') {
          this.getCertificates(0, this.itemsPerCard).then()
          this.toastr.success(result.body.name + " is toegevoegd")
        }
      });
    }
  }

  expandTaskCard() {
    let categoryCardElement = document.getElementById("categoryCard")
    let certificateCardElement = document.getElementById("certificateCard")
    let element = document.getElementById("taskIcon")

    if (element) {
      if (this.taskCardStyle === 'expanded-card')
        element.innerText = "zoom_out_map"
      else
        element.innerText = "fullscreen_exit"
    }
    if (this.taskCardStyle == 'expanded-card') {
      if (categoryCardElement)
        categoryCardElement.hidden = false;
      if (certificateCardElement)
        certificateCardElement.hidden = false;
      this.taskCardStyle = 'card';
      this.itemsPerCard = 5;
      this.tasks = this.tasks?.slice(0, this.itemsPerCard) ??[];
    } else {
      if (categoryCardElement)
        categoryCardElement.hidden = true;
      if (certificateCardElement)
        certificateCardElement.hidden = true;
      this.taskCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getTasks(0, this.itemsPerCard)
    }
  }

  expandCategoryCard() {
    let taskCardElement = document.getElementById("taskCard")
    let certificateCardElement = document.getElementById("certificateCard")
    let element = document.getElementById("categoryIcon")
    if (element) {
      if (this.categoryCardStyle === 'expanded-card')
        element.innerText = "zoom_out_map"
      else
        element.innerText = "fullscreen_exit"
    }
    if (this.categoryCardStyle == 'expanded-card') {
      if (taskCardElement)
        taskCardElement.hidden = false;
      if (certificateCardElement)
        certificateCardElement.hidden = false;

      this.categoryCardStyle = 'card';
      this.itemsPerCard = 5;
      this.categories = this.categories.slice(0, this.itemsPerCard);
    } else {
      if (taskCardElement)
        taskCardElement.hidden = true;
      if (certificateCardElement)
        certificateCardElement.hidden = true;
      this.categoryCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getCategories(0, this.itemsPerCard)
    }
  }

  expandCertificateCard() {
    let taskCardElement = document.getElementById("taskCard")
    let categoryCardElement = document.getElementById("categoryCard")
    let element = document.getElementById("certificateIcon")
    if (element) {
      if (this.certificateCardStyle === 'expanded-card')
        element.innerText = "zoom_out_map"
      else
        element.innerText = "fullscreen_exit"
    }
    if (this.certificateCardStyle == 'expanded-card') {
      if (taskCardElement)
        taskCardElement.hidden = false;

      if (categoryCardElement)
        categoryCardElement.hidden = false;

      this.certificateCardStyle = 'card';
      this.itemsPerCard = 5;
      this.certificatesTypes = this.certificatesTypes.slice(0, this.itemsPerCard);
    } else {
      if (taskCardElement)
        taskCardElement.hidden = true;
      if (categoryCardElement)
        categoryCardElement.hidden = true;

      this.certificateCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getCertificates(0, this.itemsPerCard)
    }
  }
}
