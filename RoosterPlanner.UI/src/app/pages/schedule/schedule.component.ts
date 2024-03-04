import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute, ParamMap, Router } from '@angular/router';
import { Availability } from '../../models/availability';
import { AvailabilityService } from '../../services/availability.service';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { TextInjectorService } from '../../services/text-injector.service';
import moment from 'moment';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { Breadcrumb } from '../../models/breadcrumb';
import { Event } from '../../models/event';
import { faCalendarPlus } from '@fortawesome/free-solid-svg-icons';
import { MaterialModule } from '../../modules/material/material.module';
import { TableDatePipe } from '../../helpers/filter.pipe';

@Component({
	selector: 'app-schedule',
	templateUrl: './schedule.component.html',
	standalone: true,
	imports: [MaterialModule, TableDatePipe],
	styleUrls: ['./schedule.component.scss'],
})
export class ScheduleComponent implements OnInit {
	calendarIcon = faCalendarPlus;
	loaded: boolean = false;
	today: Date = new Date();
	displayedColumns: string[] = [];
	dataSource: MatTableDataSource<Availability> =
		new MatTableDataSource<Availability>();
	paginator: MatPaginator;
	sort: MatSort;
	availabilities: Availability[];

	@ViewChild(MatSort) set matSort(ms: MatSort) {
		this.sort = ms;
		this.setDataSourceAttributes();
	}

	@ViewChild(MatPaginator) set matPaginator(mp: MatPaginator) {
		this.paginator = mp;
		this.setDataSourceAttributes();
	}

	constructor(
		private route: ActivatedRoute,
		private availabilityService: AvailabilityService,
		private breadcrumbService: BreadcrumbService,
		private router: Router
	) {}

	ngOnInit(): void {
		this.displayedColumns = TextInjectorService.availabilitiesTableColumnNames;
		this.route.paramMap.subscribe(async (params: ParamMap) => {
			await this.availabilityService
				.getScheduledAvailabilities(params.get('id'))
				.then((res) => {
					if (res && res.length > 0) {
						this.availabilities = res;
						//push old availabilities to the back
						const old: Availability[] = [];
						const all: Availability[] = [];
						res.forEach((a) => {
							if (
								moment(a.shift.date).startOf('day').toDate() <
								moment().startOf('day').toDate()
							)
								old.push(a);
							else all.push(a);
						});
						old.forEach((a) => all.push(a));

						this.dataSource = new MatTableDataSource<Availability>(all);

						// @ts-ignore
						this.dataSource.sortingDataAccessor = (item, property) => {
							switch (property) {
								case 'Taak':
									return item.shift.task != null ? item.shift.task.name : null;
								case 'Datum':
									return item.shift.date;
								case 'Vanaf':
									return item.shift.startTime;
								case 'Tot':
									return item.shift.endTime;
								default:
									return item[property];
							}
						};

						const previous: Breadcrumb = new Breadcrumb(
							res[0].participation.project.name,
							'/project/' + res[0].participation.project.id
						);
						const current: Breadcrumb = new Breadcrumb('Mijn shifts', null);

						const array: Breadcrumb[] = [
							this.breadcrumbService.dashboardcrumb,
							previous,
							current,
						];
						this.breadcrumbService.replace(array);
					} else {
						const array: Breadcrumb[] = [this.breadcrumbService.dashboardcrumb];
						this.breadcrumbService.replace(array);
					}
					this.loaded = true;
				});
		});
	}

	setDataSourceAttributes() {
		this.dataSource.paginator = this.paginator;
		this.dataSource.sort = this.sort;
	}

	openInstructions(url: string | null) {
		if (url) window.open(url, '_blank');
	}

	getExactDate(date: Date, time: string): Date {
		const outputDate = moment(date);
		const outputTime = moment(time, 'HH,mm');
		outputDate.set({
			hour: outputTime.get('hour'),
			minute: outputTime.get('minute'),
			second: 0,
		});

		return outputDate.toDate();
	}

	ics() {
		const events: Event[] = [];

		if (this.availabilities && this.availabilities.length > 0) {
			this.availabilities.forEach((a) => {
				const event: Event = new Event();
				event.start = this.getExactDate(a.shift.date, a.shift.startTime);
				event.end = this.getExactDate(a.shift.date, a.shift.endTime);
				event.summary = a.shift.task.name;
				event.description = a.shift.task.description;
				event.location =
					a.participation.project.address + ' ' + a.participation.project.city;
				events.push(event);
			});
			const ics = this.createEvent(events);
			this.download('MijnShifts.ics', ics);
		}
	}

	download(filename: string, text: string) {
		const element = document.createElement('a');
		element.setAttribute(
			'href',
			'data:text/plain;charset=utf-8,' + encodeURIComponent(text)
		);
		element.setAttribute('download', filename);
		element.setAttribute('target', '_blank');
		element.style.display = 'none';
		element.click();
	}

	createEvent(events: Event[]) {
		const formatDate = (date: Date): string => {
			if (!date) {
				return '';
			}
			// don't use date.toISOString() here, it will be always one day off (cause of the timezone)
			const day = date.getDate() < 10 ? '0' + date.getDate() : date.getDate();
			const month =
				date.getMonth() < 9 ? '0' + (date.getMonth() + 1) : date.getMonth() + 1;
			const year = date.getFullYear();
			const hour =
				date.getHours() < 10 ? '0' + date.getHours() : date.getHours();
			const minutes =
				date.getMinutes() < 10 ? '0' + date.getMinutes() : date.getMinutes();
			const seconds =
				date.getSeconds() < 10 ? '0' + date.getSeconds() : date.getSeconds();
			return `${year}${month}${day}T${hour}${minutes}${seconds}`;
		};
		let VCALENDAR = `BEGIN:VCALENDAR
PRODID:-//Events Calendar//HSHSOFT 1.0//DE
VERSION:2.0
`;
		for (const event of events) {
			const timeStamp = formatDate(new Date());
			const uuid = `${timeStamp}Z-uid@hshsoft.de`;
			/**
			 * Don't ever format this string template!!!
			 */
			const EVENT = `BEGIN:VEVENT
DTSTAMP:${timeStamp}Z
DTSTART:${formatDate(event.start)}
DTEND:${formatDate(event.end)}
SUMMARY:${event.summary}
DESCRIPTION:${event.description}
LOCATION:${event.location}
URL:${event.url}
UID:${uuid}
END:VEVENT`;
			VCALENDAR += `${EVENT}
`;
		}
		VCALENDAR += `END:VCALENDAR`;

		return VCALENDAR;
	}

	details(id) {
		this.router.navigate(['task', id]).then();
	}
}
