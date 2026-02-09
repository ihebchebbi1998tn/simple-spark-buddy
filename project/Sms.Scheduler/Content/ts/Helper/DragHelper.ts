import {
	CalendarModel,
	DateHelper,
	DomHelper,
	DragHelper,
	DragHelperConfig,
	EventModel,
	Grid,
	GridRowModel,
	ResourceModel,
	ScrollManager,
	Tooltip
} from "@bryntum/schedulerpro";
import type {Timeline} from "../Timeline";
import type {Pipeline} from "../Pipeline";
import {isServiceOrder, ServiceOrder} from "../Model/ServiceOrder";
import {isTechnician, Technician} from "../Model/Technicians";
import type {Absence} from "../Model/Absence";
import {namespace} from "@Main/namespace";
import {type Dispatch, isDispatch} from "@Sms.Scheduler/Model/Dispatch";
import { isAbsenceOrder, isArticleDowntime } from "../Model/AbsenceOrder";
import { isJob } from "../Model/Job";

export class Drag extends DragHelper {
	public scrollManager!: ScrollManager;
	public grid!: Grid;
	tip: Tooltip;
	enablePlanningConfirmations: () => boolean;
	allowSchedulingForPast: () => boolean;
	snapFirstEventOfDayToStartOfDay: () => boolean;
	orderSelector: (g: Grid, e: HTMLElement) => ServiceOrder[];

	constructor(config: Partial<DragHelperConfig> & {
		schedule: Timeline,
		grid: Pipeline,
		constrain: boolean,
		enablePlanningConfirmations: () => boolean,
		allowSchedulingForPast: () => boolean,
		snapFirstEventOfDayToStartOfDay: () => boolean,
		orderSelector: (g: Grid, e: HTMLElement) => ServiceOrder[]
	}) {
		super(config);
		const me = this;
		me.grid = config.grid;
		me.schedule = config.schedule;
		me.enablePlanningConfirmations = config.enablePlanningConfirmations;
		me.orderSelector = config.orderSelector;
		me.allowSchedulingForPast = config.allowSchedulingForPast;
		me.snapFirstEventOfDayToStartOfDay = config.snapFirstEventOfDayToStartOfDay;

		// Configure DragHelper with schedule's scrollManager to allow scrolling while dragging
		//me.scrollManager = me.schedule.scrollManager;
		this.on({
			dragstart: this.onDragStart,
			drag: this.onDrag,
			drop: this.onDrop,
			beforeDragStart: this.beforeDragStart
		});
	}

	static get $name() {
		return 'CRMPDrag';
	}

	// Factoryable type name
	static get type() {
		return 'CRMdrag';
	}

	static get configurable() {
		return {
			callOnFunctions: false,
			// Don't drag the actual row element, clone it
			cloneTarget: true,
			mode: 'translateXY',
			// We size the cloned element manually
			autoSizeClonedTarget: false,
			// Only allow drops on the schedule area
			dropTargetSelector: '.b-timeline-subgrid',
			schedule: null,
			grid: null,
		};
	}

	public _schedule!: Timeline & { allowOverlap?: boolean, isHorizontal?: boolean };

	get schedule() {
		return this._schedule;
	}

	set schedule(schedule) {
		this._schedule = schedule;

		// Configure DragHelper with schedule's scrollManager to allow scrolling while dragging
		//this.scrollManager = schedule.scrollManager;
	}

