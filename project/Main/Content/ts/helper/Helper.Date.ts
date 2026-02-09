import dayjs from "dayjs";
import moment from "moment";

export class HelperDate {
	static getCurrentTimeStringRoundedToQuarterHours(roundUp: boolean): string {
		if (roundUp === undefined) {
			throw new Error("Helper.Date.getCurrentTime: param roundUp not defined");
		}
		const now = dayjs();
		let currentHour = now.hour();
		const currentMinute = now.minute();
		let minute;
		if (currentMinute === 0 || (currentMinute < 15 && roundUp === false)) {
			minute = "00";
		} else if (currentMinute <= 15 || (currentMinute < 30 && roundUp === false)) {
			minute = "15";
		} else if (currentMinute <= 30 || (currentMinute < 45 && roundUp === false)) {
			minute = "30";
		} else if (currentMinute <= 45 || (currentMinute < 60 && roundUp === false)) {
			minute = "45";
		} else {
			minute = "00";
			currentHour++;
			if (currentHour === 24) {
				currentHour = 0;
			}
		}
		const hour = currentHour < 10 ? "0" + currentHour.toString() : currentHour.toString();
		return hour + ":" + minute;
	}

	static getLastWeek(): Date {
		const today = new Date();
		return new Date(today.getFullYear(), today.getMonth(), today.getDate() - 7);
	}

	static minusDays(days: number): Date {
		const today = new Date();
		return new Date(today.getFullYear(), today.getMonth(), today.getDate() - days);
	}

	// http://stackoverflow.com/questions/17732897/difference-between-two-dates-in-years-months-days-in-javascript
	static diffInDays(a: Date, b: Date): number {
		// @ts-ignore
		const days = (a - b) / (60 * 60 * 24 * 1000);
		return Math.round(days);
	}

	static setTimeToDate(dateA: Date, timeB: Date): Date {
		if (!timeB) {
			return dateA;
		}
		if (!dateA) {
			return null;
		}

		var datetime = new Date(dateA.getFullYear(), dateA.getMonth(), dateA.getDate(),
			(timeB || dateA).getHours(), (timeB || dateA).getMinutes(), (timeB || dateA).getSeconds());
		return datetime;
	}

	static areRangesOverlapping(range1Start: Date, range1End: Date, range2Start: Date, range2End: Date): boolean {
		return range1Start < range2End && range2Start < range1End;
	}

	/**
	 * Returns true if there is any overlap between the whole input day and the range of "from" and "to"
	 */
	static dayIsInRange(day: Date, from: Date, to: Date): boolean {
		const dateStart = moment(day).startOf("day").toDate();
		const dateEnd = moment(day).endOf("day").toDate();

		return HelperDate.areRangesOverlapping(dateStart, dateEnd, from, to);
	}

	/**
	 * Returns true if the input date is between "from" and "to"
	 */
	static dateIsInRange(date: Date, from: Date, to: Date): boolean {
		return date >= from && date < to;
	}

	static getRangesOverlapInMinutes(range1Start: Date, range1End: Date, range2Start: Date, range2End: Date): number {
		const doesOverlap = HelperDate.areRangesOverlapping(range1Start, range1End, range2Start, range2End);
		if (doesOverlap) {
			const from = range1Start > range2Start ? range1Start : range2Start;
			const end = range1End < range2End ? range1End : range2End;

			return moment(end).diff(from, 'minutes');
		}
		else {
			return 0;
		}
	}

	static getDayRangeOverlapInMinutes(day: Date, from: Date, to: Date) {
		const dateStart = moment(day).startOf("day").toDate();
		const dateEnd = moment(day).startOf("day").add(1, "day").toDate();

		return HelperDate.getRangesOverlapInMinutes(dateStart, dateEnd, from, to);
	}

	static getDayRangeOverlapDuration(day: Date, from: Date, to: Date, timeZoneFrom: string, timeZoneTo: string): string {
		let minutes = HelperDate.getDayRangeOverlapInMinutes(day, from, to);
		let diff = 0;
		if(timeZoneFrom && timeZoneTo) {
			diff = HelperDate.getTimeZoneDifferenceMinutes(timeZoneFrom, timeZoneTo);
		}
		return moment.duration(minutes + diff, 'minutes').toString();
	}

	static areOnSameDay(date1: Date, date2: Date) {
		const m1 = moment(date1), m2 = moment(date2);

		return m1.year() == m2.year() && m1.dayOfYear() == m2.dayOfYear();
	}

	static getDatesOfRange(from: Date, to: Date): Date[] {
		let currDate = moment(from).startOf('day');
		let lastDate = moment(to).startOf('day');
		let sameDay = currDate.year() == lastDate.year() && currDate.dayOfYear() == lastDate.dayOfYear();

		let dates = [currDate.toDate()];

		while (currDate.add(1, 'days').diff(lastDate) < 0) {
			dates.push(currDate.toDate());
		}

		if (!sameDay && (to.getHours() > 0 || to.getMinutes() > 0 || to.getSeconds() > 0 || to.getMilliseconds() > 0)) {
			dates.push(lastDate.toDate());
		}

		return dates;
	}
	
