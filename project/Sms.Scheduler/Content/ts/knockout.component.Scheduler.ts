import type {Timeline} from "./Timeline";
import {Dispatch, isDispatch} from "./Model/Dispatch";
import {isTechnician, Technician} from "./Model/Technicians";
import type {Drag} from "./Helper/DragHelper";
import {MapPanel} from "./MapPanel";
import {registerComponent} from "@Main/componentRegistrar";
import {
	AssignmentAllocationInterval,
	AssignmentModel,
	BrowserHelper,
	CalendarModel,
	DateField,
	DateHelper,
	Duration,
	EventModel,
	type GridRowModel,
	LocaleManager,
	type MenuItemConfig,
	Model,
	PresetManager,
	ResourceAllocationInterval,
	ResourceModel,
	SchedulerEventModel,
	Splitter,
	Store,
	StringHelper,
	TimeAxis,
	TimeAxisColumn,
	TimeSpan,
	Tooltip,
	ViewPreset,
	SchedulerPro,
	SchedulerResourceModel,
	Menu,
	MenuItem,
} from "@bryntum/schedulerpro";
import {isServiceOrder, ServiceOrder} from "./Model/ServiceOrder";
import {Pipeline} from "./Pipeline";
import {ComboLegend} from "./Legend";
import {MapMarker, MapMarkerType, MapMessageType} from "./Model/MapMessage";
import {SchedulerDetailsViewModel} from "./SchedulerDetailsViewModel";
import {asyncComputed} from "./knockout-async-computed";
import type {HourSpan} from "./Model/HourSpan";
import {DispatchStore, StoreIds} from "./Model/DispatchStore";
import {CrmAssignmentStore} from "./Model/AssignmentStore";
import {CrmCrudManager} from "./Model/CrudManager";
import type {Assignment} from "./Model/Assignment";
import "@bryntum/schedulerpro/locales/schedulerpro.locale.De.js";
import "@bryntum/schedulerpro/locales/schedulerpro.locale.Es.js";
import "@bryntum/schedulerpro/locales/schedulerpro.locale.FrFr.js";
import "@bryntum/schedulerpro/locales/schedulerpro.locale.Hu.js";
import {CrmResourceUtilization} from "./ResourceUtilization";
import {RouteData} from "./Model/RouteData";
import {Consumer} from "./Consumer";
import type {Feature, FeatureCollection} from "geojson";
import moment from "moment";
import {isArticle} from "./Model/Article";
import {isServiceOrderTimePosting} from "./Model/ServiceOrderTimePosting";
import {Absence, isAbsence} from "./Model/Absence";
import {isAbsenceOrder, isArticleDowntime} from "./Model/AbsenceOrder";
import {isJob} from "@Sms.Scheduler/Model/Job";
import {namespace} from "@Main/namespace";
import type {Loader} from '@googlemaps/js-api-loader';
import type {RouteConfig} from "../@types";

registerComponent({
	componentName: "scheduler",
	template: "Sms.Scheduler/Scheduler/TimelinePanel",
	viewModel: {
		createViewModel: async function (data) {
			await LocaleManager.applyLocale('En', {DateHelper: {locale: 'en-GB', weekStartDay: 1}});
			LocaleManager.locales.Fr ??= LocaleManager.locales.FrFr;
			LocaleManager.locale = data.locale;
			const schedulerComponent = new window.Sms.Scheduler.ViewModels.Scheduler(data);
			await schedulerComponent.scheduler.timeAxis.filterBy(schedulerComponent.defaultTimeAxisFilter);
			await schedulerComponent.loadInlineData(data.inlineData);
			await schedulerComponent.applyResourceGroup();
			await schedulerComponent.init();
			return schedulerComponent;
		}
	},
});

type DateRangeChangeEvent = {
	startDate: Date,
	endDate: Date,
	removeAllEvents?: boolean,
	callback?: (unknown) => void,
	processStarted: boolean,
	withdrawn: boolean
};

export class Scheduler extends window.Main.ViewModels.ViewModelBase {
	disposed = false;
	scheduler: Timeline = null;
	drag: Drag = null;
	markerDrag: Drag = null;
	pipeline: Pipeline = null;
	resourceUtilization: CrmResourceUtilization = null;
	pipelineSelectedServiceOrders = ko.observableArray<ServiceOrder>([]);
	eventMarkers: KnockoutObservableArray<MapMarker> = ko.observableArray<MapMarker>([]);
	resourceMarkers: KnockoutObservableArray<MapMarker> = ko.observableArray<MapMarker>([]);
	mapPanel: MapPanel = null;
	//@ts-ignore
	parentViewModel: window.Sms.Scheduler.ViewModels.SchedulerDetailsViewModel = null;
	mapSplitter: Splitter = null;
	horizontalSplitter: Splitter = null;
	defaultTaskDurationInPixel = ko.observable(100);
	crudManager: CrmCrudManager = null;
	attachToStore = new WeakSet<any>();
	detachFromStore = new WeakSet<any>();
	routeConfig: KnockoutObservable<RouteConfig> = ko.observable<RouteConfig>(null);
	dateRangeChangeEventsConsumer: Consumer<DateRangeChangeEvent>;
	headerTooltip: Tooltip = null;
	pipelineSplitter: Splitter = null;
	googleMapsLoader: Loader = null;

	pendingChanges = ko.observable(0);
	pendingChangesTrigger = ko.observable(1).extend({rateLimit: 500});
	resourceStoreChangeTrigger = ko.observable(1).extend({ rateLimit: 500 });

	edgeThreshold: number = 150;
	draggedEventData: {
		eventIds: (string | number)[],
		assignmentIds: (string | number)[]
	} = null;

	requiredPresetIds = {
		weekly: 1,
		daily: 1,
		monthly: 1,
		quarterly: 1
	};

	defaultTimeAxisFilter: (t: TimeAxis) => boolean = null;