	async createEvent(schedule: Timeline, newX: number, resource: ResourceModel, tasks: EventModel[], orders: GridRowModel[], calendar: CalendarModel, targetDispatch: Dispatch = null): Promise<(Absence | Dispatch)[]> {
		schedule.disableScrollingCloseToEdges(schedule.timeAxisSubGrid);
		// If drop was done in a valid location, set the startDate and transfer the task to the Scheduler event store
		let date = schedule.getDateFromCoordinate(newX, 'round', false, true);
		let result: (Absence | Dispatch)[] = [];
		const resources = Array.isArray(resource) ? (resource as ResourceModel[]) : [resource];
		if (isTechnician(resources[0]) || isAbsenceOrder(orders[0]) || isArticleDowntime(orders[0])) {
			if(isDispatch(tasks[0]) && this.snapFirstEventOfDayToStartOfDay()) {
				if (schedule.eventStore.getEventsForResource(resource).filter((r) => window.Helper.Date.areOnSameDay(date, r.startDate as Date)).length === 0) {
					date = resource.getCalendar().getWorkingTimeRanges(DateHelper.startOf(date), DateHelper.endOf(date))[0].startDate;
				}
			}
				for (let order of orders) {
				try {
					const duration = window.Helper.Scheduler.determineNewEventDuration(
						order,
						schedule.defaultEventDuration,
						schedule.serviceOrderDispatchMaximumDuration,
						schedule.ServiceOrderDispatchIgnoreCalculatedDuration
					);

					if (isAbsenceOrder(order) || isArticleDowntime(order)) {
						for (const r of resources) {
							const task = await window.Helper.Scheduler.createEvent(order, r, date, duration);
							let startDate = isDispatch(task) ? task.OriginalData.Date : task.OriginalData.From;
							schedule.scheduleEvent({
								eventRecord: task,
								startDate: startDate,
								resourceRecord: r
							});
							result.push(task);
						}
					} else {
						const technician = window.Helper.Scheduler.determineServiceOrderTeamLeader(order as ServiceOrder, resources as Technician[]);
						const task = await window.Helper.Scheduler.createEvent(order, technician, date, duration, targetDispatch);
						if (task != targetDispatch) {
							await schedule.eventStore.addAsync(task);
							await schedule.assignmentStore.addAsync(resources.map(r => ({ event: task, resource: r })));
						}
						result.push(task);
					}

					schedule.refreshRows();
				} catch (e) {
					window.Log.error("Failed creating event", e);
				}
			}
		}

		schedule.features.eventTooltip.disabled = false;
		return result;
	}

	createProxy(element) {
		const
			proxy = document.createElement('div'),
			{schedule} = this,
			orders = this.orderSelector(this.grid, element);

		const duration = orders.reduce((sum, order) => {
			const duration = window.Helper.Scheduler.determineNewEventDuration(
				order,
				schedule.defaultEventDuration,
				schedule.serviceOrderDispatchMaximumDuration,
				schedule.ServiceOrderDispatchIgnoreCalculatedDuration
			);

			sum += duration.asMilliseconds();

			return sum;
		}, 0);

		let durationInPx = schedule.timeAxisViewModel.getDistanceForDuration(duration);

		// Fake an event bar
		proxy.classList.add('b-sch-event-wrap', 'b-unassigned-class')
		proxy.innerHTML = `<div class="b-event-name">${orders.map(o => o.name).join(", ")}</div>`;

		if (schedule.isHorizontal) {
			// @ts-ignore
			proxy.style.height = `${schedule.rowHeight - (2 * schedule.resourceMargin)}px`;
			proxy.style.width = `${durationInPx}px`;
		} else {
			proxy.style.height = `${durationInPx}px`;
			proxy.style.width = `${schedule.resourceColumnWidth}px`;
		}

		return proxy;
	}

	beforeDragStart = () => {
		return window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit");
	}

	// Drop callback after a mouse up, take action and transfer the unplanned task to the real EventStore (if it's valid)

	onDragStart = ({context}) => {
		const
			me = this,
			{schedule} = me,
			orders = this.orderSelector(this.grid, context.grabbed),
			{eventTooltip} = schedule.features;

		// save a reference to the task so we can access it later
		context.tasks = orders.map(order => {
			let task = new EventModel();

			const duration = window.Helper.Scheduler.determineNewEventDuration(
				order,
				schedule.defaultEventDuration,
				schedule.serviceOrderDispatchMaximumDuration,
				schedule.ServiceOrderDispatchIgnoreCalculatedDuration
			);

			task.duration = duration.as(task.durationUnit as any);
			return task;
		});

		// Prevent tooltips from showing while dragging
		eventTooltip.disabled = true;
		context.orders = orders;
		schedule.enableScrollingCloseToEdges(schedule.timeAxisSubGrid);

		if (!me.tip) {
			me.tip = new Tooltip({
				align: 'b-t',
				//clippedBy  : [schedule.timeAxisSubGridElement, schedule.bodyContainer],
				forElement: context.element,

				cls: 'b-popup b-sch-event-tooltip'
			});
		}
	};

