import { CalendarDateFormatter, DateFormatterParams } from 'angular-calendar';
import { formatDate } from '@angular/common';
import { Injectable } from '@angular/core';

@Injectable()
export class CustomDateFormatter extends CalendarDateFormatter {
	// you can override any of the methods defined in the parent class
	fallbackLocale = 'nl-NL';

	override monthViewColumnHeader({
		date,
		locale,
	}: DateFormatterParams): string {
		return formatDate(date, 'EEE', locale ?? this.fallbackLocale);
	}

	override monthViewTitle({ date, locale }: DateFormatterParams): string {
		return formatDate(date, 'MMM y', locale ?? this.fallbackLocale);
	}

	override weekViewColumnHeader({ date, locale }: DateFormatterParams): string {
		return formatDate(date, 'EEE', locale ?? this.fallbackLocale);
	}

	override dayViewHour({ date, locale }: DateFormatterParams): string {
		return formatDate(date, 'HH:mm', locale ?? this.fallbackLocale);
	}
}
