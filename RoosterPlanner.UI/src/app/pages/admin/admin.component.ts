import {Component, OnInit} from '@angular/core';
import {ProjectService} from "../../services/project.service";
import {Project} from "../../models/project";
import {Router} from "@angular/router"
import {MatDialog} from '@angular/material/dialog';
import {CreateProjectComponent} from "../../components/create-project/create-project.component";
import {User} from "../../models/user";
import {UserService} from "../../services/user.service";
import {AddAdminComponent} from "../../components/add-admin/add-admin.component";
import {ToastrService} from "ngx-toastr";
import {AddTaskComponent} from "../../components/add-task/add-task.component";
import {Task} from "../../models/task";
import {Category} from "../../models/category";


@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.scss']
})
export class AdminComponent implements OnInit {
  projects: Project[] = [];
  loaded: boolean = false;
  administrators: User[] = []
  itemsPerCard: number = 5;

  projectCardStyle = 'card';
  projectsElementHeight: number;
  adminCardStyle = 'card';
  adminElementHeight: number;

  reasonableMaxInteger: number = 10000; //aanpassen na 10k projecten/admins ;)
  tasks:Task[]=[];
  taskCardStyle = 'card';
  private tasksElementHeight: number;

  constructor(public dialog: MatDialog,
              private projectService: ProjectService,
              private router: Router,
              private userService: UserService,
              private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.getProjects(0, this.itemsPerCard).then()
    this.getAdministrators(0, this.itemsPerCard).then(() => this.loaded = true)
    //this.getTasks

    let task:Task=new Task();
    let category:Category=new Category();
    category.code='KOK'
    category.name='Koken'
    task.id="12"
    task.name="Sous chef"
    task.category=category
    task.instruction=null
    task.instruction="http://test.com"
    task.description= "Het is belangrijk dat deze persoon niet alleen verstand heeft van koken maar ook leiding geven. Deze persoon werkt direct onder de chef en is eigenlijk..."
    this.tasks.push(task)

    let task2:Task=new Task();
    let category2:Category=new Category();
    task2.id="13"
    task2.name="Serveren"
    category2.code='BED'
    category2.name='Bediening'
    task2.category=category2
    task2.description= "een leuke beschrijving"
    this.tasks.push(task2)


    let task3:Task=new Task();
    let category3:Category=new Category()
    task3.id="14"
    task3.name="Klussen"
    category3.code='OVR'
    category3.name='Overige'
    task3.category=category3
    task3.description= "een leuke beschrijving"
    this.tasks.push(task3)
    this.tasks.push(task3)
    this.tasks.push(task3)
    this.tasks.push(task3)
  }

  async getProjects(offset: number, pageSize: number) {
    await this.projectService.getAllProjects(offset, pageSize).then(x => {
      this.projects = x;
      this.projects.sort((a, b) => a.startDate < b.startDate ? 1 : -1);
    });
  }

  async getAdministrators(offset: number, pageSize: number) {
    await this.userService.getAdministrators(offset, pageSize).then(x => {
      this.administrators = x;
      this.administrators.sort((a, b) => a.firstName > b.firstName ? 1 : -1);
    });
  }

  async getTasks(offset: number, pageSize: number) {
/*    await this.taskService.getTasks(offset,pageSize).then(tasks=>{
      this.tasks=tasks;
      this.tasks.sort((a, b) => a.name > b.name ? 1 : -1);
    })*/
  }


  addProject() {
    const dialogRef = this.dialog.open(CreateProjectComponent, {
      width: '500px',
      data: {
        createProject: true,
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(result => {
      if (result !== 'false') {
        setTimeout(() => {
          this.getProjects(0, this.itemsPerCard).then(() => {
            if (result != null) {
              this.toastr.success(result.name + " is toegevoegd als nieuw project")
            }
          })
        }, 1000);
      }
    });
  }

  async modAdmin(modifier: string) {
    let toastrString: string;
    let dataModifier: boolean;
    if (modifier === 'add') {
      toastrString = 'toegevoegd';
      dataModifier = true;
    } else if (modifier === 'remove') {
      await this.getAdministrators(0, this.reasonableMaxInteger)
      toastrString = 'verwijderd';
      dataModifier = false;
    }

    const dialogRef = this.dialog.open(AddAdminComponent, {
      width: '500px',
      data: {
        addAdminType: dataModifier,
        administrators: this.administrators
      }
    });
    dialogRef.disableClose = true;
    dialogRef.afterClosed().subscribe(async result => {
      if (result != null) {
        setTimeout(() => {
          this.getAdministrators(0, this.itemsPerCard).then()
          this.toastr.success(result + " is " + toastrString + " als administrator")
        }, 500);
      }
    });
  }

  todo() {
    this.toastr.warning("Deze functie moet nog geschreven worden")
  }

  expandProjectCard() {
    if (this.projectCardStyle == 'expanded-card') {
      document.getElementById("adminCard").hidden = false;
      document.getElementById("dataCard").hidden = false;
      document.getElementById("taskCard").hidden = false;
      this.projectCardStyle = 'card';
      this.itemsPerCard = 5;
      this.projects=this.projects.slice(0,this.itemsPerCard);
    } else {
      document.getElementById("adminCard").hidden = true;
      document.getElementById("dataCard").hidden = true;
      document.getElementById("taskCard").hidden = true;
      this.projectCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger
      this.getProjects(0, this.itemsPerCard).then(() => {
        this.projectsElementHeight = (this.projects.length * 48);

      })
    }
  }

  expandAdminCard() {
    if (this.adminCardStyle == 'expanded-card') {
      document.getElementById("projectCard").hidden = false;
      document.getElementById("dataCard").hidden = false;
      document.getElementById("taskCard").hidden = false;
      this.adminCardStyle = 'card';
      this.itemsPerCard = 5;
      this.administrators=this.administrators.slice(0,this.itemsPerCard);
    } else {
      document.getElementById("projectCard").hidden = true;
      document.getElementById("dataCard").hidden = true;
      document.getElementById("taskCard").hidden = true;
      this.adminCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getAdministrators(0, this.itemsPerCard).then(() => {
        this.adminElementHeight = (this.administrators.length * 48);
      })

    }
  }

  modTask(modifier: string) {
    if(modifier==='add'){
      const dialogRef = this.dialog.open(AddTaskComponent, {
        width: '500px',
        height: '500px',
        data: {
          modifier:'toevoegen',
        }
      });
      dialogRef.disableClose = true;
      dialogRef.afterClosed().subscribe(result => {
        if (result !== 'false') {
          this.toastr.success(result.name + " is toegevoegd")
          //this.task=result();
          console.log(result)
        }
      });
    }
  }

  expandTaskCard() {
    if (this.taskCardStyle == 'expanded-card') {
      document.getElementById("projectCard").hidden = false;
      document.getElementById("dataCard").hidden = false;
      document.getElementById("adminCard").hidden = false;
      this.taskCardStyle = 'card';
      this.itemsPerCard = 5;
      this.tasks=this.tasks.slice(0,this.itemsPerCard);
    } else {
      document.getElementById("projectCard").hidden = true;
      document.getElementById("dataCard").hidden = true;
      document.getElementById("adminCard").hidden = true;
      this.taskCardStyle = 'expanded-card';
      this.itemsPerCard = this.reasonableMaxInteger;
      this.getTasks(0, this.itemsPerCard).then(() => {
        this.tasksElementHeight = (this.tasks.length * 48);
      })

    }
  }
}