	onDrag = ({event, context}) => {
		const tasks = context.tasks as EventModel[];
		const orders = context.orders.map(x => isJob(x) ? x.serviceOrder : x) as ServiceOrder[];
		const me = this;
		const { schedule } = me;
		const coordinate = DomHelper[`getTranslate${schedule.isHorizontal ? 'X' : 'Y'}`](
			context.element
		);
		// Persist coordinate for onDrop (createEvent expects axis-coordinate)
		context.newX = coordinate;
		
		let resourceRecord = (context.target && schedule.resolveResourceRecord(context.target, [event.offsetX, event.offsetY])) as ResourceModel;

		// Fallback: resolve resource record from the hovered row element (group headers can fail resolveResourceRecord)
		if (!resourceRecord && context.target) {
			const targetEl = context.target as HTMLElement;
			const rowEl = (targetEl.closest?.('.b-grid-row') ?? targetEl.closest?.('.b-sch-timeaxis-row')) as HTMLElement;
			const rowId = rowEl?.getAttribute?.('data-id') ?? (rowEl as any)?.dataset?.id;
			const byId = rowId != null ? (schedule.resourceStore as any).getById?.(rowId) : null;
			resourceRecord = (byId as ResourceModel) ?? resourceRecord;
		}

		// Resolve leaf resources robustly for group header/virtual nodes (children can be boolean)
		const resolveLeafRecords = (root: any): ResourceModel[] => {
			if (!root) return [];
			if (root.isLeaf) return [root as ResourceModel];

			const leaves: ResourceModel[] = [];

			// Prefer Bryntum tree traversal APIs if present
			if (typeof root.traverse === 'function') {
				root.traverse((node: any) => {
					if (node?.isLeaf) leaves.push(node as ResourceModel);
				});
				return leaves;
			}

			// Fallback to recursive children traversal
			const collectViaChildren = (node: any) => {
				const children = node?.children;
				if (Array.isArray(children) && children.length > 0) {
					for (const child of children) collectViaChildren(child);
					return;
				}
				if (node?.isLeaf) leaves.push(node as ResourceModel);
			};
			collectViaChildren(root);

			// Ultimate fallback: derive descendants by walking parent pointers in the store
			if (!leaves.length) {
				const ancestorId = root?.id;
				const isDescendantOf = (node: any): boolean => {
					let p = node?.parent;
					while (p) {
						if (p === root) return true;
						if (ancestorId != null && p.id === ancestorId) return true;
						p = p.parent;
					}
					return false;
				};
				const storeLeaves = schedule.resourceStore.allRecords.filter(r => (r as any)?.isLeaf && isDescendantOf(r)) as ResourceModel[];
				leaves.push(...storeLeaves);
			}

			return leaves;
		};

		const leafRecords = resolveLeafRecords(resourceRecord);
		const leafIds = new Set(leafRecords.map(r => r?.id).filter(id => id != null));
		let resources = schedule.resourceStore.allRecords.filter(r => leafIds.has((r as any).id) && (r as any).isLeaf) as ResourceModel[];

		// If our leaf collection returned instances not matching store instances, keep them as a last resort
		if (!resources.length) {
			resources = leafRecords;
		}

		const resource = resources?.[0];
		const calendar = (resource && (resource["calendar"] as CalendarModel)) ?? (schedule.project.calendar as CalendarModel);
		// Coordinates required when used in vertical mode, since it does not use actual columns

		const startDate = schedule.getDateFromCoordinate(coordinate, 'round', false, true);
		let target = this.schedule.resolveEventRecord(context.target) as Dispatch;
		let canDrop = this.allowSchedulingForPast() || startDate > new Date();

		if (startDate && resource) {
			let endDate = startDate;
			for (let task of tasks) {
				endDate = calendar.calculateEndDate(endDate, DateHelper.asMilliseconds(task.duration, task.durationUnit));
			}

			if (schedule.isHorizontal) {
				// Remove previous highlight (if any)
				const prevRowEl = context.highlightRowEl as HTMLElement;
				prevRowEl?.classList.remove('target-resource');

				// Highlight the hovered row element directly (group headers use different DOM in some modes)
				const targetEl = context.target as HTMLElement;
				let rowEl = (targetEl?.closest?.('.b-grid-row') ?? targetEl?.closest?.('.b-sch-timeaxis-row')) as HTMLElement;

				// Fallback: look up row by record id
				if (!rowEl && resourceRecord?.id != null) {
					const esc = (window as any).CSS?.escape ? (window as any).CSS.escape(String(resourceRecord.id)) : String(resourceRecord.id).replace(/"/g, '\\"');
					rowEl = (schedule.element as HTMLElement)?.querySelector?.(`.b-grid-row[data-id="${esc}"], .b-sch-timeaxis-row[data-id="${esc}"]`) as HTMLElement;
				}

				rowEl?.classList.add('target-resource');
				context.highlightRowEl = rowEl;
			}

			let isValid = true;
			let technicianHasSkills: boolean = true;
			let technicianHasAssets: boolean = true;

			const missingSkillKeys: string[] = [];
			const missingAssetKeys: string[] = [];

			for (let order of orders) {
				if (isTechnician(resource)) {
					if (order.OriginalData?.RequiredSkillKeys?.length > 0) {
						technicianHasSkills &&= window.Helper.Scheduler.hasSkillsForOrder(resource, order.OriginalData, startDate, endDate, missingSkillKeys);
					}
					if (order.OriginalData?.RequiredAssetKeys?.length > 0) {
						technicianHasAssets &&= window.Helper.Scheduler.hasAssetsForOrder(resource, order.OriginalData, startDate, endDate, missingAssetKeys);
					}
				}

				// Don't allow drops anywhere, only allow drops if the drop is on the timeaxis and on top of a Resource
				isValid &&= window.Helper.Scheduler.isValidForResource(order.OriginalData, (resource as ResourceTypes).OriginalData, target) && (schedule.allowOverlap || schedule.isDateRangeAvailable(startDate, endDate, null, resource));
			}

			const missingSkills = Array.from(new Set(missingSkillKeys)).map(key => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.skills, key));
			const missingAssets = Array.from(new Set(missingAssetKeys)).map(key => window.Helper.Scheduler.CreateLookupProxy(ServiceOrder.lookups.assets, key));

			if (isTechnician(resource)) {
				isValid &&= resource.isActiveAtDate(startDate);
			}

			context.canDrop = canDrop;
			// Save reference to resource so we can use it in onTaskDrop
			context.resourceRecord = resourceRecord;
			context.resource = resource;
			context.resources = resources;
			context.valid = isValid;
			context.startDate = startDate;
			context.endDate = endDate;

			if (me.tip && context.valid) {
				let tipHtml = "";
				let startAndEndAreOnSameDay = window.Helper.Date.areOnSameDay(startDate, endDate);

				let date = startDate;
				let taskIndex = 0;
				for (let order of orders) {
					let task = tasks[taskIndex];

					let formattedStartDate = window.moment(date).format(startAndEndAreOnSameDay ? "LT" : "lll");
					date = calendar.calculateEndDate(date, DateHelper.asMilliseconds(task.duration, task.durationUnit));
					let formattedEndDate = window.moment(date).format(startAndEndAreOnSameDay ? "LT" : "lll");
					let warningItems = isTechnician(resource) ? window.Helper.Scheduler.PlanningValidations.GetPlanningValidationTooltipForOrder(order, resource, startDate, date) : [];

					context.valid &&= technicianHasSkills && technicianHasAssets;
					if (isTechnician(resource)) {
						let validationText = "";
						if (!technicianHasSkills) {
							validationText = window.Helper.String.getTranslatedString("TechnicianHasNoneOfTheRequiredOrderSkills")
								.replace("{0}", window.Helper.User.getDisplayName(resource.OriginalData))
								.replace("{1}", missingSkills.join());
						} else if (technicianHasSkills && window._.difference(order.OriginalData.RequiredSkillKeys, resource.ValidSkills.map(s => s.Key)).length > 0) {
							validationText = window.Helper.String.getTranslatedString("TechnicianLacksSomeOfRequiredOrderSkills")
								.replace("{0}", window.Helper.User.getDisplayName(resource.OriginalData))
								.replace("{1}", missingSkills.join());
						}
						if (!technicianHasAssets) {
							validationText = window.Helper.String.getTranslatedString("TechnicianHasNoneOfTheRequiredOrderAssets")
								.replace("{0}", window.Helper.User.getDisplayName(resource.OriginalData))
								.replace("{1}", missingAssets.join());
						} else if (technicianHasAssets && window._.difference(order.OriginalData.RequiredAssetKeys, resource.ValidAssets.map(a => a.Key)).length > 0) {
							validationText = window.Helper.String.getTranslatedString("TechnicianLacksSomeOfRequiredOrderAssets")
								.replace("{0}", window.Helper.User.getDisplayName(resource.OriginalData))
								.replace("{1}", missingAssets.join());
						}

						tipHtml += `
						<div class="b-sch-event-title">${order.name}</div>
						<div class="b-sch-tooltip-startdate">${window.Helper.String.getTranslatedString("StartDate")}: ${formattedStartDate}</div>
						<div class="b-sch-tooltip-enddate">${window.Helper.String.getTranslatedString("EndDate")}: ${formattedEndDate}</div>
						${validationText != "" ? `<div class="restriction-title"><b>${window.Helper.getTranslatedString("Restrictions")}:</b></div>
						<ul class="restriction-list b-sch-tip-invalid">
							<li class="b-sch-tip-message">${validationText}</li>
						</ul>` : ''}
						${warningItems.length > 0 ? `<div class="restriction-title"><b>${window.Helper.String.getTranslatedString("Warning")}:</b></div>
							${window.Helper.Scheduler.PlanningValidations.displayWarningsAsTooltip(warningItems)}` : ''}
						${!canDrop ? `<div class="b-sch-tip-invalid"><div class="b-sch-tip-message">${window.Helper.getTranslatedString('SchedulingForPastIsNotAllowed')}</div></div>` : ''}
					`;
					}

					taskIndex++;
				}

				me.tip.html = tipHtml;
				me.tip.showBy(context.element);
			} else {
				me.tip.html = `
				<div class="b-sch-event-title">${orders.map(o => o.name).join(", ")}</div>
				<div class="restriction-title"><b>${window.Helper.String.getTranslatedString("NotValidFor")}: ${(resource as ResourceTypes).ResourceType}</b></div>`;
				me.tip.showBy(context.element);
			}
		} else {
			// Clear highlight when not over a valid resource/date
			if (schedule.isHorizontal) {
				const prevRowEl = context.highlightRowEl as HTMLElement;
				prevRowEl?.classList.remove('target-resource');
			}
			context.highlightRowEl = null;
			context.canDrop = context.valid = false;
		}
	};

