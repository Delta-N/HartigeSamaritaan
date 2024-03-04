import { InjectionToken } from '@angular/core';

export const API_URL = new InjectionToken<string>('Api Url');

export const DEVELOPMENT_STATS_ENABLED = new InjectionToken<boolean>(
	'Development Page Enabled'
);
