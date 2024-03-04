import { Component, OnInit, ViewChild } from '@angular/core';
import { Project } from '../../models/project';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { ProjectService } from '../../services/project.service';
import { Shift } from '../../models/shift';
import { MatPaginator, PageEvent } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { ShiftService } from '../../services/shift.service';
import { TextInjectorService } from '../../services/text-injector.service';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Breadcrumb } from '../../models/breadcrumb';
import { Task } from 'src/app/models/task';
import { MatSliderChange } from '@angular/material/slider';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { ShiftFilter } from '../../models/filters/shift-filter';
import { DateConverter } from '../../helpers/date-converter';
import { Searchresult } from '../../models/searchresult';
import { Shiftdata } from '../../models/helper-models/shiftdata';

@Component({
	selector: 'app-shift-overview',
	templateUrl: './shift-overview.component.html',
	styleUrls: ['./shift-overview.component.scss'],
})
export class ShiftOverviewComponent implements OnInit {
	guid: string | null;
	loaded: boolean = false;
	title: string = 'Shift overzicht';

	project: Project | null;
	shiftData: Shiftdata;
	searchResult: Searchresult<Shift> = new Searchresult<Shift>();

	//filterobjects
	projectTasks: Task[] = [];
	dates: Date[] = [];
	starts: string[] = [];
	ends: string[] = [];
	participantsRequired: number[] = [];

	selectedTasks: Task[] = [];
	date: Date;
	start: string;
	end: string;
	participantReq: number;

	pageSort: string[] = ['date', 'asc'];
	offset: number = 0;
	pageSize: number = 10;
	index: number = 0;

	displayedColumns: string[] = [];
	dataSource: MatTableDataSource<Shift> = new MatTableDataSource<Shift>();
	paginator: MatPaginator;
	sort: MatSort;

	@ViewChild(MatSort) set matSort(ms: MatSort) {
		this.sort = ms;
		this.sort.sortChange.subscribe(() => {
			this.paginator.pageIndex = 0;
			this.pageSort = [this.sort.active, this.sort.direction];
			this.filter();
		});
		this.setDataSourceAttributes();
	}

	@ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
		this.paginator = mp;
		this.setDataSourceAttributes();
	}

	constructor(
		private route: ActivatedRoute,
		private projectService: ProjectService,
		private router: Router,
		private shiftService: ShiftService,
		private breadcrumbService: BreadcrumbService
	) {
		const current: Breadcrumb = new Breadcrumb('Shift overzicht', null);

		const breadcrumbs: Breadcrumb[] = [
			this.breadcrumbService.dashboardcrumb,
			this.breadcrumbService.managecrumb,
			current,
		];
		this.breadcrumbService.replace(breadcrumbs);
		this.displayedColumns = TextInjectorService.shiftTableColumnNames;
	}

	async ngOnInit(): Promise<void> {
		this.route.paramMap.subscribe((params: ParamMap) => {
			this.guid = params.get('id');
		});
		await this.projectService.getProject(this.guid).then(async (project) => {
			this.project = project;
			this.title += ': ' + this.project?.name;
		});

		await this.shiftService.getShiftData(this.guid).then((res) => {
			if (res) {
				this.shiftData = res;
				this.shiftData.tasks = this.shiftData.tasks.filter((t) => t);

				this.shiftData.tasks.sort((a, b) => (a.name > b.name ? 1 : -1));
				this.shiftData.dates.sort((a, b) => (a > b ? 1 : -1));
				this.shiftData.startTimes.sort((a, b) => (a > b ? 1 : -1));
				this.shiftData.endTimes.sort((a, b) => (a > b ? 1 : -1));
				this.shiftData.participantsRequired.sort((a, b) => (a > b ? 1 : -1));

				this.selectedTasks = this.shiftData.tasks;
				this.date = this.shiftData.dates[0];
				this.start = this.shiftData.startTimes[0];
				this.end = this.shiftData.endTimes[0];
				this.participantReq = this.shiftData.participantsRequired[0];
				this.filter();
				this.loaded = true;
			}
		});

		this.dataSource.sortingDataAccessor = (item, property) => {
			switch (property) {
				case 'Taak':
					return item.task !== null ? item.task.name : null;
				case 'Datum':
					return item.date;
				case 'Vanaf':
					return item.startTime;
				case 'Tot':
					return item.endTime;
				case '#Benodigde vrijwilligers':
					return item.participantsRequired;
				default:
					return item[property];
			}
		};
	}

	setDataSourceAttributes() {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	details(id: string) {
		this.router.navigate(['manage/shift', id]).then();
	}

	setMinDate($event: MatSliderChange) {
		this.date = this.shiftData.dates[$event.value];
		this.offset = 0;
		this.index = 0;
	}

	setMinStartTime($event: MatSliderChange) {
		this.start = this.shiftData.startTimes[$event.value];
		this.offset = 0;
		this.index = 0;
	}

	setMinEndTime($event: MatSliderChange) {
		this.end = this.shiftData.endTimes[$event.value];
		this.offset = 0;
		this.index = 0;
	}

	setMinPar($event: MatSliderChange) {
		this.participantReq = this.shiftData.participantsRequired[$event.value];
		this.offset = 0;
		this.index = 0;
	}

	OnCheckboxChange($event: MatCheckboxChange) {
		const task = this.shiftData.tasks.find((pt) => pt.id === $event.source.id);
		if ($event.checked && task) this.selectedTasks.push(task);
		else {
			const st = this.selectedTasks.find((st) => st.id === $event.source.id);
			if (st) this.selectedTasks = this.selectedTasks.filter((t) => t !== task);
		}
		this.offset = 0;
		this.index = 0;
		this.filter();
	}

	async filter() {
		const filter: ShiftFilter = new ShiftFilter();
		filter.projectId = this.project?.id;
		filter.offset = this.offset;
		filter.pageSize = this.pageSize;
		filter.sort = this.pageSort;

		const tasks: string[] = [];
		this.selectedTasks.forEach((st) => tasks.push(st.id));
		filter.tasks = tasks;

		filter.date = DateConverter.addOffset(this.date);
		filter.start = this.start;
		filter.end = this.end;
		filter.participantsRequired = this.participantReq;

		if (filter.start && filter.end) {
			await this.shiftService.getShifts(filter).then((res) => {
				if (res) {
					this.searchResult = res;
					this.dataSource.data = res.resultList;
				}
			});
			this.paginator.length = this.searchResult.totalcount;
			this.paginator.pageIndex = this.index;
		}
	}

	changePage($event: PageEvent) {
		this.pageSize = $event.pageSize;
		this.offset = $event.pageIndex * $event.pageSize;
		this.index = $event.pageIndex;
		this.filter();
	}
}