	onDrop = async ({context}) => {
		const me = this;
		const { schedule } = me;

		// Always cleanup UI state
		me.tip?.hide();
		const prevRowEl = context.highlightRowEl as HTMLElement;
		prevRowEl?.classList.remove('target-resource');
		context.highlightRowEl = null;
		schedule.features.eventTooltip.disabled = false;

		const valid = !!context.valid;
		const canDrop = !!context.canDrop;
		const resources = (context.resources ?? []) as ResourceModel[];
		const resource = context.resource as ResourceModel;

		if (!valid || !canDrop || !resource) {
			return;
		}

		const targetDispatch = this.schedule.resolveEventRecord(context.target) as Dispatch;
		const calendar = (resource && (resource["calendar"] as CalendarModel)) ?? (schedule.project.calendar as CalendarModel);
		const tasks = context.tasks as EventModel[];
		const orders = context.orders as GridRowModel[];

		const violationItems: any[] = [];
		for (const r of resources) {
			if (isTechnician(r)) {
				for (const serviceOrder of orders.filter(o => isServiceOrder(o)) as unknown[]) {
					violationItems.push(...window.Helper.Scheduler.PlanningValidations.GetPlanningValidationItemsForOrder(serviceOrder as ServiceOrder, r, context.startDate, context.endDate));
				}
			}
		}

		if (this.enablePlanningConfirmations() && violationItems.length > 0) {
			window.Helper.Scheduler.ShowPopup(violationItems, async () => {
				await this.createEvent(schedule, context.newX, resources, tasks, orders, calendar, targetDispatch);
			});
		} else {
			await this.createEvent(schedule, context.newX, resources, tasks, orders, calendar, targetDispatch);
		}

		schedule.renderContents();
	};
	onDragAbort() {
		this.tip?.hide();
	}
}

namespace("Sms.Scheduler").Drag = Drag;