	constructor(data) {
		super();
		let self = this;
		self.parentViewModel = data.parentViewModel;

		if (data.modelForParent)
			data.modelForParent(self);

		self.initializeViewPresets();
		const presets = PresetManager.records.filter(p => this.requiredPresetIds[p.id]);

		let initialStartDate = DateHelper.startOf(self.parentViewModel.startDate, 'week');

		ComboLegend.initClass();
		// @ts-ignore
		self.scheduler = new window.Sms.Scheduler.Timeline({
			appendTo: 'schedulertimelinepanel',
			ref: 'timeline',
			stateId: 'timeline',
			startDate: initialStartDate,
			flex: 1,
			presets,
			viewPreset: 'weekly',
			zoomKeepsOriginalTimespan: true,
			keyMap: {
				//@ts-ignore
				ArrowLeft: {handler: () => self.gotoPreviousDate()},
				//@ts-ignore
				ArrowRight: {handler: () => self.gotoNextDate()},
			},
			features: {
				//stripe     : true,
				nonWorkingTime: true,
				eventDragCreate: false,
				eventCopyPaste: false,
				filterBar: true,
				sortable: false,
				tree: true,
				treeGroup: {
					levels: [],
				},
				dependencies: false,
				resourceTimeRanges: true,
				eventNonWorkingTime: {
					disabled: false
				},
				timeAxisHeaderMenu: {
					items: {
						eventsFilter: false,
						zoomLevel: false,
						currentTimeLine: {
							text: window.Helper.String.getTranslatedString('ShowCurrentTime')
						}
					}
				},

				scheduleTooltip: {
					hideForNonWorkingTime: false,
					//@ts-ignore
					getText(date: Date, event: Dispatch | Absence | ServiceOrderTimePosting, resource: ResourceTypes) {
						let calendar = resource.calendar || this.owner.project.effectiveCalendar;
						let isWorkingTime: boolean = false;
						let tooltipText: string = null;
						if (calendar?.isWorkingTime) {
							isWorkingTime = calendar.isWorkingTime(date);
							tooltipText = isWorkingTime ? '' : window.Helper.getTranslatedString("NonWorkingTime");
						}
						if (isWorkingTime && resource.timeRanges) {
							let timeRange = resource.timeRanges.find(t => DateHelper.timeSpanContains(DateHelper.startOf(t.dates[0]), DateHelper.endOf(t.dates[t.dates.length - 1]), DateHelper.startOf(date), DateHelper.endOf(date)));
							if (timeRange != null) {
								tooltipText = timeRange.name;
							}
						}
						return tooltipText;
					}
				},
				timeRanges: {
					showCurrentTimeLine: true
				},
				eventDrag: {
					validatorFn(data) {
						const {startDate} = data;
						let canDrop = Boolean(this.owner.AllowSchedulingForPast()) || Boolean(startDate > new Date());
						if (!canDrop) {
							return {
								valid: false,
								message: window.Helper.String.getTranslatedString("SchedulingForPastIsNotAllowed")
							};
						}
						return window.Sms.Scheduler.Timeline.eventDragValidator(data);
					}
				},
				calendarHighlight: {
					// visualize resource calendars while interacting with events
					calendar: 'resource',
					collectAvailableResources: (context: {
						scheduler: SchedulerPro,
						eventRecords: SchedulerEventModel[]
					}) => {
						return (context.scheduler as Timeline).getResourcesToHighlight(context.eventRecords);
					}
				},
				cellTooltip: {
					allowOver: true,
					scrollable: true,
					width: '28em',
					tooltipRenderer: (data) => {
						const record = data.record as ResourceModel;
						if (isTechnician(record)) {
							let profile = ko.unwrap(this.parentViewModel.profile);
							let tooltipProps = ko.unwrap(profile.ClientConfig.ResourceTooltip);
							if (tooltipProps.length === 0) {
								return window.Helper.getTranslatedString('TooltipIsNotConfigured');
							}
							let tooltipContext = {"Resource": record};

							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps, "Resource");
						} else if (isArticle(record)) {
							let tooltipProps = [
								"Resource.ItemNo",
								"Resource.Description"
							];
							let tooltipContext = {"Resource": record};
							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps, "Resource");
						}

						return "";
					},
					hideOnDelegateChange: true,
					hoverDelay: DateHelper.as('ms', self.parentViewModel.profile().ClientConfig.TooltipDisplayDelay ?? 0, 's'),
					hideDelay: 0
				},
				eventTooltip: {
					allowOver: true,
					scrollable: true,
					width: '28em',
					//@ts-expect-error
					template: (data: { eventRecord: EventModel }) => {
						if (isDispatch(data.eventRecord)) {
							let profile = ko.unwrap(this.parentViewModel.profile);
							let tooltipProps = ko.unwrap(profile.ClientConfig.ServiceOrderDispatchTooltip);
							if (tooltipProps.length === 0) {
								return window.Helper.getTranslatedString('TooltipIsNotConfigured');
							}
							let tooltipContext = {
								"ServiceOrder": data.eventRecord.ServiceOrder,
								"ServiceOrderDispatch": data.eventRecord
							};

							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps, "ServiceOrderDispatch");
						} else if (isAbsence(data.eventRecord)) {
							let tooltipProps = [
								"AbsenceType.Value",
								"Absence.Description",
								"#.#",
								"Absence.From",
								"Absence.To",
							];
							let tooltipContext = {
								"Absence": data.eventRecord.OriginalData,
								"AbsenceType": data.eventRecord.AbsenceTypeData
							};
							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps, "Absence");
						}
						return "";
					},
					hideOnDelegateChange: true,
					hoverDelay: DateHelper.as('ms', self.parentViewModel.profile().ClientConfig.TooltipDisplayDelay ?? 0, 's'),
					hideDelay: 0
				},
				resourceNonWorkingTime: {},
				nestedEvents: {
					// Stack nested events initially (can be changed from the toolbar)
					eventLayout: 'stack',
					// Grow nested events a bit, compared to the default which is 30
					eventHeight: 25,
					// Reserve more space above the nested events container
					headerHeight: 15,
					// Space between nested events
					barMargin: 1,
					allowNestingOnDrop: false,
					allowDeNestingOnDrop: false
				},
				cellMenu: {
					items: {
						openDetails: {
							text: `${window.Helper.getTranslatedString("Details")}`,
							icon: 'b-fa b-fa-fw b-fa-folder-open',
							weight: 1,
							onItem({record}) {
								if (isTechnician(record)) {
									window.open(`#/Main/User/DetailsTemplate/${record.OriginalData.Id}`, window.Helper.Scheduler.URLTarget())
								} else if (isArticle(record)) {
									window.open(`#/Crm.Article/Article/DetailsTemplate/${record.OriginalData.Id}`, window.Helper.Scheduler.URLTarget())
								}
							}
						},
						getRoute: {
							weight: 250,
							icon: 'b-fa b-fa-solid b-fa-route',
							text: `${window.Helper.getTranslatedString("GetRoute")}`,
							async onItem({record}) {
								const technician = record as Technician;
								self.routeConfig({
									routeData: [new RouteData(ko.observable(technician.OriginalData.Id), ko.observable(self.parentViewModel.profile().ClientConfig.RouteColors[0]))],
									useTechnicianHomeAddressAsOrigin: false,
									useTechnicianHomeAddressAsFinalDestination: false,
									getRoutePerDay: false
								});

								await self.getRoute(self.routeConfig());
							}
						},
						orderResources: {
							separator: true,
							icon: 'b-fa b-fa-solid b-fa-arrows-up-down',
							text: `${window.Helper.getTranslatedString("ReorderResources")}`,
							async onItem(event) {
								const technicians = self.scheduler.resourceStore.records.filter(isTechnician);
								// Show custom editor
								$('#smModal').modal("show",
									{
										route: `Sms.Scheduler/Scheduler/ResourceReorder/`,
										viewModel: {technicians: technicians, parentViewModel: self.parentViewModel}
									});
								// Prevent built in editor
								return false;
							},
						},
					},
					processItems({items, record}) {
						let visibleEvents = self.scheduler.eventStore.getEvents({
							resourceRecord: record as SchedulerResourceModel,
							startDate: self.scheduler.timeAxis.startDate,
							endDate: self.scheduler.timeAxis.endDate,
							filter: r => {
								return isDispatch(r) && r.ServiceOrder.Latitude != null && r.ServiceOrder.Longitude != null
							}
						});
						if ((visibleEvents as Model[]).length < 1) {
							items.getRoute = false;
						}
						if (!isTechnician(record)) {
							items.orderResources = false;
						}
						if (isTechnician(record)) {
							if (!self.parentViewModel.autosave()) {
								let orderResources = items.orderResources as MenuItemConfig;
								orderResources.disabled = true;
								orderResources.tooltip = window.Helper.String.getTranslatedString("FunctionalityNotAvailableWarning");
							}
						}
						if (!self.parentViewModel.autosave()) {
							let removeRow = items.removeRow as MenuItemConfig;
							removeRow.disabled = true;
							removeRow.tooltip = window.Helper.String.getTranslatedString("FunctionalityNotAvailableWarning");
						}
						if (!isTechnician(record) && !isArticle(record)) {
							//@ts-expect-error
							items.openDetails.hidden = true;
						}
						if(!window.AuthorizationManager.currentUserHasPermission("Scheduler::EditProfile")) {
							items.removeRow = false;
						}
						if (!(isTechnician(record) && window.AuthorizationManager.currentUserHasPermission("User::Read")
							|| !isTechnician(record) && window.AuthorizationManager.currentUserHasPermission("Article::Read"))) {
							items.openDetails = false;
						}
					}
				},
				scheduleMenu: {
					items: {
						addEvent: !window.AuthorizationManager.currentUserHasPermission("Adhoc::Create") ? false : {
							text: window.Helper.String.getTranslatedString('CreateAdHocServiceOrderHere'),
							icon: 'b-fa b-fa-fw b-fa-solid b-fa-plus',
							onItem(event) {
								//@ts-expect-error
								let {date, resourceRecord} = event;
								$('#xlModal').modal("show",
									{
										route: `Sms.Scheduler/Scheduler/AdHocTemplate/`,
										viewModel: {
											date: date,
											resourceRecord: resourceRecord,
											parentViewModel: self.parentViewModel
										} //using it to pass the event to the constructor
									});
							}
						},
						showNonWorkingHours: {
							separator: true,
							text: window.Helper.String.getTranslatedString('ShowNonWorkingHours'),
							icon: 'b-fa b-fa-fw b-fa-eye',
							onItem() {
								self.scheduler.timeAxis.clearFilters();
							}
						},
						hideNonWorkingHours: {
							text: window.Helper.String.getTranslatedString('HideNonWorkingHours'),
							icon: 'b-fa b-fa-fw b-fa-eye-slash',
							onItem() {
								self.scheduler.timeAxis.filterBy(self.defaultTimeAxisFilter);
							}
						},
						onlyForThisDayNonWorkingHours: {
							text: window.Helper.String.getTranslatedString('ShowDayNonWorkingHours'),
							icon: 'b-fa b-fa-fw b-fa-eye-low-vision',
							//@ts-expect-error
							onItem({date}) {
								let startDate = DateHelper.startOf(date as Date, 'day');
								let endDate = DateHelper.add(startDate, 1, 'day');

								self.scheduler.timeAxis.filterBy(t =>
									(startDate <= t.endDate && t.startDate <= endDate) || self.defaultTimeAxisFilter(t)
								);
							}
						},
					},
					processItems: event => {
						const presetId = self.scheduler.viewPreset["id"] as string;

						if (presetId == 'daily') {
							event.items.showNonWorkingHours = false;
							event.items.hideNonWorkingHours = false;
							event.items.onlyForThisDayNonWorkingHours = false;
						}

						if (!self.parentViewModel.autosave()) {
							let addEvent = event.items.addEvent as MenuItemConfig;
							addEvent.disabled = true;
							addEvent.tooltip = window.Helper.String.getTranslatedString("FunctionalityNotAvailableWarning");
						}
					}
				},
				eventMenu: {
					items: {
						cutEvent: false,
						splitEvent: false,
						promoteToLeader: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							text: `${window.Helper.getTranslatedString("PromoteToLeader")}`,
							icon: 'b-fa b-fa-solid b-fa-crown',
							weight: 100,
							async onItem({eventRecord, resourceRecord}) {
								const scheduler = this.owner.owner as Timeline;

								if(!isTechnician(resourceRecord)) {
									return;
								}

								eventRecord.set({
									Username: resourceRecord.ResourceKey
								});


								scheduler.refreshRows();
							}
						},
						confirmDispatch: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							separator: true,
							text: `${window.Helper.getTranslatedString("Release")}`,
							icon: 'zmdi zmdi-mail-send',
							weight: 100,
							async onItem(event) {
								const scheduler = this.owner.owner;
								let selectedEvents = window.Sms.Scheduler.Timeline.getEventContextMenuSelectedItems(scheduler, event["eventRecord"]);

								for (let selectedEvent of selectedEvents) {
									if (isDispatch(selectedEvent)) {
										selectedEvent.set({
											StatusKey: "Released"
										});
									}
								}
							}
						},
						fixDispatch: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							text: `${window.Helper.getTranslatedString("FixDispatch")}`,
							weight: 100,
							async onItem(event) {
								const scheduler = this.owner.owner;
								let selectedEvents = window.Sms.Scheduler.Timeline.getEventContextMenuSelectedItems(scheduler, event["eventRecord"]);

								for (let selectedEvent of selectedEvents) {
									if (isDispatch(selectedEvent)) {
										selectedEvent.set({
											IsFixed: event.item.checked
										});
									}
								}
							}
						},
						completeDispatch: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							text: `${window.Helper.getTranslatedString("Complete")}`,
							icon: 'zmdi zmdi-mail-send',
							weight: 100,
							onItem({eventRecord}) {
								if(!isDispatch(eventRecord)) {
									return;
								}
								let dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch = (eventRecord).OriginalData;
								let element: HTMLElement = document.querySelector(`[data-ref="completeDispatch"]`);

								ko.cleanNode(element);
								const ddvm = { init: () => self.reloadDateRange() };
								ko.applyBindings(ddvm, element);

								element.dataset.route = 'Crm.Service/Dispatch/ChangeStatusTemplate/' + dispatch.Id;
								element.dataset.toggle = "modal";
								element.dataset.target = "#modal";
							}
						},
						openDispatch: {
							separator: true,
							text: `${window.Helper.getTranslatedString("Open Dispatch")}`,
							icon: 'zmdi zmdi-layers',
							weight: 200,
							onItem({eventRecord}) {
								const scheduler = this.owner.owner;
								let selectedEvents = window.Sms.Scheduler.Timeline.getEventContextMenuSelectedItems(scheduler, eventRecord);

								for (let selectedEvent of selectedEvents) {
									if (isDispatch(selectedEvent) &&
										(selectedEvent?.OriginalData.entityState == $data.EntityState.Unchanged || selectedEvent?.OriginalData.entityState === undefined)) {
										window.open(`#/Crm.Service/Dispatch/DetailsTemplate/${selectedEvent.id}`, window.Helper.Scheduler.URLTarget())
									}
								}
							}
						},
						openOrder: {
							text: `${window.Helper.getTranslatedString("Open Order")}`,
							icon: 'b-fa b-fa-fw b-fa-folder-open',
							weight: 200,
							onItem({eventRecord}) {
								const scheduler = this.owner.owner;
								let selectedEvents = window.Sms.Scheduler.Timeline.getEventContextMenuSelectedItems(scheduler, eventRecord);

								for (let selectedEvent of selectedEvents) {
									if (isDispatch(selectedEvent)) {
										window.open(`#/Crm.Service/ServiceOrder/DetailsTemplate/${selectedEvent.OriginalData.OrderId}`, window.Helper.Scheduler.URLTarget())
									}
								}
							}
						},
						deleteEvent: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							weight: 250,
							async onItem({eventRecord}) {
								if (isDispatch(eventRecord)) {
									//@ts-expect-error
									if ((await window.database.CrmService_ServiceOrderDispatch.IsReplicated({dispatchId: eventRecord.OriginalData.Id}).first()).value) {
										window.Helper.Scheduler.ShowPopup([{
											type: 'label',
											cls: 'scheduler-warning',
											text: `${eventRecord.name}: ${window.Helper.String.getTranslatedString('AlreadyReplicatedWarning')}`,
											style: 'width: 100%',
										}]);
										return;
									}
								}
								const scheduler = this.owner.owner as Timeline;
								scheduler.eventStore.remove(eventRecord);
							}
						},
						unassignEvent: {
							text: `${window.Helper.getTranslatedString("Unassign")}`,
							weight: 260,
							onItem(event: {
								item: MenuItem;
								menu: Menu;
								domEvent: Event;
								record?: Model;
								column?: Model;
								columnRecord?: Model;
								taskRecord?: Model;
								eventRecord?: Model;
								resourceRecord?: Model;
								assignmentRecord?: Model;
								checked: boolean;
							}): void {
								const { assignmentRecord, eventRecord } = event;
								if (!assignmentRecord || !eventRecord) {
									return;
								}
		
								const scheduler = this.owner.owner as Timeline;
								if (isDispatch(eventRecord)) {
									let validationItems = [];
									const resource = (assignmentRecord as Assignment).resource as ResourceModel;
									const serviceOrder = (eventRecord).ServiceOrder;
			
									if (serviceOrder.PreferredTechnician != null && serviceOrder.PreferredTechnician == (assignmentRecord as Assignment).resourceId) {
										validationItems.push({
											type: 'label',
											cls: 'scheduler-warning',
											text: `${serviceOrder.name}: ${window.Helper.String.getTranslatedString('PreferredTechnicianRemovedWarning').replace("{0}", window.Helper.User.getDisplayName(serviceOrder.OriginalData.PreferredTechnicianUser))}`,
											style: 'width: 100%',
										});
									}
			
									const assignments = scheduler.assignmentStore.getAssignmentsForEvent(eventRecord as TimeSpan);
									const otherAssignmentsHasPreferredUserGroups = assignments.filter(a => a.id != (assignmentRecord as Assignment).id).some(a => isTechnician(resource) && resource.UserGroups?.includes(serviceOrder.PreferredUserGroup));
			
									if (serviceOrder.PreferredUserGroup != null && isTechnician(resource) && resource.UserGroups.includes(serviceOrder.PreferredUserGroup) && !otherAssignmentsHasPreferredUserGroups) {
										validationItems.push({
											type: 'label',
											cls: 'scheduler-warning',
											text: `${serviceOrder.name}: ${window.Helper.String.getTranslatedString('PreferredUserGroupRemovedWarning').replace("{0}", serviceOrder.PreferredUserGroup)}`,
											style: 'width: 100%',
										});
									}
			
									if (validationItems.length > 0) {
										window.Helper.Scheduler.ShowPopup(validationItems, () => {
											self.handleAssignmentRemoval(scheduler, assignmentRecord as Assignment, eventRecord);
										});
									} else {
										self.handleAssignmentRemoval(scheduler, assignmentRecord as Assignment, eventRecord);
									}
								}
							}
						},
						editEvent: false,
						editTimelineEvent: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							separator: true,
							weight: 300,
							icon: 'b-fa b-fa-pencil',
							onItem({assignmentRecord, eventRecord}) {
								if (!isServiceOrderTimePosting(eventRecord)) {
									// Show custom editor
									$('#lgModal').modal("show",
										{
											route: `Sms.Scheduler/Scheduler/EditTemplate/`,
											viewModel: {eventRecord: eventRecord, parentViewModel: self.parentViewModel} //using it to pass the event to the constructor
										});
								}
								// Prevent built in editor
								return false;
							}
						},
						groupDispatches: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							icon: 'b-fa b-fa-link',
							async onItem(event: {
								item: MenuItem;
								menu: Menu;
								domEvent: Event;
								record?: Model;
								column?: Model;
								columnRecord?: Model;
								taskRecord?: Model;
								eventRecord?: Model;
								resourceRecord?: Model;
								assignmentRecord?: Model;
								checked: boolean;
							}) {
								const { assignmentRecord, eventRecord } = event;
								const scheduler = this.owner.owner as Timeline;
								if (isDispatch(eventRecord)) {
									let validationItems = [];
									//@ts-ignore
									const resource = assignmentRecord?.resource as ResourceModel;
									const serviceOrder = eventRecord.ServiceOrder;
									//@ts-ignore
									if (serviceOrder.PreferredTechnician != null && serviceOrder.PreferredTechnician == assignmentRecord?.resourceId) {
										validationItems.push({
											type: 'label',
											cls: 'scheduler-warning',
											text: `${serviceOrder.name}: ${window.Helper.String.getTranslatedString('PreferredTechnicianRemovedWarning').replace("{0}", window.Helper.User.getDisplayName(serviceOrder.OriginalData.PreferredTechnicianUser))}`,
											style: 'width: 100%',
										});
									}

									const assignments = scheduler.assignmentStore.getAssignmentsForEvent(eventRecord as TimeSpan);
									const otherAssignmentsHasPreferredUserGroups = assignments.filter(a => a.id != assignmentRecord?.id).some(a => isTechnician(resource) && resource.UserGroups?.includes(serviceOrder.PreferredUserGroup));

									if (serviceOrder.PreferredUserGroup != null && isTechnician(resource) && resource.UserGroups.includes(serviceOrder.PreferredUserGroup) && !otherAssignmentsHasPreferredUserGroups) {
										validationItems.push({
											type: 'label',
											cls: 'scheduler-warning',
											text: `${serviceOrder.name}: ${window.Helper.String.getTranslatedString('PreferredUserGroupRemovedWarning').replace("{0}", serviceOrder.PreferredUserGroup)}`,
											style: 'width: 100%',
										});
									}

									if (validationItems.length > 0) {
										window.Helper.Scheduler.ShowPopup(validationItems, () => {
											self.handleAssignmentRemoval(scheduler, assignmentRecord as Assignment, eventRecord);
										});
									} else {
										self.handleAssignmentRemoval(scheduler, assignmentRecord as Assignment, eventRecord);
									}
								}
							},
							separator: true,
							text: `${window.Helper.getTranslatedString("GroupDispatches")}`,
							weight: 300
						},
						ungroupDispatches: !window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") ? false : {
							icon: 'b-fa b-fa-link-slash',
							onItem({eventRecord}) {
								if (isDispatch(eventRecord)) {
									return;
								}

								const scheduler = this.owner.owner;
								let selectedDispatches = window.Sms.Scheduler.Timeline.getEventContextMenuSelectedItems(scheduler, eventRecord).filter(isDispatch);
								for (const selectedDispatch of selectedDispatches) {
									selectedDispatch.set({
										SchedulingGroupId: null
									});
								}

								self.scheduler.refreshRows();
							},
							separator: true,
							text: `${window.Helper.getTranslatedString("UngroupDispatches")}`,
							weight: 300
						}
					},
					processItems(event) {
						const {resourceRecord, eventRecord, items} = event;

						if (!window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") || isServiceOrderTimePosting(eventRecord)) {
							items.editTimelineEvent = false;
							items.deleteEvent = false;
							items.completeDispatch = false;
							items.confirmDispatch = false;
							items.fixDispatch = false;
							items.groupDispatches = false;
							items.openDispatch = false;
							items.openOrder = false;
							items.promoteToLeader = false;
							items.unassignEvent = false;
							items.ungroupDispatches = false;

							return;
						}
						if (!isDispatch(eventRecord) || isTechnician(resourceRecord) && this.project.assignmentStore.getAssignmentsForEvent(eventRecord).filter(a => a.OriginalData instanceof Sms.Scheduler.Rest.Model.SmsScheduler_DispatchPersonAssignment).length <= 1) {
							items.unassignEvent = false;
						}
						if (isDispatch(eventRecord)) {
							let selectedDispatchIds = self.scheduler.selectedAssignments.map(x => (x as Assignment).OriginalData.DispatchKey);
							let selectedDispatches = (self.scheduler.events as Model[]).filter((x): x is Dispatch => isDispatch(x) && selectedDispatchIds.includes(x.OriginalData.Id));
							let uniqueGroupIds = window._.uniq(selectedDispatches.map(x => x.OriginalData.ExtensionValues.SchedulingGroupId));
							if (selectedDispatches.length <= 1 || uniqueGroupIds.filter(x => x !== null).length > 1 || !uniqueGroupIds.some(x => x === null)) {
								items.groupDispatches = false;
							}
							if (uniqueGroupIds.filter(x => x !== null).length === 0) {
								items.ungroupDispatches = false;
							}

							let dispatch = eventRecord.OriginalData;
							(items.editTimelineEvent as MenuItemConfig).text = `${window.Helper.getTranslatedString("EditDispatch")}`;
							(items.deleteEvent as MenuItemConfig).text = `${window.Helper.getTranslatedString("DeleteDispatch")}`;

							if (!(dispatch.StatusKey === 'InProgress' || dispatch.StatusKey === 'SignedByCustomer')) {
								items.completeDispatch = false;
							} else if (!self.parentViewModel.autosave()) {
								const completeDispatch = items.completeDispatch as MenuItemConfig;
								completeDispatch.disabled = true;
								completeDispatch.tooltip = window.Helper.String.getTranslatedString("FunctionalityNotAvailableWarning");
							}

							if (dispatch.StatusKey !== 'Scheduled') {
								items.confirmDispatch = false
							}
							if (dispatch.IsActive) {
								//@ts-ignore
								items.fixDispatch.checked = dispatch.IsFixed;
							} else {
								items.fixDispatch = false;
							}
							if (!eventRecord.dispatchInEditableState) {
								items.fixDispatch = false;
							}
							if (dispatch.StatusKey !== 'Scheduled' || dispatch.IsFixed) {
								items.deleteEvent = false;
							}
							if (!isTechnician(resourceRecord) || dispatch.Username == resourceRecord.ResourceKey) {
								items.promoteToLeader = false;
							}
							if (dispatch.entityState == $data.EntityState.Modified) {
								let openDispatch = items.openDispatch as MenuItemConfig;
								openDispatch.disabled = true;
								openDispatch.tooltip = window.Helper.String.getTranslatedString("AutosaveOffWarning");
							}
						} else if (isAbsence(eventRecord)) {
							(items.editTimelineEvent as MenuItemConfig).text = `${window.Helper.getTranslatedString("EditAbsence")}`;
							(items.deleteEvent as MenuItemConfig).text = `${window.Helper.getTranslatedString("DeleteAbsence")}`;
							items.completeDispatch = false;
							items.confirmDispatch = false;
							items.fixDispatch = false;
							items.groupDispatches = false;
							items.openDispatch = false;
							items.openOrder = false;
							items.promoteToLeader = false;
							items.unassignEvent = false;
							items.ungroupDispatches = false;
						}
					}
				}
			},
			project: {
				skipNonWorkingTimeWhenSchedulingManually: false,
				skipNonWorkingTimeInDurationWhenSchedulingManually: false,
				addConstraintOnDateSet: false,
				calendar: self.parentViewModel.rootCalendar.value,

				eventStore: new DispatchStore(),
				assignmentStore: new CrmAssignmentStore(),
				resourceStore: {
					sorters: [
						{field: 'SortOrder', ascending: true},
						{field: 'Lastname', ascending: true}
					]
				},
				//@ts-ignore
				serviceOrderStore: {
					removeUnassignedEvent: false,
					modelClass: ServiceOrder,
					storeId: "serviceOrders"
				},
				listeners: {
					beforeSend({requestConfig}) {
						requestConfig.params.profile = self.parentViewModel.profile()?.Id;
					}
				}
			},
			listeners: {
				presetChange: function (e: {
					startDate: Date,
					endDate: Date,
					from: ViewPreset,
					to: ViewPreset,
					zoomDate?: Date
				}) {
					let toPresetId = e.to.id;
					let df = this.widgetMap.datePicker as DateField;

					if (e.zoomDate) {
						df.value = e.zoomDate;
					}

					let selectedDate = df.value as Date;

					let newStartDate: Date = null;
					let newEndDate: Date = null;

					if (toPresetId == 'monthly') {
						newStartDate = DateHelper.startOf(selectedDate, "month");
						newEndDate = DateHelper.add(newStartDate, 1, 'month');

					} else if (toPresetId == 'weekly') {
						newStartDate = DateHelper.startOf(selectedDate, "week");
						newEndDate = DateHelper.add(newStartDate, 1, 'week');

					} else if (toPresetId == 'daily') {
						newStartDate = DateHelper.startOf(selectedDate, "day");
						newEndDate = DateHelper.add(newStartDate, 1, 'day');
					} else if (toPresetId == 'quarterly') {
						newStartDate = DateHelper.startOf(selectedDate, "quarter");
						newEndDate = DateHelper.add(newStartDate, 1, 'quarter');
					} else
						throw new Error("Unknown preset id.");

					queueMicrotask(() => self.scheduler.setTimeSpan(newStartDate, newEndDate));
					queueMicrotask(() => self.defaultTaskDurationInPixel(self.getDefaultTaskDurationInPixel()));

					//this is to fix hidden headers that sometimes happens!
					//@ts-ignore
					queueMicrotask(() => self.scheduler.timeAxis.refreshData());
				},
				beforeEventDropFinalize(data) {
					let {assignmentRecords, eventRecords} = data.context;
					for (let assignmentRecord of assignmentRecords) {
						const event = assignmentRecord.event as EventModel;
						const resource = assignmentRecord.resource as ResourceModel;
						let {startDate, endDate} = event;
						if (isDispatch(event) && isTechnician(resource) && (!window.Helper.Scheduler.hasSkillsForOrder(resource, event.ServiceOrder.OriginalData, startDate as Date, endDate as Date) || !window.Helper.Scheduler.hasAssetsForOrder(resource, event.ServiceOrder.OriginalData, startDate as Date, endDate as Date))) {
							assignmentRecords.splice(assignmentRecords.indexOf(assignmentRecord), 1);
							eventRecords.splice(eventRecords.indexOf(event), 1);
							if (assignmentRecords.length == 1) {
								data.context.valid = false;
								return;
							}
						}
					}
					self.draggedEventData = null;

					queueMicrotask(() =>
						$('.overlayDiv').each(function () {
							$(this).hide();
						})
					);

					if (!JSON.parse(window.Sms.Scheduler.Settings.WorkingTime.IgnoreWorkingTimesInEndDateCalculation)) {
						for (const eventRecord of eventRecords) {
							if (eventRecord?.OriginalData instanceof Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch && eventRecord?.manuallyScheduled) {
								eventRecord.manuallyScheduled = false;
							}
						}
					}
				},
				beforeEventResizeFinalize(e) {
					const {context} = e;
					let {
						assignmentRecord,
						eventRecord,
						startDate,
						endDate,
						originalStartDate,
						originalEndDate
					} = context;
					if (eventRecord.OriginalData instanceof Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch) {
						if (!window.Helper.Scheduler.hasSkillsForOrder(assignmentRecord.resource, eventRecord.ServiceOrder.OriginalData, startDate, endDate) || !window.Helper.Scheduler.hasAssetsForOrder(assignmentRecord.resource, eventRecord.ServiceOrder.OriginalData, startDate, endDate)) {
							context.valid = false;
							context.finalize(false);
							return false;
						}

						if (self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchForceMaximumDuration && self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration) {
							const calendar = (context.resourceRecord).calendar as CalendarModel;

							const newDurationInMinutes = eventRecord.manuallyScheduled ?
								moment(endDate).diff(startDate, "minutes") :
								DateHelper.as("minute", calendar.calculateDurationMs(startDate, endDate), "ms");
							const oldDurationInMinutes = eventRecord.manuallyScheduled ?
								moment(originalEndDate).diff(originalStartDate, "minutes") :
								DateHelper.as("minute", calendar.calculateDurationMs(originalStartDate, originalEndDate), "ms");

							const startHasChanged = !moment(startDate).isSame(originalStartDate);
							const endHasChanged = !moment(endDate).isSame(originalEndDate);

							if (newDurationInMinutes > oldDurationInMinutes && self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration < newDurationInMinutes) {
								if (!startHasChanged && endHasChanged) {
									//only end has changed
									if (eventRecord.manuallyScheduled) {
										context.endDate = moment(startDate).add(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration, "minutes").toDate();
									} else {
										context.endDate = calendar.calculateEndDate(startDate, DateHelper.asMilliseconds(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration, "minute"));
									}
								} else if (startHasChanged && !endHasChanged) {
									//only start has changed
									if (eventRecord.manuallyScheduled) {
										context.startDate = moment(endDate).subtract(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration, "minutes").toDate();
									} else {
										context.startDate = calendar.calculateStartDate(endDate, DateHelper.asMilliseconds(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration, "minute"));
									}
								} else if (startHasChanged && endHasChanged) {
									//both have changed! this should not happen but lets cancel the change to be safe
									context.valid = false;
									context.finalize(false);
									return false;
								}
							}
						}
						if (self.parentViewModel.profile().ClientConfig.AllowSchedulingForPast === false && startDate < new Date()) {
							context.valid = false;
							context.finalize(false);
							return false;
						}
					}
					if (e?.eventName != "eventPartialResize") {
						context.finalize(true);
						return true;
					}
					return true;
				},
				eventResizeEnd(e: { source : Scheduler, changed : boolean, eventRecord : EventModel }) {
					self.draggedEventData = null;

					queueMicrotask(() =>
						$('.overlayDiv').each(function () {
							$(this).hide();
						})
					);
				},
				eventPartialResize(e) {
					if (this?.listeners?.beforeeventresizefinalize?.[0]?.fn?.name == "beforeEventResizeFinalize") {
						return this.listeners.beforeeventresizefinalize[0].fn(e);
					}
					return true;
				},
				eventResizeStart(e: { source : Scheduler, eventRecord : EventModel, resourceRecord : ResourceModel, event : MouseEvent }) {
					if (!JSON.parse(window.Sms.Scheduler.Settings.WorkingTime.IgnoreWorkingTimesInEndDateCalculation) &&
						isDispatch(e.eventRecord) && e?.eventRecord?.manuallyScheduled) {
						e.eventRecord.manuallyScheduled = false;
					}
					self.draggedEventData = {
						eventIds: [e.eventRecord.id],
						assignmentIds: self.scheduler.assignmentStore.getAssignmentsForEvent(e.eventRecord).map(r => r.id),
					};
					self.showOverlays();
				},
				// Listener called before the built in editor is shown
				beforeEventEdit({eventRecord}: { eventRecord: EventModel }) {
					if (window.AuthorizationManager.currentUserHasPermission("Scheduler::Edit") && !isServiceOrderTimePosting(eventRecord)) {
						// Show custom editor
						$('#lgModal').modal("show",
							{
								route: `Sms.Scheduler/Scheduler/EditTemplate/`,
								viewModel: {eventRecord: eventRecord, parentViewModel: self.parentViewModel} //using it to pass the event to the constructor
							});
					}
					// Prevent built in editor
					return false;
				},
				beforeAssignmentDelete({assignmentRecords, source, context}) {
					let assignmentsToRemove = assignmentRecords.filter(a => a.event.readOnly === false);
					if (assignmentsToRemove.length > 0) {
						self.scheduler.eventStore.remove(assignmentsToRemove.map(a => a.eventId));
						self.scheduler.assignmentStore.remove(assignmentsToRemove);
					}
					return false;
				},
				beforeEventDrag(data: {
					source: Scheduler,
					eventRecord: EventModel,
					resourceRecord: ResourceModel,
					eventRecords: EventModel[],
					assignmentRecords: Assignment[],
					event: PointerEvent
				}) {
					let {eventRecords, assignmentRecords} = data;
					this.selectGroupedAssignments(assignmentRecords);
					self.draggedEventData = {
						eventIds: eventRecords.map(e => e.id),
						assignmentIds: assignmentRecords.map(a => a.id)
					};
					self.showOverlays();
				},
				eventDragAbortFinalized(data) {
					$('.overlayDiv').each(function () {
						$(this).hide();
					});
				}
			},
			snap: true,
			tbar: [
				{
					// @ts-ignore
					type: 'comboLegend',
					width: "40%",
					store: {
						data: window.Helper.Scheduler.InitComboLegendsItems(self.parentViewModel.lookups.serviceOrderDispatchStatuses.$array)
					},
					onSelect({source}) {
						// @ts-ignore
						self.scheduler.eventStore.clearFilters(this.records.length === 0);
						self.clearRoute(false);

						if (this.records.length > 0) {
							const statuses: string[] = this.records.filter(item => item.type == 'Status').map(item => item.value);
							if (statuses.length > 0) {
								self.scheduler.eventStore.filter(eventRecord => statuses.some(item => eventRecord.OriginalData.StatusKey == item || eventRecord.parent?.OriginalData?.StatusKey == item || (item === 'Absence' ? isAbsence(eventRecord) : false)));
							}

							const entityStates: number[] = this.records.filter(item => item.type == 'EntityState').map(item => item.value);
							if (entityStates.length > 0) {
								self.scheduler.eventStore.filter(eventRecord => entityStates.includes(eventRecord?.OriginalData?.entityState));
							}
						}
					}
				},
				'->',
				{
					label: window.Helper.getTranslatedString('Go to date'),
					inputWidth: '5%',
					width: 'auto',
					type: 'datefield',
					ref: 'datePicker',
					editable: false,
					picker: {
						showWeekColumn: true,
						disableWeekends: false
					},
					step: '1d',
					listeners: {
						change({userAction, value, oldValue}) {
							if (userAction) {
								let presetId = self.scheduler.viewPreset["id"] as string;

								if (presetId == 'monthly')
									self.scheduler.setStartDate(DateHelper.startOf(DateHelper.clearTime(value), 'month'));
								else if (presetId == 'weekly')
								{
									const startOfWeek = DateHelper.startOf(value, "week");
									const endOfWeek = DateHelper.add(startOfWeek, 7, "days");
									self.scheduler.setTimeSpan(startOfWeek, endOfWeek);
								}
								else if (presetId == 'daily')
									self.scheduler.setStartDate(DateHelper.startOf(DateHelper.clearTime(value), 'day'));
								else if (presetId == 'quarterly')
									self.scheduler.setStartDate(DateHelper.startOf(DateHelper.clearTime(value), 'quarter'));
								else
									throw new Error("Unknown preset id.");
							}
						}
					},
					highlightExternalChange: false,
					value: initialStartDate
				},
				{
					label: window.Helper.getTranslatedString('View'),
					type: 'viewpresetcombo',
					width: '10%',
					ref: 'presetCombo',
					presets: presets.map(p => p.id),
					picker: {
						maxHeight: 500
					}
				},
				{
					text: window.Helper.getTranslatedString('Current Week'),
					width: '10%',
					cls: 'b-raised b-blue',
					ref: 'goToThisWeek',
					tooltip: window.Helper.getTranslatedString('CurrentWeekTooltip'),
					onAction: () => self.gotoDate(new Date(), "weekly")
				},
				{
					type: 'textfield',
					ref: 'filterByName',
					//@ts-ignore
					icon: 'b-fa b-fa-filter',
					cls: 'align-v-end',
					placeholder: window.Helper.getTranslatedString('FilterEvents'),
					clearable: true,
					width: '15%',
					keyStrokeChangeDelay: 100,
					triggers: {
						filter: {
							align: 'start',
							cls: 'b-fa b-fa-filter'
						}
					},
					onChange({value}) {
						value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
						const regEx = new RegExp(value, 'i');
						// Replace all previous filters and set a new filter
						self.scheduler.eventStore.filter({
							filters: (event: EventModel) => {
								if (isDispatch(event)) {
									return regEx.exec(event.name) != null ||
										regEx.exec(event.ServiceOrder?.Company) != null ||
										regEx.exec(event.ServiceOrder?.ErrorMessage) != null ||
										regEx.exec(event?.ServiceOrder?.InstallationName) != null ||
										regEx.exec(event?.ServiceOrder?.InstallationDescription) != null ||
										regEx.exec(event?.ServiceOrderTimeDispatches?.toString()) != null ||
										regEx.exec(event?.ServiceOrder?.Status?.toString()) != null;
								} else if (isAbsence(event)) {
									return regEx.exec(event.name) != null ||
										regEx.exec(event.AbsenceType) != null;
								}
								return true;
							},
							replace: true
						});

						self.scheduler.resourceStore.filter({
							filters: (resource: ResourceModel) => {
								return value
									// @ts-ignore
									? self.scheduler.assignmentStore.records.filter(r => self.scheduler.eventStore.records.map(r2 => r2.id).includes(r.eventId)).map(a => a.resourceId).includes(resource.id)
									: self.scheduler.resourceStore.clearFilters()
							},
							replace: true
						});
					}
				}
			],
			//@ts-ignore
			timeAxis: {
				continuous: false,
				generateTicks(axisStartDate, axisEndDate, unit = this.unit, increment = this.increment) {
					if (self?.scheduler?.isDestroyed) {
						return [];
					}

					const {nonWorkingHours} = window.Helper.Scheduler.WorkingHours.getProfileHours(self.parentViewModel.profile());

					if (unit === 'hour') {
						//compress nonworking hours only for hour unit
						let ticks: { startDate: Date, endDate: Date }[] = [];

						let days = DateHelper.diff(axisStartDate, axisEndDate, 'days');
						for (let day = 0; day < days; day++) {
							let startOfDay = DateHelper.add(axisStartDate, day, 'days');

							let dayHourSpans: HourSpan[] = [];
							for (let h = 0; h < 24; h += increment) {
								dayHourSpans.push({from: h, to: h + increment});
							}

							if (increment == 1) {
								//merge
								let i = 0;
								while (i < dayHourSpans.length - 1) {
									//check merging possibility
									let currentHourSpan = dayHourSpans[i];
									let isCurrentCompletlyNonWorking = nonWorkingHours.some(x => currentHourSpan.from >= x.from && currentHourSpan.to <= x.to);

									let nexthourSpan = dayHourSpans[i + 1];
									let isNextCompletlyNonWorking = nonWorkingHours.some(x => nexthourSpan.from >= x.from && nexthourSpan.to <= x.to);

									if (isCurrentCompletlyNonWorking && isNextCompletlyNonWorking) {
										//merge the two
										currentHourSpan.to = nexthourSpan.to;
										dayHourSpans.splice(i + 1, 1);
									} else {
										++i;
									}
								}
							}

							let dayTicks = dayHourSpans.map(x => ({
								startDate: DateHelper.add(startOfDay, x.from, 'hours'),
								endDate: DateHelper.add(startOfDay, x.to, 'hours')
							}));

							ticks.push(...dayTicks);
						}

						return ticks;
					} else {
						//copy paste of default implementation
						const me = this, ticks = [], usesExclusion = Boolean(me.include);
						let intervalEnd, tickEnd, isExcluded, dstDiff = 0, {
							startDate,
							endDate
						} = me.getAdjustedDates(axisStartDate, axisEndDate);
						me.tickCache = {};
						if (usesExclusion) {
							me.initExclusion();
						}
						while (startDate < endDate) {
							intervalEnd = DateHelper.getNext(startDate, unit, increment, me.weekStartDay);
							if (!me.autoAdjust && intervalEnd > endDate) {
								intervalEnd = endDate;
							}
							if (unit === "hour" && increment > 1 && ticks.length > 0 && dstDiff === 0) {
								const prev = ticks[ticks.length - 1];
								dstDiff = (prev.startDate.getHours() + increment) % 24 - prev.endDate.getHours();
								if (dstDiff !== 0) {
									intervalEnd = DateHelper.add(intervalEnd, dstDiff, "hour");
								}
							}
							isExcluded = false;
							if (usesExclusion) {
								tickEnd = new Date(intervalEnd.getTime());
								isExcluded = me.processExclusion(startDate, intervalEnd, unit);
							} else {
								tickEnd = intervalEnd;
							}
							if (!isExcluded) {
								ticks.push({
									id: ticks.length + 1,
									startDate,
									endDate: intervalEnd
								});
								me.tickCache[startDate.getTime()] = ticks.length - 1;
							}
							startDate = tickEnd;
						}

						this.owner.timeAxisColumn.refreshHeader();

						return ticks;
					}
				}
			}
		});
		self.scheduler.defaultEventDuration = moment.duration(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchDefaultDuration, "minutes");
		self.scheduler.serviceOrderDispatchMaximumDuration = moment.duration(self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration, "minutes");
		self.scheduler.ServiceOrderDispatchIgnoreCalculatedDuration = self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchIgnoreCalculatedDuration;
		self.scheduler.AllowSchedulingForPast = () => self.parentViewModel.profile().ClientConfig.AllowSchedulingForPast;
		self.scheduler.dataForFirstRow = () => self.parentViewModel.profile().ClientConfig.DataForFirstRow;
		self.scheduler.dataForSecondRow = () => self.parentViewModel.profile().ClientConfig.DataForSecondRow;
		self.scheduler.dataForThirdRow = () => self.parentViewModel.profile().ClientConfig.DataForThirdRow;
		self.scheduler.rowHeight = self.scheduler.baseRowHeight + self.scheduler.numberOfRows() * self.scheduler.singleRowHeight;


		const throttleAndDebounceMouseMovement = (throttleLimit, debounceDelay) => {
			let inThrottle: boolean, debounceTimeout: NodeJS.Timeout, intervalId: NodeJS.Timeout;

			const throttleFunc = (event) => {
				if (!inThrottle) {
					inThrottle = true;
					if (event.target.id === "leftOverlayDiv") {
						self.gotoPreviousDate();
					} else {
						self.gotoNextDate();
					}
					self.scheduler.highlightResourceCalendars(self.scheduler.getResourcesToHighlight(self.scheduler.eventStore.records.filter(e => self.draggedEventData.eventIds.includes(e.id)) as SchedulerEventModel[]));
					setTimeout(() => {
						inThrottle = false
					}, throttleLimit);
				}
			};

			return {
				start(event) {
					clearTimeout(debounceTimeout);
					if (self.draggedEventData === null) return;

					if (intervalId) return;
					debounceTimeout = setTimeout(() => {
						intervalId = setInterval(() => {
							throttleFunc(event);
						}, throttleLimit);
					}, debounceDelay);
				},
				stop() {
					clearInterval(intervalId);
					clearTimeout(debounceTimeout);
					intervalId = null;
					debounceTimeout = null;
					inThrottle = false;
				}
			}
		};
		const control = throttleAndDebounceMouseMovement(900, 100);
		$('#rightOverlayDiv')[0].addEventListener("mousemove", control.start.bind(control));
		$('#leftOverlayDiv')[0].addEventListener("mousemove", control.start.bind(control));

		$('#rightOverlayDiv')[0].addEventListener("mouseleave", control.stop.bind(control));
		$('#leftOverlayDiv')[0].addEventListener("mouseleave", control.stop.bind(control));

		self.defaultTimeAxisFilter = (t: TimeAxis): boolean => {
			if (self.scheduler.isDestroyed) {
				return false;
			}

			const calendar = self.scheduler.project.calendar as CalendarModel;
			const presetId = self.scheduler.viewPreset["id"] as string;

			if (presetId != 'daily') {
				return calendar.isWorkingTime(t.startDate, t.endDate);
			}

			return true;
		}

		const getShiftDateFunction = function (forward: boolean) {
			const s = forward ? 1 : -1;

			return function () {
				const presetId = self.scheduler.viewPreset["id"] as string;
				let value = this.value;
				switch (presetId) {
					case 'quarterly':
						value = DateHelper.add(this.value, s * 1, 'quarter');
						break;
					case 'monthly':
						value = DateHelper.add(this.value, s * 1, 'month');
						break;
					case 'weekly':
						value = DateHelper.add(this.value, s * 7, 'day');
						break;
					case 'daily': {
						let endValue: Date;
						let isWorking: boolean;
						do {
							value = DateHelper.add(value, s * 1, 'day');
							endValue = DateHelper.add(value, 1, 'day');
						} while (!(self.scheduler.project.calendar as CalendarModel).isWorkingTime(value, endValue));
						break;
					}
					default:
						throw new Error("Unknown preset id.");
				}
				self.scheduler.saveState();
				return value;
			};
		}

		Object.defineProperties(self.scheduler.widgetMap.datePicker, {
			backShiftDate: {
				get: getShiftDateFunction(false)
			},
			forwardShiftDate: {
				get: getShiftDateFunction(true)
			}
		});

		this.dateRangeChangeEventsConsumer = new Consumer(
			async (e: DateRangeChangeEvent) => {
				e.processStarted = true;
				if (e.withdrawn) {
					return;
				}
				this.parentViewModel.loading(true);

				const technicians = self.scheduler.resourceStore.allRecords.filter(isTechnician);
				const tools = self.scheduler.resourceStore.allRecords.filter(isArticle);

				let dispatches = await self.parentViewModel.loadTechniciansDispatches(technicians, e.startDate, e.endDate);
				let dispatchesIds = new Set(dispatches.map(d => d.id));
				let absences = await self.parentViewModel.loadAbsences(technicians, tools, e.startDate, e.endDate);
				let absencesIds = new Set(absences.map(a => a.id));
				//This list is for when autosave is off and an event is unassigned. Unassigning wont mark the dispatch as modified. This list is used to not remove such dispatches from timeline while changing range.
				let removedAssignmentsDispatchIds = (self.scheduler.assignmentStore?.["removed"]?.["values"]?.map(x => x.eventId) ?? []) as (string | number)[];

				//Find Items to be removed
				let eventsToBeRemoved = e.removeAllEvents ? self.scheduler.eventStore.allRecords : self.scheduler.eventStore.allRecords.filter(e =>
					(!removedAssignmentsDispatchIds.includes(e.id)) && !self.draggedEventData?.eventIds.includes(e.id) &&
					(e["OriginalData"]?.["entityState"] === $data.EntityState.Unchanged || e["OriginalData"]?.["entityState"] === undefined) &&
					(((isDispatch(e as EventModel) || isServiceOrderTimePosting(e as EventModel)) && !dispatchesIds.has(e.id)) ||
						(isAbsence(e as EventModel) && !absencesIds.has(e.id)))
				);
				let eventsToBeRemovedIds = new Set(eventsToBeRemoved.map(e => e.id));
				let assignmentsToBeRemoved = (self.scheduler.assignmentStore.allRecords as AssignmentModel[]).filter(a => eventsToBeRemovedIds.has(a.eventId));

				//Prevent DB deletes
				eventsToBeRemoved.forEach(e => self.detachFromStore.add(e));
				assignmentsToBeRemoved.forEach(e => self.detachFromStore.add(e));

				//Remove old items
				self.scheduler.eventStore.remove(eventsToBeRemoved, true);
				self.scheduler.assignmentStore.remove(assignmentsToBeRemoved, true);

				//Find Items to be added
				let eventStoreItemIds = new Set(self.scheduler.eventStore.allRecords.map(e => e.id));
				let eventsToBeAdded = [
					...dispatches.filter(d => !eventStoreItemIds.has(d.id)),
					...absences.filter(d => !eventStoreItemIds.has(d.id))
				];
				let assignmentsToBeAdded = await self.parentViewModel.loadAssignments(eventsToBeAdded);

				//Prevent DB inserts
				assignmentsToBeAdded.forEach(e => self.attachToStore.add(e));
				eventsToBeAdded.forEach(e => {
					self.attachToStore.add(e);
					(e.children as EventModel[])?.forEach(t => self.attachToStore.add(t));
				});


				//Add Items
				await self.scheduler.eventStore.addAsync(eventsToBeAdded);
				await self.scheduler.assignmentStore.addAsync(assignmentsToBeAdded);

				//Clear possible changes to newly added items
				eventsToBeAdded.forEach(x => x.clearChanges(true));
				assignmentsToBeAdded.forEach(x => x.clearChanges(true));

				//This workaround is to prevent the dirtyflag (dashed lines) to be on the events
				//@ts-ignore
				self.scheduler.refresh();

				e.callback?.(e);

				//recalculate routes based on current events
				await self.getRoute(self.routeConfig(), self.scheduler.timeAxis.startDate, self.scheduler.timeAxis.endDate, true);

				this.parentViewModel.loading(false);
			}
		);

		self.scheduler.on('dateRangeChange', async e => {
			let daysBefore: number, daysAfter: number;
			let presetId = self.scheduler.viewPreset["id"] as string;

			if (presetId == 'monthly') {
				daysBefore = daysAfter = 31;
			} else if (presetId == 'weekly') {
				daysBefore = daysAfter = 7;
			} else if (presetId == 'daily') {
				daysBefore = daysAfter = 3;
			} else if (presetId == 'quarterly') {
				daysBefore = daysAfter = 92;
			} else
				throw new Error("Unknown preset id.");

			let startDate = DateHelper.add(e.new.startDate, -daysBefore, 'day');
			let endDate = DateHelper.add(e.new.endDate, daysAfter, 'day');

			this.dateRangeChangeEventsConsumer.queue.forEach(e => {
				if (!e.processStarted) {
					e.withdrawn = true;
				}
			});
			this.dateRangeChangeEventsConsumer.enqueue({
				startDate: startDate,
				endDate: endDate,
				callback: () => this.triggerPendingChanges(),
				processStarted: false,
				withdrawn: false
			});
			this.dateRangeChangeEventsConsumer.notify();
		});

		self.crudManager = new CrmCrudManager({
			//@ts-ignore
			project: self.scheduler.project,
			context: self
		}, self.attachToStore, self.detachFromStore, self.parentViewModel.syncData.bind(self.parentViewModel));
		if (!self.parentViewModel.autosave()) {
			self.crudManager.suspendAutoSync();
		}
		self.crudManager.on("save", () => {
			//The dirty class is not removed automatically after saves. This workaround removes it �
			self.scheduler.element.querySelectorAll(".b-sch-dirty.b-sch-event").forEach(e => e.classList.remove("b-sch-dirty"));

			self.triggerPendingChanges();
		});

		self.scheduler.project.assignmentStore.on("update", async (event: {
			source: CrmAssignmentStore,
			record: Assignment,
			changes: Object
		}) => {
			let assignment: any = window.database.attachOrGet(event.record.OriginalData);
			try {
				if (event.changes['resourceId']) {
					const eventRecord = event.record.event as EventModel;
					if (isDispatch(eventRecord)) {
						let dispatch: any = this.parentViewModel.syncWasOff() ? eventRecord.OriginalData : window.database.attachOrGet(eventRecord.OriginalData);
						if (!eventRecord.isTeamDispatch() || eventRecord.isTeamDispatch() && event.changes['resourceId'].oldValue == dispatch.Username) {
							dispatch.Username = event.changes['resourceId'].value;
							await eventRecord.setAsync("Username", dispatch.Username);
							eventRecord.OriginalData = dispatch;
						}
						assignment.ResourceKey = event.changes['resourceId'].value;
					} else if (isAbsence(eventRecord)) {
						if (assignment instanceof Sms.Scheduler.Rest.Model.SmsScheduler_DispatchPersonAssignment) {
							let absence: any = this.parentViewModel.syncWasOff() ? eventRecord.OriginalData : window.database.attachOrGet(eventRecord.OriginalData);
							absence.ResponsibleUser = event.changes['resourceId'].value;
							await eventRecord.setAsync("ResponsibleUser", absence.ResponsibleUser);
							assignment.ResourceKey = absence.ResponsibleUser;
							eventRecord.OriginalData = absence;
						} else if (assignment instanceof Sms.Scheduler.Rest.Model.SmsScheduler_DispatchArticleAssignment) {
							let absence: any = this.parentViewModel.syncWasOff() ? eventRecord.OriginalData : window.database.attachOrGet(eventRecord.OriginalData);
							absence.ArticleKey = event.changes['resourceId'].value;
							await eventRecord.setAsync("ArticleKey", absence.ArticleKey);
							assignment.ResourceKey = absence.ArticleKey;
							eventRecord.OriginalData = absence;
						}
					}
				}
			} catch (e) {
				window.Log.error(e);
			}
		});
		self.scheduler.project.eventStore.on("update", async (event: {
			source: DispatchStore,
			record: EventModel,
			changes: Object
		}) => {
			if (!isDispatch(event.record) && !isAbsence(event.record)) return;
			//Attach on demand
			const attachOrGet = (() => {
				let _entity: $data.Entity = null;
				return <T extends $data.Entity>() => (_entity ?? (_entity = window.database.attachOrGet((event.record as Dispatch | Absence).OriginalData))) as T;
			})();


			try {
				if (event.record.readonly && isDispatch(event.record) && event.record.Status.toString() != 'Released' && event.changes["StatusKey"] == null) {
					return;
				}
				if (isDispatch(event.record)) {
					if(!window.Helper.ServiceOrder.dispatchesCanBeAdded(ko.observable(event.record.ServiceOrder.OriginalData.asKoObservable()), self.parentViewModel.lookups.serviceOrderStatuses) || !window.Helper.ServiceOrder.isServiceOrderEditable(ko.observable(event.record.ServiceOrder.OriginalData.asKoObservable()), ko.observable(window.Helper.User.currentUser), self.parentViewModel.lookups.serviceOrderStatuses)) {
						const popupItems = [{
							type: 'label',
							cls: 'scheduler-warning',
							text: window.Helper.String.getTranslatedString("RuleViolation.ServiceOrderMustBeInPlannableState"),
							style: 'width: 100%',
						}];
						window.Helper.Scheduler.ShowPopup(popupItems);
						event.source.revertChanges();
						return;
					}
					if (event.changes['StatusKey']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.StatusKey = event.changes['StatusKey'].value;
					}

					if (event.changes['SchedulingGroupId'] && event.record?.OriginalData?.ExtensionValues) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.ExtensionValues.SchedulingGroupId = event.changes['SchedulingGroupId'].value;
					}

					if (event.changes['TeamId'] && event.record?.OriginalData?.ExtensionValues) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.ExtensionValues.TeamId = event.changes['TeamId'].value;
					}

					if (event.changes['IsFixed']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.IsFixed = event.changes['IsFixed'].value;
					}

					if (event.changes['startDate']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.Date = event.changes['startDate'].value;
					}

					if (event.changes['endDate']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.EndDate = event.changes['endDate'].value;
					}

					if (event.changes['duration']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.NetWorkMinutes = DateHelper.as('minute', event.changes['duration'].value, (event.record).durationUnit);
					}

					if (event.changes['ServiceOrderTimeDispatches']) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.ServiceOrderTimeDispatches = [...event.changes['ServiceOrderTimeDispatches'].value];
					}

					if (event.changes["Username"]) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.Username = event.changes['Username'].value;
					}

					if (event.changes["manuallyScheduled"]) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.NetWorkMinutes = DateHelper.as('minute', event.record.duration, (event.record).durationUnit);
					}

					if (event.changes["Remark"]) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.Remark = event.changes['Remark'].value;
					}

					if (event.changes["InfoForTechnician"]) {
						const dispatch = attachOrGet<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>();
						dispatch.InfoForTechnician = event.changes['InfoForTechnician'].value;
					}

				} else if (isAbsence(event.record)) {
					if (event.changes['startDate']) {
						const absence = attachOrGet<Sms.Scheduler.Rest.Model.SmsScheduler_Absence>();
						absence.From = event.changes['startDate'].value ?? absence.From;
					}

					if (event.changes['endDate']) {
						const absence = attachOrGet<Sms.Scheduler.Rest.Model.SmsScheduler_Absence>();
						absence.To = event.changes['endDate'].value ?? absence.To;
					}

					if (event.changes['Description']) {
						const absence = attachOrGet<Sms.Scheduler.Rest.Model.SmsScheduler_Absence>();
						absence.Description = event.changes['Description'].value;
					}
					if (event.changes['TimeEntryTypeKey'] || event.changes['DowntimeReasonKey']) {
						if (event.record.OriginalData instanceof Sms.Scheduler.Rest.Model.SmsScheduler_Absence) {
							const absence = attachOrGet<Sms.Scheduler.Rest.Model.SmsScheduler_Absence>();
							absence.TimeEntryTypeKey = event.changes['TimeEntryTypeKey'].value;
							event.record.AbsenceTypeData = await window.database.CrmPerDiem_TimeEntryType.filter("it => it.Key == this.key && it.Language == this.language", {
								key: event.changes['TimeEntryTypeKey'].value,
								language: self.parentViewModel.currentUserLocale
							}).first();
						} else if (event.record.OriginalData instanceof Crm.Article.Rest.Model.CrmArticle_ArticleDowntime) {
							const absence = attachOrGet<Crm.Article.Rest.Model.CrmArticle_ArticleDowntime>();
							absence.DowntimeReasonKey = event.changes['DowntimeReasonKey'].value;
							event.record.AbsenceTypeData = await window.database.CrmArticle_ArticleDowntimeReason.filter("it => it.Key == this.key && it.Language == this.language", {
								key: event.changes['DowntimeReasonKey'].value,
								language: self.parentViewModel.currentUserLocale
							}).first();
						}
					}
				}
				self.scheduler.refreshRows();
			} catch (e) {
				window.Log.error(e);
			}
		});

		self.scheduler.project.resourceStore.on("beforeRemove", async (event) => {
			try {
				if (event.isCollapse) {
					return;
				}
				const resourceIdsToBeRemoved = event.records.map(t => t.id) as string[];
				const assignmentsToBeRemoved = (self.scheduler.assignmentStore.allRecords as AssignmentModel[]).filter(a => resourceIdsToBeRemoved.includes(a.resourceId as string));
				//Prevent DB deletes
				assignmentsToBeRemoved.forEach(e => self.detachFromStore.add(e));

				const profileResources = await window.database.SmsScheduler_ProfileResource.filter("it.ProfileKey == profileId && it.ResourceKey in resourceIdsToBeRemoved", {
					profileId: this.parentViewModel.profile().Id,
					resourceIdsToBeRemoved
				}).toArray();
				profileResources.forEach(profileResource => window.database.remove(profileResource));

				this.parentViewModel.profile().ResourceKeys.splice(this.parentViewModel.profile().ResourceKeys.indexOf(event.records[0].id), 1);
			} catch (e) {
				window.Log.error(e);
			}
		});

		self.scheduler.on('renderEvent', async ({ eventRecord, element }: { eventRecord: Model, element: HTMLDivElement }) => {
			if(!isDispatch(eventRecord)) return;
			element.querySelector('.sch-first-row').textContent = await eventRecord.getRowData(self.parentViewModel.profile().ClientConfig.DataForFirstRow);
			const secondRow = element.querySelector('.sch-second-row');
			if(secondRow) {
				secondRow.textContent = await eventRecord.getRowData(self.parentViewModel.profile().ClientConfig.DataForSecondRow);
			}
			const thirdRow = element.querySelector('.sch-third-row');
			if(thirdRow) {
				thirdRow.textContent = await eventRecord.getRowData(self.parentViewModel.profile().ClientConfig.DataForThirdRow);
			}
		});

		//@ts-ignore
		self.scheduler.project.commit();
		self.parentViewModel.scheduler(self.scheduler);

		self.crudManager.on('hasChanges', () => self.triggerPendingChanges());
		self.crudManager.on('noChanges', () => self.triggerPendingChanges());
		ko.computed(() => self.pendingChangesTrigger() && self.updatePendingChanges());

		self.scheduler.eventStore.on('change', self.onEventStoreChange, self);
		self.scheduler.resourceStore.on('change', self.onResourceStoreChange, self);
		self.scheduler.timeAxis.on('reconfigure', self.onTimeAxisReconfigure, self);
		//If data loaded before the map, trigger onStoreChange manually
		if (self.scheduler.eventStore.count) {
			self.onEventStoreChange({action: 'dataset', records: self.scheduler.eventStore.records});
		}
		if (self.scheduler.resourceStore.count) {
			self.onResourceStoreChange({action: 'dataset', records: self.scheduler.resourceStore.records});
		}

		this.pipelineSplitter = new Splitter({
			appendTo: 'scheduler',
			maxWidth: 6.5,
		});

		self.pipeline = new Pipeline({
			label: window.Helper.getTranslatedString('Name'),
			appendTo: "scheduler",
			rowHeight: self.parentViewModel.profile().ClientConfig.PipelineSecondLine != null ? 45 : 30,
			store: {
				tree: true,
				data: data.inlineData.serviceOrders
			},
			stateId: 'pipelinePanel',
			collapsed: true,
			features: {
				cellEdit: false,
				rowCopyPaste: false,
				regionResize: true,
				tree: true,
				cellMenu: {
					items: {
						removeRow: false
					},
					processItems({items, column, record}) {
						if (!isServiceOrder(record)) {
							items.OpenOrderButton = false;
							items.openDispatches = false;
						}
					}
				},
				cellTooltip: {
					allowOver: true,
					scrollable: true,
					width: '28em',
					tooltipRenderer: (data) => {
						if (isServiceOrder(data.record)) {
							let profile = ko.unwrap(this.parentViewModel.profile);
							let tooltipProps = ko.unwrap(profile.ClientConfig.ServiceOrderTooltip);
							if (tooltipProps.length === 0) {
								return window.Helper.getTranslatedString('TooltipIsNotConfigured');
							}
							let tooltipContext = { "ServiceOrder": data.record };

							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps);
						} else if (isJob(data.record)) {
							let profile = ko.unwrap(this.parentViewModel.profile);
							let tooltipProps = ko.unwrap(profile.ClientConfig.ServiceOrderTimeTooltip);
							if (tooltipProps.length === 0) {
								return window.Helper.getTranslatedString('TooltipIsNotConfigured');
							}
							let tooltipContext = { "ServiceOrderTime": data.record.OriginalData, "ServiceOrder": data.record.serviceOrder };

							return window.Helper.Scheduler.Tooltips.BuildTooltip(tooltipContext, tooltipProps, "ServiceOrder");
						} else {
							return ``
						}
					},
					hideOnDelegateChange: true,
					hoverDelay: DateHelper.as('ms', self.parentViewModel.profile().ClientConfig.TooltipDisplayDelay ?? 0, 's'),
					hideDelay: 0
				}
			},
			onSelectionChange(event) {
				const
					{selection} = event,
					{calendarHighlight} = self.scheduler.features;
				let serviceOrders = selection.filter(isServiceOrder);
				let jobs = selection.filter(isJob);
				self.pipelineSelectedServiceOrders(serviceOrders);
				let absenceOrders = selection.filter(isAbsenceOrder);
				let articleDowntimes = selection.filter(isArticleDowntime);
				if (jobs.length == 0 && serviceOrders.length == 0 && absenceOrders.length == 0 && articleDowntimes.length == 0) {
					calendarHighlight.unhighlightCalendars();
					return;
				}
			let resourcesToHighlight: Model[] = [];
				if(jobs.length > 0) {
					if (jobs[0].serviceOrder.OriginalData.RequiredSkillKeys?.length == 0 && jobs[0].serviceOrder.OriginalData.RequiredAssetKeys?.length == 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord));
					} else if (jobs[0].serviceOrder.OriginalData.RequiredAssetKeys?.length == 0 && jobs[0].serviceOrder.OriginalData.RequiredSkillKeys?.length > 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, jobs[0].serviceOrder.OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					} else if (jobs[0].serviceOrder.OriginalData.RequiredSkillKeys?.length == 0 && jobs[0].serviceOrder.OriginalData.RequiredAssetKeys?.length > 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, jobs[0].serviceOrder.OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					} else {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, jobs[0].serviceOrder.OriginalData, self.scheduler.startDate, self.scheduler.endDate) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, jobs[0].serviceOrder.OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					}
				}
				if (serviceOrders.length > 0) {
					if (serviceOrders[0].OriginalData.RequiredSkillKeys?.length == 0 && serviceOrders[0].OriginalData.RequiredAssetKeys?.length == 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord));
					} else if (serviceOrders[0].OriginalData.RequiredAssetKeys?.length == 0 && serviceOrders[0].OriginalData.RequiredSkillKeys?.length > 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, serviceOrders[0].OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					} else if (serviceOrders[0].OriginalData.RequiredSkillKeys?.length == 0 && serviceOrders[0].OriginalData.RequiredAssetKeys?.length > 0) {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, serviceOrders[0].OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					} else {
						resourcesToHighlight = self.scheduler.resourceStore.query((resourceRecord: ResourceModel): resourceRecord is Technician => isTechnician(resourceRecord) && window.Helper.Scheduler.hasAssetsForOrder(resourceRecord, serviceOrders[0].OriginalData, self.scheduler.startDate, self.scheduler.endDate) && window.Helper.Scheduler.hasSkillsForOrder(resourceRecord, serviceOrders[0].OriginalData, self.scheduler.startDate, self.scheduler.endDate));
					}
				}
				if (absenceOrders.length > 0) {
					resourcesToHighlight = self.scheduler.resourceStore.query(isTechnician);
				}
				if (articleDowntimes.length > 0) {
					resourcesToHighlight = self.scheduler.resourceStore.query(isArticle);
				}
				
				// Include generated parent nodes that contain highlighted resources
				// This ensures the blue line indicator appears on group headers when dragging from pipeline
				if (resourcesToHighlight.length > 0) {
					const highlightedIds = new Set(resourcesToHighlight.map(r => r.id));
					self.scheduler.resourceStore.forEach((resource: ResourceModel) => {
						if (window.Sms.Scheduler.Timeline.isGeneratedParentNode(resource as SchedulerResourceModel)) {
							const hasHighlightedChild = (parent: ResourceModel): boolean => {
								const children = parent.children;
								if (!Array.isArray(children) || children.length === 0) return false;
								for (const child of children as ResourceModel[]) {
									if (highlightedIds.has(child.id)) return true;
									const childChildren = child.children;
									if (Array.isArray(childChildren) && childChildren.length > 0 && hasHighlightedChild(child)) return true;
								}
								return false;
							};
							if (hasHighlightedChild(resource)) {
								resourcesToHighlight.push(resource);
							}
						}
					});
				}
				
				if (!calendarHighlight.disabled && resourcesToHighlight.length > 0) {
					calendarHighlight.highlightResourceCalendars(resourcesToHighlight as ResourceModel[]);
				} else {
					calendarHighlight.unhighlightCalendars();
				}
			},
			tbar: {
				overflow: null,
				items: [{
					type: 'textfield',
					ref: 'filterByName',
					//@ts-ignore
					icon: 'b-fa b-fa-filter',
					placeholder: window.Helper.getTranslatedString('FilterPipeline'),
					cls: 'align-v-end',
					clearable: true,
					width: '15em',
					keyStrokeChangeDelay: 100,
					triggers: {
						filter: {
							align: 'start',
							cls: 'b-fa b-fa-filter'
						}
					},
					onChange({value}) {
						if (self.pipeline?.selectedRecords?.length > 0) {
							self.pipeline.deselectAll(false, false);
						}

						value = value.replace(/[.*+?^${}()|[\]\\]/g, '\\$&');
						const regEx = new RegExp(value, 'i');

						const serviceOrderMatch = (serviceOrder: ServiceOrder) => regEx.exec(serviceOrder.name) != null ||
							regEx.exec(serviceOrder?.Company) != null ||
							regEx.exec(serviceOrder?.ZipCode) != null ||
							regEx.exec(serviceOrder?.ErrorMessage) != null ||
							regEx.exec(serviceOrder?.InstallationName) != null ||
							regEx.exec(serviceOrder?.InstallationDescription) != null ||
							regEx.exec(serviceOrder?.Status?.toString()) != null;

						// Replace all previous filters and set a new filter
						//@ts-ignore
						self.pipeline.store.filter({
							filters: (row: GridRowModel) => {
								if (isServiceOrder(row)) {
									return serviceOrderMatch(row);
								} else if (isAbsenceOrder(row) || isArticleDowntime(row)) {
									return regEx.exec(row.name) != null ||
										regEx.exec(row.AbsenceType) != null;
								} else if (isJob(row)) {
									return serviceOrderMatch(row.serviceOrder) || regEx.exec(row.name) != null;
								} else if (row["name"]) {
									return regEx.exec(row["name"]) != null;
								} else {
									//Unknown row
									return false;
								}
							},
							replace: true
						});
					}
					},
					'->',
					{
						icon: 'b-fa b-fa-angle-double-down',
						cls: 'b-transparent',
						id: 'expand-collapse-button',
						onClick: ({source}) => {
							let pipeline = self.pipeline;
							if (pipeline["collapsed"]) {
								pipeline.expandAll();
								pipeline["collapsed"] = false;
								source.icon = 'b-fa b-fa-angle-double-up';
							} else {
								pipeline.collapseAll();
								pipeline["collapsed"] = true;
								source.icon = 'b-fa b-fa-angle-double-down';
							}
						}
					}
				]
			},
			listeners: {
				toggleNode() {
					let f = (n) => `${!n.parent.isRoot ? f(n.parent) : ''}\\${n.name}`;
					//@ts-ignore
					const expandedNodes = self.pipeline.store.allRecords.filter(x => x.isExpanded()).map(f);
					window.Helper.Database.saveToLocalStorage(`${window.Helper.User.getCurrentUserName()}_expandedNodes`, JSON.stringify(expandedNodes))
				},
				paint({firstPaint}) {
					if (firstPaint) {
						setTimeout(async () => {
							if (self.parentViewModel.orderId) {
								//@ts-ignore
								if (self.pipeline.collapsed) {
									await self.pipeline.expandAll();
								}
								await self.pipeline.scrollRowIntoView(self.parentViewModel.orderId, {
									animate: true,
									highlight: true
								});
							}
						}, 500);
					}
				}
			}
		});

		self.pipeline.on("dataChange", () => {
			self.pipeline.renderRows();
		});

		self.pipeline.pipelineFirstLineData = () => self.parentViewModel.profile().ClientConfig.PipelineFirstLine;
		self.pipeline.pipelineSecondLineData = () => self.parentViewModel.profile().ClientConfig.PipelineSecondLine;

		const FILTERS_STORAGE_KEY = 'b-example-gantt-fieldfilters-filters';
		const parseDate = dateString => dateString ? new Date(dateString) : dateString;
		//@ts-ignore
		const savedFiltersJSON = BrowserHelper.getLocalStorageItem(FILTERS_STORAGE_KEY);
		let savedFilters;
		if (savedFiltersJSON) {
			try {
				savedFilters = JSON.parse(savedFiltersJSON, function (key, value) {

					if (key === 'value' && this.type === 'date') {
						return (Array.isArray(value) ? value.map(parseDate) : parseDate(value));
					} else if (key === 'value' && this.type === 'duration') {
						return (Array.isArray(value)
							//@ts-ignore
							? value.map(durationStr => new Duration(durationStr))
							//@ts-ignore
							: new Duration(value));
					}
					return value;
				});
			} catch (e) {
				window.Log.error(`Couldn't parse saved filters:\n\n${savedFilters}\n\nError was:\n\n${e}`);
			}
		}
		this.setupDragDrops();
		self.parentViewModel.groupPipeline(null, null);

		self.scheduler.on("resize", () => {
			queueMicrotask(() => self.defaultTaskDurationInPixel(self.getDefaultTaskDurationInPixel()));
		});

		let oldWindowWidth = 0;

		this.horizontalSplitter = new Splitter({
			appendTo: 'schedulertimelinepanel',
			orientation: 'horizontal'
		});

		this.scheduler.tickSize = 0;

		//time axis header cell to display the timespan if its hours. this is used for day and week presets.
		this.headerTooltip = new Tooltip({
			forSelector: '.b-lowest>.b-sch-header-timeaxis-cell',
			getHtml: ({tip, element, activeTarget, event}) => {
				if (activeTarget.dataset.tickIndex && self.scheduler.timeAxis) {
					const viewPresetId = self.scheduler.viewPreset["id"] as string;
					const t = self.scheduler.timeAxis.getAt(parseInt(activeTarget.dataset.tickIndex));

					if(viewPresetId === 'quarterly' || viewPresetId === 'monthly') {
						return DateHelper.format(t["startDate"], 'ddd L');
					}

					let startHour = DateHelper.format(t["startDate"], 'HH:mm');
					let endHour = DateHelper.format(t["endDate"], 'HH:mm');

					return `${startHour} - ${endHour}`;
				}
				return null;
			}
		});
	}

	showOverlays() {
		const self = this;
		const rect = self.scheduler.timeAxisSubGrid.element.parentElement.parentElement.getBoundingClientRect();
		const splitterWidth = 6.5;
		const leftOverlay = document.getElementById('leftOverlayDiv');
		const rightOverlay = document.getElementById('rightOverlayDiv');

		leftOverlay.style.width = rightOverlay.style.width = `${self.edgeThreshold}px`;
		leftOverlay.style.height = rightOverlay.style.height = `${rect.height}px`;
		leftOverlay.style.top = rightOverlay.style.top = `${rect.top}px`;
		leftOverlay.style.left = `${self.scheduler.timeAxisSubGrid.element.getBoundingClientRect().left - self.edgeThreshold - splitterWidth}px`;
		rightOverlay.style.left = `${rect.right + splitterWidth}px`;

		$(leftOverlay).show();
		$(rightOverlay).show();
	}

	updateFromProfileHours(profile: Sms.Scheduler.Rest.Model.SmsScheduler_Profile) {
		//@ts-ignore
		this.scheduler.timeAxis.updateGenerateTicks()
	}

	async applyResourceGroup() {
		const profile = ko.unwrap(this.parentViewModel.profile);
		const groupProps = ko.unwrap(profile.ClientConfig.ResourceGroup);

		if (groupProps && groupProps.length > 0) {
			const processedGroups = groupProps.map(g => g.split("."));

			if (processedGroups.some(g => g.length != 2 || g[0] != "Resource")) {
				throw new Error("Specified resource group is not supported.");
			}

			await this.scheduler.features.treeGroup.group(processedGroups.map(g => g[1]));
		} else {
			await this.scheduler.features.treeGroup.clearGroups();
		}
	}

	reloadDateRange() {
		const p = new Promise((fulfilled) => {
			this.dateRangeChangeEventsConsumer.queue.forEach(e => {
				if (!e.processStarted) {
					e.withdrawn = true;
				}
			});
			this.dateRangeChangeEventsConsumer.enqueue({
				startDate: this.scheduler.startDate,
				endDate: this.scheduler.endDate,
				removeAllEvents: true,
				callback: fulfilled,
				processStarted: false,
				withdrawn: false
			});
			this.dateRangeChangeEventsConsumer.notify();
		});

		p.then(() => {
			//This trigger is needed because noChanges events is sometimes called before hasChanges.
			this.triggerPendingChanges();
		});

		return p;
	}

	setupDragDrops() {
		const self = this;
		let container = document.getElementById("scheduler");

		let prevResourceRecord: ResourceModel = null;
		let prevHighlightRowEl: HTMLElement = null;

		// Drag debug overlay (enable via DevTools console):
		// localStorage.setItem('Sms.Scheduler:dragDebug', '1'); location.reload();
		const DRAG_DEBUG_KEY = 'Sms.Scheduler:dragDebug';
		const isDragDebugEnabled = () => localStorage.getItem(DRAG_DEBUG_KEY) === '1';

		const escapeHtml = (s: any) => String(s ?? '')
			.replace(/&/g, '&amp;')
			.replace(/</g, '&lt;')
			.replace(/>/g, '&gt;')
			.replace(/"/g, '&quot;')
			.replace(/'/g, '&#39;');

		const ensureDragDebugOverlay = (): HTMLDivElement | null => {
			if (!isDragDebugEnabled()) return null;
			let el = document.getElementById('sms-drag-debug-overlay') as HTMLDivElement;
			if (el) return el;

			el = document.createElement('div');
			el.id = 'sms-drag-debug-overlay';
			el.style.position = 'fixed';
			el.style.right = '12px';
			el.style.bottom = '12px';
			el.style.zIndex = '999999';
			el.style.maxWidth = '520px';
			el.style.maxHeight = '55vh';
			el.style.overflow = 'auto';
			el.style.pointerEvents = 'none';
			el.style.background = 'rgba(0,0,0,0.78)';
			el.style.color = '#fff';
			el.style.border = '1px solid rgba(255,255,255,0.18)';
			el.style.borderRadius = '10px';
			el.style.boxShadow = '0 12px 40px rgba(0,0,0,0.35)';
			(el.style as any).backdropFilter = 'blur(6px)';
			el.style.padding = '10px 12px';
			el.style.fontFamily = 'ui-monospace, SFMono-Regular, Menlo, Monaco, Consolas, "Liberation Mono", "Courier New", monospace';
			el.style.fontSize = '12px';
			el.style.lineHeight = '1.35';

			document.body.appendChild(el);
			return el;
		};

		const setDragDebugOverlay = (title: string, lines: string[]) => {
			const el = ensureDragDebugOverlay();
			if (!el) return;
			el.innerHTML = `
				<div style="font-weight:700; margin-bottom:6px;">${escapeHtml(title)} <span style="font-weight:500; opacity:0.75;">(dragDebug=1)</span></div>
				<pre style="white-space:pre-wrap; margin:0;">${escapeHtml(lines.join('\n'))}</pre>
			`;
		};
		const resolveResourceRecordFromDom = (targetEl: HTMLElement, offsetX: number, offsetY: number): ResourceModel => {
			if (!targetEl) return null;
			let record = self.scheduler.resolveResourceRecord(targetEl, [offsetX, offsetY]) as ResourceModel;
			if (record) return record;

			// Fallback: group header rows sometimes don't resolve; try data-id on the closest row element
			const rowEl = (targetEl.closest?.('.b-grid-row') ?? targetEl.closest?.('.b-sch-timeaxis-row')) as HTMLElement;
			const rowId = rowEl?.getAttribute?.('data-id') ?? (rowEl as any)?.dataset?.id;
			return (rowId != null ? ((self.scheduler.resourceStore as any).getById?.(rowId) as ResourceModel) : null) ?? null;
		};

		const resolveLeafResources = (root: ResourceModel): ResourceModel[] => {
			if (!root) return [];
			if ((root as any).isLeaf) return [root];

			const leaves: ResourceModel[] = [];

			// Prefer Bryntum traversal API if present
			if (typeof (root as any).traverse === 'function') {
				(root as any).traverse((node: any) => {
					if (node?.isLeaf) leaves.push(node as ResourceModel);
				});
			} else {
				// Recursive children traversal (children can be boolean)
				const collect = (node: any) => {
					const children = node?.children;
					if (Array.isArray(children) && children.length > 0) {
						for (const c of children) collect(c);
						return;
					}
					if (node?.isLeaf) leaves.push(node as ResourceModel);
				};
				collect(root);
			}

			// Ultimate fallback: derive descendants by walking parent pointers in the store
			if (!leaves.length) {
				const ancestorId = (root as any).id;
				const isDescendantOf = (node: any): boolean => {
					let p = node?.parent;
					while (p) {
						if (p === root) return true;
						if (ancestorId != null && p.id === ancestorId) return true;
						p = p.parent;
					}
					return false;
				};
				leaves.push(...(self.scheduler.resourceStore.allRecords.filter(r => (r as any)?.isLeaf && isDescendantOf(r)) as ResourceModel[]));
			}

			// Normalize to store instances when possible
			const leafIds = new Set(leaves.map(r => (r as any)?.id).filter(id => id != null));
			const storeMatches = self.scheduler.resourceStore.allRecords.filter(r => leafIds.has((r as any).id) && (r as any).isLeaf) as ResourceModel[];
			return storeMatches.length ? storeMatches : leaves;
		};

		const resolveHighlightRowEl = (targetEl: HTMLElement, record: ResourceModel): HTMLElement => {
			if (!targetEl) return null;
			let rowEl = (targetEl.closest?.('.b-grid-row') ?? targetEl.closest?.('.b-sch-timeaxis-row')) as HTMLElement;
			if (rowEl) return rowEl;

			const id = (record as any)?.id;
			if (id == null || !container?.querySelector) return null;
			const esc = (window as any).CSS?.escape ? (window as any).CSS.escape(String(id)) : String(id).replace(/"/g, '\\"');
			return container.querySelector(`.b-grid-row[data-id="${esc}"], .b-sch-timeaxis-row[data-id="${esc}"]`) as HTMLElement;
		};

		container.addEventListener("dragover", function (e) {
			e.preventDefault();

			if (Array.from(e.dataTransfer.types).includes('crm/serviceorder')) {
				let isValid = false;

				const isHorizontal = self.scheduler.mode === "horizontal";
				const allowOverlap = self.scheduler["allowOverlap"];
				const startDate = self.scheduler.getDateFromDomEvent(e, "round", false);

				if (startDate) {
					const targetEl = e.target as HTMLElement;
					const resourceRecord = targetEl ? resolveResourceRecordFromDom(targetEl, e.offsetX, e.offsetY) : null;

					const resources = resolveLeafResources(resourceRecord);

					// Fallback: for virtual/team group header rows, ensure we can resolve at least one technician
					if (!resources.length && resourceRecord) {
						const firstTechnician = window.Sms?.Scheduler?.Timeline?.getFirstTechnicianChild?.(resourceRecord as any) as any;
						if (firstTechnician) resources.push(firstTechnician as ResourceModel);
					}

					const resource = resources?.[0] as any;
					let endDateForAvailability = startDate;

					// When overlap is disallowed, validate based on actual availability (previously blocked all drops)
					if (isTechnician(resource)) {
						try {
							const serviceorderId = e.dataTransfer.getData('crm/serviceorder');
							if (serviceorderId) {
								const order = (self.pipeline.store as Store).find((so: GridRowModel) => so.id == serviceorderId) as ServiceOrder;
								if (order) {
									const duration = window.Helper.Scheduler.determineNewEventDuration(
										order,
										self.scheduler.defaultEventDuration,
										self.scheduler.serviceOrderDispatchMaximumDuration,
										self.scheduler.ServiceOrderDispatchIgnoreCalculatedDuration
									);
									endDateForAvailability = moment(startDate).add(duration).toDate();
								}
							}
						} catch {
							// ignore debug-only dataTransfer quirks
						}
					}

					if (isTechnician(resource) /*|| resource.constructor === Tool*/) {
						if (isHorizontal) {
							prevHighlightRowEl?.classList.remove('target-resource');
							const rowEl = resolveHighlightRowEl(targetEl, resourceRecord);
							rowEl?.classList.add('target-resource');
							prevHighlightRowEl = rowEl;
							prevResourceRecord = resourceRecord;
						}
						isValid = Boolean(allowOverlap) || self.scheduler.isDateRangeAvailable(startDate, endDateForAvailability, null, resource);
					}

					if (isDragDebugEnabled()) {
						const rowEl = (targetEl.closest?.('.b-grid-row') ?? targetEl.closest?.('.b-sch-timeaxis-row')) as HTMLElement;
						const rowId = rowEl?.getAttribute?.('data-id') ?? (rowEl as any)?.dataset?.id;
						const ugCtor = (window as any).Main?.Rest?.Model?.Main_Usergroup;
						const isTeamGroup = Boolean(ugCtor && resourceRecord && (resourceRecord as any)["key"] instanceof ugCtor);
						setDragDebugOverlay('dragover', [
							`startDate: ${window.moment(startDate).format('YYYY-MM-DD HH:mm:ss')}`,
							`endDate(availability): ${window.moment(endDateForAvailability).format('YYYY-MM-DD HH:mm:ss')}`,
							`offset: ${e.offsetX}, ${e.offsetY} | mode: ${self.scheduler.mode}`,
							`rowId: ${rowId ?? 'null'}`,
							`resourceRecord: ${resourceRecord ? `${(resourceRecord as any).id} (${(resourceRecord as any).name ?? ''})` : 'null'} | teamGroup: ${isTeamGroup}`,
							`leafResources: ${resources.length} | first: ${resources[0] ? `${(resources[0] as any).id} (${(resources[0] as any).name ?? ''})` : 'null'}`,
							`allowOverlap: ${Boolean(allowOverlap)} | isValid: ${isValid}`,
						]);
					}
				} else if (isDragDebugEnabled()) {
					setDragDebugOverlay('dragover', [
						'startDate: null (not over time axis)',
						`offset: ${e.offsetX}, ${e.offsetY} | mode: ${self.scheduler.mode}`,
					]);
				}
				if (!isValid) {
					e.dataTransfer.dropEffect = "none";
					prevHighlightRowEl?.classList.remove('target-resource');
					prevHighlightRowEl = null;
					prevResourceRecord = null;
				}
			} else {
				e.dataTransfer.dropEffect = "none";
			}
		});


		container.addEventListener("drop", async function (e) {
			e.preventDefault();

			// Clear any highlight on drop
			prevHighlightRowEl?.classList.remove('target-resource');
			prevHighlightRowEl = null;

			let serviceorderId = e.dataTransfer.getData('crm/serviceorder');
			if (serviceorderId) {
				const resourceRecord = prevResourceRecord;
				prevResourceRecord = null;

				const resources = resolveLeafResources(resourceRecord);

				// Fallback: for virtual/team group header rows, ensure we can resolve at least one technician
				if (!resources.length && resourceRecord) {
					const firstTechnician = window.Sms?.Scheduler?.Timeline?.getFirstTechnicianChild?.(resourceRecord as any) as any;
					if (firstTechnician) resources.push(firstTechnician as ResourceModel);
				}

				const resource = (resources?.[0] ?? null) as Technician;

				if (isDragDebugEnabled()) {
					const ugCtor = (window as any).Main?.Rest?.Model?.Main_Usergroup;
					const isTeamGroup = Boolean(ugCtor && resourceRecord && (resourceRecord as any)["key"] instanceof ugCtor);
					setDragDebugOverlay('drop', [
						`serviceorderId: ${serviceorderId}`,
						`resourceRecord: ${resourceRecord ? `${(resourceRecord as any).id} (${(resourceRecord as any).name ?? ''})` : 'null'} | teamGroup: ${isTeamGroup}`,
						`leafResources: ${resources.length} | first: ${resources[0] ? `${(resources[0] as any).id} (${(resources[0] as any).name ?? ''})` : 'null'}`,
						`resource(used): ${resource ? `${(resource as any).id} (${(resource as any).name ?? ''})` : 'null'}`,
					]);
				}

				if (resource) {
					let order = (self.pipeline.store as Store).find((so: GridRowModel) => so.id == serviceorderId) as ServiceOrder;
					if (order) {
						let date = self.scheduler.getDateFromDomEvent(e, "round", false);
						if(isTechnician(resource)) {
							if(self.parentViewModel.profile().ClientConfig.SnapFirstEventOfDayToStartOfDay) {
								if(self.scheduler.eventStore.getEventsForResource(resource).filter((r) => window.Helper.Date.areOnSameDay(date, r.startDate as Date)).length === 0) {
									date = resource.getCalendar().getWorkingTimeRanges(DateHelper.startOf(date), DateHelper.endOf(date))[0].startDate;
								}
							}
						}
						const duration = window.Helper.Scheduler.determineNewEventDuration(
							order,
							self.scheduler.defaultEventDuration,
							self.scheduler.serviceOrderDispatchMaximumDuration,
							self.scheduler.ServiceOrderDispatchIgnoreCalculatedDuration
						);

							const endDate = moment(date).add(duration).toDate();
							const rangeOk = self.scheduler.isDateRangeAvailable(date, endDate, null, resource);

							if (isDragDebugEnabled()) {
								setDragDebugOverlay('drop', [
									`serviceorderId: ${serviceorderId}`,
									`date: ${window.moment(date).format('YYYY-MM-DD HH:mm:ss')}`,
									`endDate: ${window.moment(endDate).format('YYYY-MM-DD HH:mm:ss')}`,
									`isDateRangeAvailable: ${rangeOk}`,
								]);
							}

							if (!rangeOk) {
							e.dataTransfer.dropEffect = "none";
							return;
						}

						let hasSkillFor = window.Helper.Scheduler.hasSkillsForOrder(resource, order.OriginalData, date, endDate);
						let hasAssetsFor = window.Helper.Scheduler.hasAssetsForOrder(resource, order.OriginalData, date, endDate);

						if (hasSkillFor && hasAssetsFor) {
							let violationItems = [];
							for (const r of resources) {
								if (isTechnician(r)) {
									violationItems.push(...window.Helper.Scheduler.PlanningValidations.GetPlanningValidationItemsForOrder(order, r, date, endDate));
								}
							}
							if (self.parentViewModel.profile().ClientConfig.EnablePlanningConfirmations && violationItems.length > 0) {
								window.Helper.Scheduler.ShowPopup(violationItems, async () => {
									await createEvent();
								});
							} else {
								await createEvent();
							}

							async function createEvent() {
								try {
									if (isServiceOrder(order)) {
										const duration = window.Helper.Scheduler.determineNewEventDuration(
											order,
											self.scheduler.defaultEventDuration,
											self.scheduler.serviceOrderDispatchMaximumDuration,
											self.scheduler.ServiceOrderDispatchIgnoreCalculatedDuration
										);
										const technician = window.Helper.Scheduler.determineServiceOrderTeamLeader(order, resources as Technician[]);
										const task = await window.Helper.Scheduler.createEvent(order, technician, date, duration);
										await self.scheduler.eventStore.addAsync(task);
										await self.scheduler.assignmentStore.addAsync(resources.map(r => ({ event: task, resource: r as Technician })));
									}
								} catch (e) {
									window.Log.error(e);
								}
							}
						}
					}
				}

			} else {
				e.dataTransfer.dropEffect = "none";
			}
		});

		this.drag = new window.Sms.Scheduler.Drag({
			schedule: this.scheduler,
			grid: this.pipeline,
			constrain: false,
			enablePlanningConfirmations: () => this.parentViewModel.profile().ClientConfig.EnablePlanningConfirmations,
			targetSelector: ".draggable",
			orderSelector: (g, e) => g.selectedRecords as ServiceOrder[],
			allowSchedulingForPast: () => this.parentViewModel.profile().ClientConfig.AllowSchedulingForPast,
			snapFirstEventOfDayToStartOfDay: () => this.parentViewModel.profile().ClientConfig.SnapFirstEventOfDayToStartOfDay
		});

		this.markerDrag = new window.Sms.Scheduler.Drag({
			schedule: this.scheduler,
			grid: this.pipeline,
			constrain: false,
			enablePlanningConfirmations: () => this.parentViewModel.profile().ClientConfig.EnablePlanningConfirmations,
			targetSelector: ".marker-serviceorder",
			orderSelector: (g, e) => {
				let marker = $(e).data("mapElement") as MapMarker;
				let data = g.data as Model[];
				return [data.find(d => d.id == marker.id) as ServiceOrder];
			},
			allowSchedulingForPast: () => this.parentViewModel.profile().ClientConfig.AllowSchedulingForPast,
			snapFirstEventOfDayToStartOfDay: () => this.parentViewModel.profile().ClientConfig.SnapFirstEventOfDayToStartOfDay
		});
	}

	async loadInlineData(data) {
		await this.scheduler.project.loadInlineData(data);
		SchedulerDetailsViewModel.applyResourceCalendars(this.parentViewModel.profile(), data.resourcesData);
		this.pipeline.store.data = data.serviceOrders;
		//@ts-ignore
		const expandedNodes = JSON.parse(window.Helper.Database.getFromLocalStorage(`${window.Helper.User.getCurrentUserName()}_expandedNodes`)) as string;
		if (expandedNodes != null) {
			let f = (n) => `${!n.parent.isRoot ? f(n.parent) : ''}\\${n.name}`;
			//@ts-ignore
			const nodesToExpand = this.pipeline.store.allRecords.filter(x => x.isParent && expandedNodes.includes(f(x)));
			nodesToExpand.forEach(i => this.pipeline.toggleCollapse(i, false, true));
		}
	}

	getDefaultTaskDurationInPixel() {
		return this.scheduler.timeAxisViewModel.getDistanceForDuration(this.scheduler.defaultEventDuration.asMilliseconds());
	}

	dispose() {
		this.drag?.destroy();
		this.markerDrag?.destroy();
		this.resourceUtilization?.destroy();
		this.mapPanel?.dispose();
		this.mapSplitter.destroy();
		this.pipelineSplitter.destroy();
		this.horizontalSplitter.destroy();
		this.crudManager.destroy();
		this.headerTooltip.destroy();
		this.pipeline.destroy();
		this.scheduler.destroy();
		this.disposed = true;
	}

	initializeViewPresets() {
		const minuteIncrement = Number((window as any)?.Sms?.Scheduler?.Settings?.WorkingTime?.MinutesInterval) || 15;
		PresetManager.registerPreset("weekly", {
			name: window.Helper.String.getTranslatedString('Week'),
			displayDateFormat: 'H:mm',
			tickWidth: 20,
			defaultSpan: 7,
			shiftUnit: 'day',
			shiftIncrement: 1,
			timeResolution: {
				unit: 'minute',
				increment: minuteIncrement
			},
			headers: [
				{
					unit: 'Week',
					align: 'center',
					renderer: (startDate, endDate) => `${window.Helper.String.getTranslatedString("CalendarWeekAbbreviation")} ${DateHelper.format(startDate, 'WW')}`
				},
				{
					unit: 'Day',
					align: 'center',
					dateFormat: 'ddd L'
				},
				{
					unit: 'HOUR',
					align: 'center',
					increment: 4,
					dateFormat: 'HH',
					headerCellCls: "hoursheadercell"
				}
			],
			mainHeaderLevel: 1
		});
		PresetManager.registerPreset("quarterly", {
			name: window.Helper.String.getTranslatedString('Quarter'),
			displayDateFormat: 'L',
			tickWidth: 15,
			defaultSpan: 1,
			shiftUnit: 'day',
			shiftIncrement: 1,
			timeResolution: {
				unit: 'hour',
				increment: 1
			},
			headers: [
				{
					unit: 'Month',
					align: 'center',
					dateFormat: 'MMMM'
				},
				{
					unit: 'Week',
					align: 'center',
					renderer: (startDate, endDate) => `${window.Helper.String.getTranslatedString("CalendarWeekAbbreviation")} ${DateHelper.format(startDate, 'WW')}`
				},
				{
					unit: 'Day',
					align: 'center',
					dateFormat: 'DD',
					headerCellCls: "dayheadercell"
				}
			],
			mainHeaderLevel: 0
		});
		PresetManager.registerPreset("monthly", {
			name: window.Helper.String.getTranslatedString('Month'),
			displayDateFormat: 'L',
			tickWidth: 15,
			defaultSpan: 1,
			shiftUnit: 'day',
			shiftIncrement: 1,
			timeResolution: {
				unit: 'hour',
				increment: 1
			},
			headers: [
				{
					unit: 'Month',
					align: 'center',
					dateFormat: 'MMMM'
				},
				{
					unit: 'Week',
					align: 'center',
					renderer: (startDate, endDate) => `${window.Helper.String.getTranslatedString("CalendarWeekAbbreviation")} ${DateHelper.format(startDate, 'WW')}`
				},
				{
					unit: 'Day',
					align: 'center',
					dateFormat: 'dd DD'
				}
			],
			mainHeaderLevel: 0
		});
		PresetManager.registerPreset("daily", {
			name: window.Helper.String.getTranslatedString('Day'),
			displayDateFormat: 'H:mm',
			tickWidth: 50,
			defaultSpan: 1,
			shiftUnit: 'day',
			shiftIncrement: 1,
			timeResolution: {
				unit: 'minute',
				increment: minuteIncrement
			},
			headers: [
				{
					unit: 'Week',
					align: 'center',
					renderer: (startDate, endDate) => `${window.Helper.String.getTranslatedString("CalendarWeekAbbreviation")} ${DateHelper.format(startDate, 'WW')}`
				},
				{
					unit: 'Day',
					align: 'center',
					dateFormat: 'ddd L'
				},
				{
					unit: 'HOUR',
					align: 'center',
					increment: 1,
					headerCellCls: "hoursheadercell",
					renderer: (startDate, endDate, cfg) => {
						let diff = DateHelper.diff(startDate, endDate, "hours");

						if (diff > 1) {
							cfg.headerCellCls = 'hoursheadercell sch-hdr-startend';
							return `<span>${DateHelper.format(startDate, 'HH')}</span><span>${DateHelper.format(DateHelper.add(endDate, -1, "hour"), 'HH')}</span>`;
						} else {
							return DateHelper.format(startDate, 'HH');
						}
					}
				}
			],
			mainHeaderLevel: 1
		});
	}

	initMap() {
		const self = this;
		self.mapSplitter = new Splitter({
			appendTo: 'scheduler',
			maxWidth: 6.5,
		});

		self.mapPanel = new MapPanel({
			ref: 'map',
			appendTo: 'scheduler',
			stateId: 'mapPanel',
			eventStore: self.scheduler.project.eventStore,
			timeAxis: self.scheduler.timeAxis,
			listeners: {
				markerclick: self.onMapMarkerClick.bind(self)
			},
			monitorResize: true,
			hidden: true
		}, self.createAllMarkersObservableArray(), self.defaultTaskDurationInPixel);
		self.scheduler.on("eventClick", async (event) => {
			self.mapPanel?.scrollMarkerIntoViewById(event.eventRecord.id, MapMarkerType.ServiceOrderDispatch);
		});
		self.scheduler.on("afterEventSave", async (event) => {
			self.mapPanel?.scrollMarkerIntoViewById(event.eventRecord.id, MapMarkerType.ServiceOrderDispatch);
		});
		self.scheduler.on("cellClick", async (event) => {
			if (event.record && event.record instanceof Technician &&
				event.column && event.column instanceof Model && !(event.column instanceof TimeAxisColumn)) {
				self.mapPanel?.scrollMarkerIntoViewById(event.record.id, MapMarkerType.Resource);
			}
		});
	}

	async toggleMap(showPanel: boolean) {
		const self = this;
		if(self.mapPanel == null) {
			self.initMap();
		}
		await this.mapPanel.toggle(showPanel);
		if (!this.mapPanel.isVisible) {
			await this.mapSplitter.hide();
		} else {
			await this.mapSplitter.show();
		}
	}

	async toggleResourceUtilization(showPanel: boolean) {
		let self = this;

		self.resourceUtilization ??= new CrmResourceUtilization({
			project: self.scheduler.project,
			stateId: 'resourceUtilization',
			ref: 'resourceUtilization',
			appendTo: 'schedulertimelinepanel',
			hideHeaders: true,
			partner: self.scheduler,
			showBarTip: true,
			flex: 1,
			hidden: true,
			getBarText(datum: ResourceAllocationInterval | AssignmentAllocationInterval): string {
				const view = this.owner;

				let result = view.getBarTextDefault(...arguments);

				if (result && datum.resource) {
					if (isTechnician(datum.resource)) {
						if (datum.resource.OriginalData.Discharged && !datum.resource.isActiveAtDate(datum.tick.startDate)) {
							datum.isOverallocated = true;
						}
					}
				}

				return result;
			}
		});

		if (showPanel) {
			await self.resourceUtilization.show();
			await self.horizontalSplitter.show();
		} else {
			await self.resourceUtilization.hide(true);
			await self.horizontalSplitter.hide();
		}
		this.resourceUtilization.saveState();
	}

	// When data changes in the eventStore, update the map markers accordingly
	onEventStoreChange(event) {
		switch (event.action) {
			case 'add':
			case 'dataset':

				if (event.action === 'dataset') {
					this.removeAllMarkers(MapMarkerType.ServiceOrderDispatch);
				}
				event.records.forEach(eventRecord => this.addMarker(eventRecord, MapMarkerType.ServiceOrderDispatch));
				break;

			case 'remove':
				event.records.forEach(event => this.removeMarker(event, MapMarkerType.ServiceOrderDispatch));
				break;

			case 'update': {
				const eventRecord = event.record;

				this.removeMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);
				this.addMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);

				break;
			}

			case 'filter': {
				const renderedMarkers = [];

				let eventMarkers = this.eventMarkers();

				this.scheduler.project.eventStore.query(rec => eventMarkers.find(m => m.id == rec.id), true).forEach(eventRecord => {
					if (!event.records.includes(eventRecord)) {
						this.removeMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);
					} else {
						renderedMarkers.push(eventRecord);
					}
				});

				event.records.forEach(eventRecord => {
					if (!renderedMarkers.includes(eventRecord)) {
						this.addMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);
					}
				});

				break;
			}
		}
	}

	onResourceStoreChange(event) {
		switch (event.action) {
			case 'add':
			case 'dataset':

				if (event.action === 'dataset') {
					this.removeAllMarkers(MapMarkerType.Resource);
				}
				event.records.forEach(resourceRecord => this.addMarker(resourceRecord, MapMarkerType.Resource));
				break;

			case 'remove':
				event.records.forEach(resourceRecord => this.removeMarker(resourceRecord, MapMarkerType.Resource));
				break;

			case 'update': {
				const resourceRecord = event.record;

				this.removeMarker(resourceRecord, MapMarkerType.Resource);
				this.addMarker(resourceRecord, MapMarkerType.Resource);

				break;
			}

			case 'filter': {
				const renderedMarkers = [];

				let resourceMarkers = this.resourceMarkers();

				this.scheduler.project.resourceStore.query(rec => resourceMarkers.find(m => m.id == rec.id), true).forEach(resourceRecord => {
					if (!event.records.includes(resourceRecord)) {
						this.removeMarker(resourceRecord, MapMarkerType.Resource);
					} else {
						renderedMarkers.push(resourceRecord);
					}
				});

				event.records.forEach(resourceRecord => {
					if (!renderedMarkers.includes(resourceRecord)) {
						this.addMarker(resourceRecord, MapMarkerType.Resource);
					}
				});

				break;
			}
		}

		this.resourceStoreChangeTrigger(this.resourceStoreChangeTrigger() + 1);
	}

	// Only show markers for events inside currently viewed time axis
	onTimeAxisReconfigure({source: timeAxis}) {
		if (timeAxis.owner.isDestroyed) {
			return;
		}

		this.scheduler.project.eventStore.forEach(eventRecord => {
			this.removeMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);
			this.addMarker(eventRecord, MapMarkerType.ServiceOrderDispatch);
		});
	}

	// Puts a marker on the map, if it has lat/lon specified + the timespan intersects the time axis
	addMarker(record: unknown, markerType: MapMarkerType) {
		let self = this;
		if (markerType === MapMarkerType.ServiceOrderDispatch) {
			const dispatch = record as EventModel;
			if (!isDispatch(dispatch) || !dispatch.ServiceOrder.Latitude || !dispatch.ServiceOrder.Longitude) return;

			if (window.Helper.Date.areRangesOverlapping(self.scheduler.timeAxis.startDate, self.scheduler.timeAxis.endDate, dispatch.startDate as Date, dispatch.endDate as Date)) {
				let marker = {
					id: dispatch.id as string,
					type: MapMarkerType.ServiceOrderDispatch,
					Latitude: dispatch.ServiceOrder.Latitude,
					Longitude: dispatch.ServiceOrder.Longitude,
					PopupInformation: dispatch.ServiceOrder.getPopupInformation(),
					ClassName: 'marker-dispatch',
					//@ts-ignore
					MarkerContent: 'D',
					MarkerColor: 'red',
				};

				self.eventMarkers.push(marker);
				self.mapPanel?.scrollMarkerIntoView(marker);
			}
		} else if (markerType === MapMarkerType.Resource) {
			const resourceRecord = record as ResourceModel;

			if (!isTechnician(resourceRecord)
				|| resourceRecord.Latitude == null
				|| resourceRecord.Longitude == null
				|| (resourceRecord.LastStatusUpdate != null
					&& DateHelper.diff(resourceRecord.LastStatusUpdate, new Date(), 'h') > 24)) {
				return;
			}

			let marker = {
				id: resourceRecord.OriginalData.Id,
				type: MapMarkerType.Resource,
				Latitude: resourceRecord.Latitude,
				Longitude: resourceRecord.Longitude,
				IconName: "marker_dude",
				PopupInformation: window.Helper.User.getDisplayName(resourceRecord.OriginalData)
			} as MapMarker;

			self.resourceMarkers.push(marker);
			self.mapPanel?.scrollMarkerIntoView(marker);
		}
	}

	removeMarker(record: unknown, markerType: MapMarkerType) {
		if (markerType === MapMarkerType.ServiceOrderDispatch) {
			this.eventMarkers.remove(m => m.id == (record as EventModel).id);
		} else if (markerType === MapMarkerType.Resource) {
			this.resourceMarkers.remove(m => m.id == (record as EventModel).id);
		}
	}

	removeAllMarkers(markerType: MapMarkerType) {
		if (markerType === MapMarkerType.ServiceOrderDispatch) {
			this.scheduler.project.eventStore.forEach(eventRecord => this.removeMarker(eventRecord, MapMarkerType.ServiceOrderDispatch));
		} else if (markerType === MapMarkerType.Resource) {
			this.scheduler.project.resourceStore.forEach(resourceRecord => this.removeMarker(resourceRecord, MapMarkerType.Resource));
		}
	}

	async onMapMarkerClick(e) {
		let marker: MapMarker = e.marker;
		if (marker.type == MapMarkerType.ServiceOrderDispatch) {
			// When a map marker is clicked, scroll the event bar into view and highlight it
			const eventRecord = this.scheduler.project.eventStore.getById(marker.id) as EventModel;
			const resourceRecord = this.scheduler.resourceStore.find(r => r.id == eventRecord.resourceId) as ResourceModel;
			await this.scheduler.scrollResourceEventIntoView(resourceRecord, eventRecord, {
				animate: true,
				highlight: true
			});
			this.scheduler.selectedEvents = [eventRecord];
		} else if (marker.type == MapMarkerType.ServiceOrder) {
			this.pipeline.scrollRowIntoView(marker.id, {animate: true, highlight: true});
		} else if (marker.type == MapMarkerType.Resource || marker.type == MapMarkerType.TechnicianHome) {
			const resourceRecord = this.scheduler.project.resourceStore.getById(marker.id) as ResourceModel;
			this.scheduler.scrollResourceIntoView(resourceRecord, {animate: true, highlight: true});
			this.scheduler.selectedRows = [resourceRecord];
		}
	}

	createAllMarkersObservableArray(): KnockoutObservableArray<KnockoutObservable<MapMarker[]>> {
		let self = this;

		let pipelineSelectedServiceOrdersMarkers = ko.computed(() => {
			let serviceOrders = self.pipelineSelectedServiceOrders();
			let markers = serviceOrders
				.filter(so => so.OriginalData.Latitude != null && so.OriginalData.Longitude != null)
				.map(so => ({
					id: so.OriginalData.Id,
					type: MapMarkerType.ServiceOrder,
					Latitude: so.OriginalData.Latitude,
					Longitude: so.OriginalData.Longitude,
					IconName: "marker_contract",
					ClassName: "marker-serviceorder",
					//@ts-ignore
					MarkerContent: window.Helper.ServiceOrder.getTypeAbbreviation(so.OriginalData, self.parentViewModel.lookups.serviceOrderTypes),
					MarkerColor: window.Helper.Lookup.getLookupColor(self.parentViewModel.lookups.serviceOrderTypes, so.OriginalData.TypeKey),
					PopupInformation: so.getPopupInformation(),
					DurationInPx: self.scheduler.timeAxisViewModel.getDistanceForDuration(window.Helper.Scheduler.determineNewEventDuration(
						so,
						self.scheduler.defaultEventDuration,
						self.scheduler.serviceOrderDispatchMaximumDuration,
						self.scheduler.ServiceOrderDispatchIgnoreCalculatedDuration
					).asMilliseconds())
				} as MapMarker));

			if (markers && markers.length > 0) {
				self.mapPanel?.scrollMarkerIntoView(markers[markers.length - 1]);
			}

			return markers;
		});

		let techniciansHomeMarkers = asyncComputed(async () => {
			self.resourceStoreChangeTrigger();
			const technicians = self.scheduler.resourceStore.allRecords.filter(isTechnician);
			let techniciansHomeAddresses = [];
			for (let t of technicians) {
				const homeAddress = await t.getHomeAddress();
				if (homeAddress) {
					techniciansHomeAddresses.push({ technician: t.OriginalData, address: homeAddress});
				}
			}

			let markers = techniciansHomeAddresses
				.filter(r => r.address.Latitude != null && r.address.Longitude != null)
				.map(a => ({
					id: a.technician.Id,
					type: MapMarkerType.TechnicianHome,
					Latitude: a.address.Latitude,
					Longitude: a.address.Longitude,
					IconName: "marker_home",
					PopupInformation: `${window.Helper.User.getDisplayName(a.technician)}, ${window.Helper.String.getTranslatedString("Home")}`
				} as MapMarker));

			if (markers && markers.length > 0) {
				self.mapPanel?.scrollMarkerIntoView(markers[markers.length - 1]);
			}

			return markers;
		}, []);

		return ko.observableArray([
			pipelineSelectedServiceOrdersMarkers as KnockoutObservable<MapMarker[]>,
			self.resourceMarkers as KnockoutObservable<MapMarker[]>,
			techniciansHomeMarkers as KnockoutObservable<MapMarker[]>,
			self.eventMarkers as KnockoutObservable<MapMarker[]>
		]);
	}

	triggerPendingChanges() {
		this.pendingChangesTrigger(this.pendingChangesTrigger() + 1);
	}

	updatePendingChanges() {
		let result = 0;
		let changingDispatchIds = new Set<string>();

		const crudManager = this.crudManager;
		if (crudManager) {
			const dispatchIds: string[] = [];
			const states = [
				{
					name: "added"
				},
				{
					name: "modified",
					changeName: "updated",
					onlyIfInChanges: true
				},
				{
					name: "removed",
					exceptionSet: this.detachFromStore
				}
			];

			const stores = [
				{
					storeId: StoreIds.AssignmentStore,
					idSelector: x => x?.eventId
				},
				{
					storeId: StoreIds.DispatchStore,
					idSelector: x => x?.OriginalData?.Id
				}
			];

			for (const store of stores) {
				const changedStore = crudManager?.getCrudStore?.(store.storeId);
				if (changedStore) {
					for (const state of states) {
						const changedState = changedStore[state.name];
						if (changedState?.values) {
							dispatchIds.push(...changedState
								.values
								.filter(x => !state?.exceptionSet?.has(x))
								.filter(x => !state.onlyIfInChanges || (crudManager.changes?.[store.storeId]?.[state.changeName || state.name]?.map(store.idSelector).includes(store.idSelector(x))))
								.map(store.idSelector));
						}
					}
				}
			}

			changingDispatchIds = new Set(dispatchIds.filter(Boolean));
			result = changingDispatchIds.size;
		}

		this.pendingChanges(result);

		if (this.pipeline) {
			const changingDispatches = this.scheduler.eventStore.allRecords.filter((e): e is Dispatch => isDispatch(e) && changingDispatchIds.has(e.id as string));
			const affectedServiceOrders = new WeakSet(changingDispatches.map(d => d.ServiceOrder));

			this.pipeline.assignClassToRecords("serviceorder-dirtyrow", affectedServiceOrders);
		}
	}

	async getRoute(routeConfig: RouteConfig, startDate: Date = this.scheduler.timeAxis.startDate, endDate: Date = this.scheduler.timeAxis.endDate, muteWarnings: boolean = false) {
		if(routeConfig == null) {
			return;
		}
		const self = this;
		if(self.mapPanel == null) {
			self.initMap();
		}
		const {routeData, useTechnicianHomeAddressAsOrigin, useTechnicianHomeAddressAsFinalDestination, getRoutePerDay} = routeConfig;
		const missingCoordinatesFor = new Set<string>();
		const foundCoordinatesFor = new Set<string>();
		const geoJsonFeatureCollection: FeatureCollection = {
			type: 'FeatureCollection',
			features: []
		};
		if (!self.googleMapsLoader) {
			self.googleMapsLoader = new (await import("@googlemaps/js-api-loader")).Loader({
				apiKey: window.Main.Settings.Geocoder.GoogleMapsApiKey,
				version: window.Main.Settings.Geocoder.GoogleMapsApiVersion,
			});
		}

		if(self.mapPanel.hasActiveRoute) {
			await self.clearRoute(false);
		}

		//map to hold the marker content. important to multi assignment dispatches to get the right content.
		const dispatchMarkerContent = new Map<string, string[]>();

		const periods = getRoutePerDay ?
			window.Helper.Date.getDatesOfRange(startDate, endDate).map(d => ({
				from: d,
				to: moment(d).endOf("day").toDate()
			})) :
			[{from: startDate, to: endDate}];

		const {DirectionsService, TravelMode} = await self.googleMapsLoader.importLibrary("routes");
		const service = new DirectionsService();
		for (const period of periods) {
			for (const data of routeData) {
				if (data.technician() == null || data.color() == null) {
					continue;
				}

				const technician = self.scheduler.resourceStore.find((r: ResourceModel) => r.id == data.technician()) as Technician;
				const visibleEvents = self.scheduler.eventStore.getEvents({
					resourceRecord: technician,
					startDate: period.from,
					endDate: period.to,
					filter: function (r: EventModel): r is Dispatch {
						return isDispatch(r) && r.ServiceOrder.Latitude != null && r.ServiceOrder.Longitude != null;
					},
					dateMap: false
				}) as Dispatch[];
				const eventsWithValidCoordinates = visibleEvents.toSorted((a, b) => (a.startDate as Date).getTime() - (b.startDate as Date).getTime());

				const periodEventsWithValidCoordinates = eventsWithValidCoordinates.filter(e => e.startDate < period.to && period.from < e.endDate);

				if (periodEventsWithValidCoordinates.length == 0) {
					missingCoordinatesFor.add(technician.name);
					continue;
				}

				const eventCoordinates: google.maps.LatLngLiteral[] = periodEventsWithValidCoordinates
					.map(e => ({lat: e.ServiceOrder.Latitude, lng: e.ServiceOrder.Longitude}));

				if (useTechnicianHomeAddressAsOrigin || useTechnicianHomeAddressAsFinalDestination) {
					const homeAddress = await technician.getHomeAddress();
					if (homeAddress && homeAddress.Latitude !== null && homeAddress.Longitude !== null) {
						if (useTechnicianHomeAddressAsOrigin) {
							eventCoordinates.unshift({lat: homeAddress.Latitude, lng: homeAddress.Longitude});
						}
						if (useTechnicianHomeAddressAsFinalDestination) {
							eventCoordinates.push({lat: homeAddress.Latitude, lng: homeAddress.Longitude});
						}
					}
				}

				if (eventCoordinates.length < 2 || (eventCoordinates.length == 2 && eventCoordinates[0].lat == eventCoordinates[1].lat && eventCoordinates[0].lng == eventCoordinates[1].lng)) {
					missingCoordinatesFor.add(technician.name);
					continue;
				} else {
					foundCoordinatesFor.add(technician.name);
				}

				try {
					const request: google.maps.DirectionsRequest = {
						origin: eventCoordinates[0],
						destination: eventCoordinates.at(-1),
						waypoints: eventCoordinates.slice(1, -1).map(ec => ({
							location: ec,
							stopover: true
						})),
						travelMode: TravelMode.DRIVING,
					};
					const result = await service.route(request);
					const routes = result.routes;

					//routes will always contain one route unless we use provideRouteAlternatives parameter.
					routes.forEach((route) => {
						route.legs.forEach((leg, index) => {
							const steps = leg.steps;
							steps.forEach((step) => {
								const coordinates = step.path.map(point => [point.lng(), point.lat()]);
								const feature: Feature = {
									type: 'Feature',
									properties: {
										name: data.technician(),
										popupContent: StringHelper.decodeHtml(`
										<div style="font-size: 1.75em">
											<h5>${technician.name} ${(getRoutePerDay ? `- ${moment(period.from).format("L")}` : "")}</h5>
											<p class="c-gray" style="margin: 5px 0 5px 0">${leg.start_address}</p>
											<p class="c-gray" style="margin: 5px 0 5px 0">${leg.end_address}</p>
											<p class="c-gray" style="margin: 5px 0 5px 0">${leg.distance.text} - ${leg.duration.text}</p>
										</div>
									`),
										color: data.color()
									},
									geometry: {
										type: 'LineString',
										coordinates: coordinates
									}
								};
								geoJsonFeatureCollection.features.push(feature);
							});
						});
					});
					periodEventsWithValidCoordinates.forEach((pe, index) => addDispatchMarkerContent(pe.id as string, `${index + 1}`));
				} catch (e) {
					let errorMessage: string;
					if (typeof e === "string") {
						errorMessage = e.toUpperCase();
					} else if (e instanceof Error) {
						errorMessage = window.Helper.String.tryExtractErrorMessageValue(e.message) ?? e.message;
					}
					swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
					window.Log.error(errorMessage);
				}
				this.mapPanel.broadcastMessage({type: MapMessageType.markers, data: self.mapPanel.mapComponent().markers()});
				if(this.mapPanel.isUndocked()) {
					this.mapPanel.broadcastMessage({type: MapMessageType.mapView, data: {lat: eventsWithValidCoordinates[0].ServiceOrder.Latitude, lng: eventsWithValidCoordinates[0].ServiceOrder.Longitude}});
				} else {
					this.mapPanel.mapComponent().setView({lat: eventsWithValidCoordinates[0].ServiceOrder.Latitude, lng: eventsWithValidCoordinates[0].ServiceOrder.Longitude});
				}
				dispatchMarkerContent.forEach((contents, id) => self.mapPanel.mapComponent().updateMarkerContent(id, contents.join()));
			}
		}

		const noRouteTechnicians = Array.from(missingCoordinatesFor).filter(t => !foundCoordinatesFor.has(t));
		if (!muteWarnings && noRouteTechnicians.length > 0) {
			window.swal(window.Helper.String.getTranslatedString("Warning"), window.Helper.getTranslatedString("RouteCouldNotBeCalculatedWarning").replace("{0}", noRouteTechnicians.join(", ")), "warning");
		}

		self.mapPanel.showRouteOnMap(geoJsonFeatureCollection);

		function addDispatchMarkerContent(id: string, content: string) {
			let contentArray: string[];

			if (!dispatchMarkerContent.has(id)) {
				contentArray = [];
				dispatchMarkerContent.set(id, contentArray);
			} else {
				contentArray = dispatchMarkerContent.get(id);
			}

			contentArray.push(content);
		}
	}

	async getRoutesQuick() {
		const selectedTechnicians = this.scheduler.selectedRecords as Technician[];

		if (selectedTechnicians.length == 0) {
			window.swal(window.Helper.String.getTranslatedString("Warning"), window.Helper.String.getTranslatedString("SelectResourceForRouteWarning"), "warning");
			return;
		}

		const colors = this.parentViewModel.profile().ClientConfig.RouteColors;
		this.routeConfig({
			routeData: selectedTechnicians.map((t, index) =>
				new RouteData(ko.observable(t.OriginalData.Id), ko.observable(colors[index % colors.length]))
			),
			useTechnicianHomeAddressAsOrigin: false,
			useTechnicianHomeAddressAsFinalDestination: false,
			getRoutePerDay: false
		});

		await this.getRoute(this.routeConfig());
	}

	async clearRoute(clearRouteConfig: boolean = true) {
		let self = this;
		if(clearRouteConfig) {
			self.routeConfig(null);
		}
		self.mapPanel?.clearRoute();
	}

	gotoDate(newDate: Date, newPreset: string = null) {
		if (newDate) {
			//We need to set _isUserAction to true, because datepicker's change lister has a condition to only take action for user input and this is a user input.
			this.scheduler.widgetMap.datePicker["_isUserAction"] = true;
			this.scheduler.widgetMap.datePicker["value"] = newDate;
		}

		if (newPreset) {
			this.scheduler.widgetMap.presetCombo["value"] = newPreset;
		}
	}

	gotoPreviousDate() {
		this?.scheduler?.widgetMap?.['datePicker']?.["onBackClick"]?.();
	}

	gotoNextDate() {
		this?.scheduler?.widgetMap?.['datePicker']?.["onForwardClick"]?.();
	}
	handleAssignmentRemoval(
		scheduler: Timeline,
		assignmentRecord: Assignment,
		eventRecord: EventModel
	) {
		scheduler.assignmentStore.remove(assignmentRecord);
		const remainingAssignments = scheduler.assignmentStore.getAssignmentsForEvent(eventRecord as TimeSpan) as Assignment[];
		const dispatch = eventRecord as Dispatch;

		const newTeamLeader = window.Helper.Scheduler.determineTeamLeader(dispatch, remainingAssignments);
		if (dispatch.OriginalData.Username != newTeamLeader) {
			eventRecord.set({
				Username: newTeamLeader
			});
		}

		scheduler.refreshRows();
	}
	}
namespace("Sms.Scheduler.ViewModels").Scheduler = Scheduler;