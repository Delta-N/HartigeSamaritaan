import { Injectable } from '@angular/core';
import { ApiService } from './api.service';
import { Task } from '../models/task';
import { EntityHelper } from '../helpers/entity-helper';
import { HttpResponse } from '@angular/common/http';
import { HttpRoutes } from '../helpers/HttpRoutes';
import { Projecttask } from '../models/projecttask';
import { Searchresult } from '../models/searchresult';
import { ErrorService } from './error.service';

@Injectable({
	providedIn: 'root',
})
export class TaskService {
	constructor(
		private apiService: ApiService,
		private errorService: ErrorService
	) {}

	async getTask(guid: string): Promise<Task> {
		if (!guid) {
			this.errorService.error('taskId mag niet leeg zijn');
			return null;
		}
		let task: Task = null;
		await this.apiService
			.get<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}/${guid}`)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) task = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return task;
	}

	//todo aanpassen
	async getAllTasks(offset: number, pageSize: number): Promise<Task[]> {
		let tasks: Task[] = [];
		await this.apiService
			.get<HttpResponse<Searchresult<Task>>>(
				`${HttpRoutes.taskApiUrl}?offset=${offset}&pageSize=${pageSize}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) tasks = res.body.resultList;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return tasks;
	}

	async postTask(task: Task): Promise<Task> {
		if (
			task == null ||
			task.name == null ||
			task.category == null ||
			task.category.id == null
		) {
			this.errorService.error('Ongeldige taak');
			return null;
		}

		if (!task.id) task.id = EntityHelper.returnEmptyGuid();

		let resTask: Task = null;
		await this.apiService
			.post<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, task)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) resTask = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return resTask;
	}

	async updateTask(updatedTask: Task): Promise<Task> {
		if (
			updatedTask == null ||
			updatedTask.name == null ||
			updatedTask.category == null ||
			updatedTask.category.id == null
		) {
			this.errorService.error('Ongeldige taak');
			return null;
		}
		if (!updatedTask.id) {
			this.errorService.error('TaskID is leeg');
			return null;
		}

		let resTask: Task = null;
		await this.apiService
			.put<HttpResponse<Task>>(`${HttpRoutes.taskApiUrl}`, updatedTask)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) resTask = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return resTask;
	}

	async deleteTask(guid: string): Promise<boolean> {
		if (!guid) {
			this.errorService.error('TaskId is leeg');
			return null;
		}
		let result = false;
		await this.apiService
			.delete<HttpResponse<number>>(`${HttpRoutes.taskApiUrl}/${guid}`)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) result = true;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return result;
	}

	async getAllProjectTasks(guid: string): Promise<Task[]> {
		if (!guid) {
			this.errorService.error('ProjectTaskId is leeg');
			return null;
		}
		let projecttasks: Task[] = [];
		await this.apiService
			.get<HttpResponse<Task[]>>(
				`${HttpRoutes.taskApiUrl}/GetAllProjectTasks/${guid}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) projecttasks = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return projecttasks;
	}

	async addTaskToProject(projectTask: Projecttask): Promise<Task> {
		if (!projectTask) {
			this.errorService.error('ProjectTask is leeg');
			return null;
		}
		let task: Task = null;
		await this.apiService
			.post<HttpResponse<Task>>(
				`${HttpRoutes.taskApiUrl}/AddTaskToProject`,
				projectTask
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) task = res.body;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return task;
	}

	async removeTaskFromProject(
		projectId: string,
		taskId: string
	): Promise<boolean> {
		if (!projectId || !taskId) {
			this.errorService.error('Ongeldige projectId en/of taskId');
			return null;
		}
		let deleted = false;
		await this.apiService
			.delete<HttpResponse<Task>>(
				`${HttpRoutes.taskApiUrl}/RemoveTaskFromProject/${projectId}/${taskId}`
			)
			.toPromise()
			.then(
				(res) => {
					if (res.ok) deleted = true;
				},
				(Error) => {
					this.errorService.httpError(Error);
				}
			);
		return deleted;
	}
}
