/// <reference lib="es2021.weakref" />
/// <reference lib="es2022.array" />
/// <reference path="es2023.array.d.ts" />
/// <reference path="esnext.collection.d.ts" />

import type { Article as ArticleType } from "../ts/Model/Article";
import type { Dispatch as DispatchType } from "../ts/Model/Dispatch";
import type { Technician as TechnicianType } from "../ts/Model/Technicians";
import type { Drag as DragType } from "../ts/Helper/DragHelper";
import type { HelperScheduler } from "../ts/Helper/Helper.Scheduler";
import { Scheduler as SchedulerType } from "../ts/knockout.component.Scheduler";
import { SchedulerAddResourceModalViewModel as SchedulerAddResourceModalViewModelType } from "../ts/Modals/SchedulerAddResourceModalViewModel";
import { SchedulerDetailsViewModel as SchedulerDetailsViewModelType } from "../ts/SchedulerDetailsViewModel";
import { SchedulerEditModalViewModel as SchedulerEditModalViewModelType } from "../ts/Modals/SchedulerEditModalViewModel";
import { SchedulerLoadProfileModalViewModel as SchedulerLoadProfileModalViewModelType } from "../ts/Modals/SchedulerLoadProfileModalViewModel";
import { SchedulerAdHocModalViewModel as SchedulerAdHocModalViewModelType } from "../ts/Modals/SchedulerAdHocModalViewModel";
import { SchedulerAddProfileModalViewModel as SchedulerAddProfileModalViewModelType } from "../ts/Modals/SchedulerAddProfileModalViewModel";
import { SchedulerEditProfileModalViewModel as SchedulerEditProfileModalViewModelType } from "../ts/Modals/SchedulerEditProfileModalViewModel";
import { SchedulerGetRouteModalViewModel as SchedulerGetRouteModalViewModelType } from "../ts/Modals/SchedulerGetRouteModalViewModel";
import { SchedulerMapViewModel as SchedulerMapViewModelType } from "../ts/SchedulerMapViewModel";
import { SchedulerResourceReorderModalViewModel as SchedulerResourceReorderModalViewModelType } from "../ts/Modals/SchedulerResourceReorderModalViewModel";
import {Assignment as AssignmentType} from "../ts/Model/Assignment"
import {Timeline as TimelineType} from "../ts/Timeline"
import type {MapMarkerType} from "../ts/Model/MapMessage";
import {type Team as TeamType} from "@Sms.Scheduler/Model/Team";
import type {RouteData} from "../ts/Model/RouteData";

declare global {
	export type ResourceTypes = TechnicianType | ArticleType;
	namespace Sms {
		namespace Scheduler {
			let Drag: typeof DragType;
			namespace Model {
				let Assignment: typeof AssignmentType;
				let Team: typeof TeamType;
				let Technician: typeof TechnicianType;
				let Article: typeof ArticleType;
				let Dispatch: typeof DispatchType;
			}
			let Timeline: typeof TimelineType;
			namespace ViewModels {
				let Scheduler: typeof SchedulerType;
				let SchedulerDetailsViewModel: typeof SchedulerDetailsViewModelType;
				let SchedulerEditModalViewModel: typeof SchedulerEditModalViewModelType;
				let SchedulerAddResourceModalViewModel: typeof SchedulerAddResourceModalViewModelType;
				let SchedulerLoadProfileModalViewModel: typeof SchedulerLoadProfileModalViewModelType;
				let SchedulerAddProfileModalViewModel: typeof SchedulerAddProfileModalViewModelType;
				let SchedulerEditProfileModalViewModel: typeof SchedulerEditProfileModalViewModelType;
				let SchedulerAdHocModalViewModel: typeof SchedulerAdHocModalViewModelType;
				let SchedulerGetRouteModalViewModel: typeof SchedulerGetRouteModalViewModelType;
				let SchedulerMapViewModel: typeof SchedulerMapViewModelType;
				let SchedulerResourceReorderModalViewModel: typeof SchedulerResourceReorderModalViewModelType;
			}
			namespace Settings {
				namespace DashboardCalendar {
					let ShowAbsencesInCalendar: boolean;
				}
				namespace WorkingTime {
					let FromDay: number;
					let ToDay: number;
					let FromHour: number;
					let ToHour: number;
					let MinutesInterval: number;
					let IgnoreWorkingTimesInEndDateCalculation: string;
				}
				let ServiceOrderZipCodeAreaLength: number;
				let DispatchesAfterReleaseAreEditable: boolean;
			}
		}
	}
	namespace Helper {
		let Scheduler: typeof HelperScheduler;
	}

	// Type definitions for bindingEvent of Knockout v3.5.0 which is not available in the solutions knockout type definition file Knockout v3.4.0
	interface KnockoutStatic {
		bindingEvent: {
			subscribe(node: Node, event: "childrenComplete" | "descendantsComplete", callback: (node: Node) => void, callbackContext?: any): Subscription;
		}
	}
}

interface Subscription {
	dispose(): void;
	disposeWhenNodeIsRemoved(node: Node): void;
}

type ComboItem =	{
		text: string,
		color: string 
	} & (StatusItem | EntityStateItem)

type StatusItem = {
	type: 'Status',
	value: string
}
type EntityStateItem = {
	type: 'EntityState',
	value: number,
	border: string
}

type ClusterGroup = {
	title: string,
	markerTypes: MapMarkerType[]
}

export type groupedGridData<T> = {
	name: string,
	expanded: boolean,
	children: groupedGridData<T>[] | T[]
}

interface RouteConfig {
	routeData: RouteData[];
	useTechnicianHomeAddressAsOrigin: boolean;
	useTechnicianHomeAddressAsFinalDestination: boolean;
	getRoutePerDay: boolean;
}