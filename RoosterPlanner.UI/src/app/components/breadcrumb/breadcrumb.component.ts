import { Component } from '@angular/core';
import { Breadcrumb } from '../../models/breadcrumb';
import { BreadcrumbService } from '../../services/breadcrumb.service';
import { RouterLink } from '@angular/router';
import { NgForOf, NgIf } from '@angular/common';

@Component({
	selector: 'app-breadcrumb',
	templateUrl: './breadcrumb.component.html',
	standalone: true,
	imports: [RouterLink, NgIf, NgForOf],
	styleUrls: ['./breadcrumb.component.scss'],
})
export class BreadcrumbComponent {
	breadcrumbs: Breadcrumb[] = [];

	constructor(private breadcrumbService: BreadcrumbService) {
		breadcrumbService.behaviourSubject.subscribe((values) => {
			this.breadcrumbs = values;
		});
	}
}