	static getTimeZoneSelect2Options() {
		return {
			data: window.moment.tz.names(),
			optionsText: (x: string) => this.getTimeZoneWithUtcText(x),
			optionsValue: (x: string) => x
		};
	}
	
	static getTimeZoneWithUtcText(timeZone: string | KnockoutObservable<string>) {
		return `(UTC ${this.getUtcOffsetString(ko.unwrap(timeZone))}) ${this.getTimeZoneCldrLocalizedText(ko.unwrap(timeZone))}`;
	} 
	
	static getTimeZoneCldrLocalizedText(timeZone: string) {
		const cldrData = window.Globalize.cldr.main("dates/timeZoneNames/zone/"+timeZone);
		if(!cldrData) {
			return timeZone;
		}
		if(cldrData.hasOwnProperty("exemplarCity")) {
			return cldrData["exemplarCity"];
		}
		if(cldrData.hasOwnProperty("short")) {
			if(cldrData.hasOwnProperty("standard")) {
				return cldrData["short"]["standard"];
			}
			if(cldrData.hasOwnProperty("generic")) {
				return cldrData["short"]["generic"];
			}
			return timeZone;
		}
		if(cldrData.hasOwnProperty("long")) {
			if(cldrData.hasOwnProperty("standard")) {
				return cldrData["long"]["standard"];
			}
			if(cldrData.hasOwnProperty("generic")) {
				return cldrData["long"]["generic"];
			}
			return timeZone;
		}
		return timeZone;
	}
	
	static getTimeZoneForUser(user: Main.Rest.Model.Main_User | KnockoutObservable<Main.Rest.Model.Main_User>): string {
		const timeZoneName = ko.unwrap(user).TimeZoneName;
		if(!window.moment.tz.names().some(x => x == timeZoneName)) {
			return window.moment.tz.guess(true);
		}
		return timeZoneName;
	}

	static isTimeZoneSame(timeZoneA: string | KnockoutObservable<string>, timeZoneB: string | KnockoutObservable<string> = null): boolean {
		if(ko.unwrap(timeZoneA) == null) {
			timeZoneA = window.moment.tz.guess(true);
		}
		if(ko.unwrap(timeZoneB) == null) {
			timeZoneB = window.moment.tz.guess(true);
		}
		return window.moment().tz(ko.unwrap(timeZoneA)).utcOffset() == window.moment().tz(ko.unwrap(timeZoneB)).utcOffset();
	}

	static convertDateToLocal(date: Date | KnockoutObservable<Date>, timeZone: string | KnockoutObservable<string>): Date {
		if(!ko.unwrap(date) || !ko.unwrap(timeZone)) {
			return ko.unwrap(date);
		}
		return window.moment(ko.unwrap(date)).tz(ko.unwrap(timeZone), true).toDate();
	}

	static convertLocalDateToTimeZone(date: Date | KnockoutObservable<Date>, timeZone: string | KnockoutObservable<string>): Date {
		if(!ko.unwrap(date) || !ko.unwrap(timeZone)) {
			return ko.unwrap(date);
		}
		//@ts-ignore
		return new Date(window.moment(ko.unwrap(date)).tz(ko.unwrap(timeZone)).stripZone().format());
	}

	static getUtcOffsetString(timeZone: string | KnockoutObservable<string>, date: Date | KnockoutObservable<Date> = null): string {
		if(!ko.unwrap(timeZone)) {
			return null;
		}
		return window.moment(ko.unwrap(date) || new Date()).tz(ko.unwrap(timeZone)).format("Z");
	}

	static getTimeZoneDifferenceMinutes(timeZone1: string | KnockoutObservable<string>, timeZone2: string | KnockoutObservable<string>): number {
		if(!ko.unwrap(timeZone1) || !ko.unwrap(timeZone2)) {
			return 0;
		}
		return window.moment().tz(ko.unwrap(timeZone1)).utcOffset() - window.moment().tz(ko.unwrap(timeZone2)).utcOffset();
	}

	static getFromAndToFromString(dateString: string): { dateFrom: Date, dateTo: Date } {
		let dateFrom = null;
		let dateTo = null;
		if (dateString === "LastMonth") {
			dateFrom = window.moment().subtract(1, "month").startOf("month");
			dateTo = window.moment().subtract(1, "month").endOf("month");
		} else if (dateString === "Next3Months") {
			dateFrom = window.moment().add(1, "month").startOf("month");
			dateTo = window.moment().add(3, "month").endOf("month");
		} else if (dateString === "ThisMonth") {
			dateFrom = window.moment().startOf("month");
			dateTo = window.moment().endOf("month");
		} else if (dateString === "NextMonth") {
			dateFrom = window.moment().add(1, "month").startOf("month");
			dateTo = window.moment().add(1, "month").endOf("month");
		} else if (dateString === "ThisYear") {
			dateFrom = window.moment().startOf("year");
			dateTo = window.moment().endOf("year");
		} else if (dateString === "LastYear") {
			dateFrom = window.moment().subtract(1, "year").startOf("year");
			dateTo = window.moment().subtract(1, "year").endOf("year");
		}
		return {dateFrom: dateFrom?.toDate(), dateTo: dateTo?.toDate()};
	}

}

