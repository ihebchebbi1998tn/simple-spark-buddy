import {namespace} from "@Main/namespace";
import {isDispatch} from "../Model/Dispatch";
import {isTechnician, Technician} from "../Model/Technicians";
import {Assignment} from "../Model/Assignment";
import {Article, isArticle} from "../Model/Article";
import {
	CalendarModel,
	DateHelper,
	type EventModel,
	ResourceModel,
	StringHelper,
	TimeSpan,
	Model
} from "@bryntum/schedulerpro";
import {SchedulerDetailsViewModel} from "../SchedulerDetailsViewModel";
import moment from "moment";
import {isAbsence} from "../Model/Absence";


export class SchedulerEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	canSelectMultipleTechnicians = ko.observable<boolean>(false);
	dispatch: KnockoutObservable<any> = ko.observable(null);
	type: string = null;

	eventRecord: EventModel = null;
	parentViewModel: SchedulerDetailsViewModel = null;
	isReleased = ko.observable<boolean>(true);
	readonly = ko.observable<boolean>(false);
	loading = ko.observable<boolean>(false);

	//Technician's Map to hold both in and out of store resource
	techniciansMap = new Map<string, Technician>();

	manuallyScheduled: boolean;
	duration: KnockoutComputed<string>;
	subscriptions: KnockoutSubscription[] = [];

	assignments: Assignment[];
	articleUserRelationshipValidationText = ko.observable<string>("");
	assignedJobs = ko.observableArray<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>([]);
	assignedResources = ko.observableArray<Main.Rest.Model.Main_User>([]).extend({
		validation: {
			message: window.Helper.String.getTranslatedString("MinResourceNumber"),
			validator: function (val: Main.Rest.Model.Main_User[], minLength: number) {
				return Array.isArray(val) && val.length >= minLength;
			},
			params: 1,
			onlyIf: () => isDispatch(this.eventRecord as Model),
			propertyName: 'assignedResources'
		}
	});
	availableResources: Technician[];
	assignedTools = ko.observableArray<Crm.Article.Rest.Model.CrmArticle_Article>([]);
	availableTools: Article[];
	errors = ko.validation.group([this.dispatch, this.assignedResources, this.assignedTools], {deep: true});
	initialAssignedResources = ko.observableArray<Main.Rest.Model.Main_User>([]);
	initialAssignedTools = ko.observableArray<Crm.Article.Rest.Model.CrmArticle_Article>([]);
	warnings = ko.observableArray<string>([]);

	lookups: LookupType = {
		timeEntryTypes: {$tableName: "CrmPerDiem_TimeEntryType"},
		articleDowntimeReason: {$tableName: 'CrmArticle_ArticleDowntimeReason'},
	};

	constructor(params: { eventRecord: EventModel, parentViewModel: SchedulerDetailsViewModel }) {
		super();
		this.eventRecord = params.eventRecord;
		this.parentViewModel = params.parentViewModel;

		this.manuallyScheduled = this.eventRecord.manuallyScheduled;
	}

	async init(id: string, args): Promise<void> {
		let self = this;
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);

		//@ts-ignore
		const observable = window.Helper.Database.createClone(this.eventRecord.OriginalData).asKoObservable();
		observable.innerInstance.addValidationRules();
		if (!JSON.parse(window.Sms.Scheduler.Settings.WorkingTime.IgnoreWorkingTimesInEndDateCalculation) &&
			isDispatch(this.eventRecord) && this.manuallyScheduled) {
			this.manuallyScheduled = false;
		}

		this.dispatch(observable);
		if(isDispatch(this.eventRecord) || isAbsence(this.eventRecord)) {
			this.readonly(this.eventRecord.readonly);
		}
		if (isDispatch(this.eventRecord)) {
			this.type = "ServiceOrderDispatch";
			let dispatch = this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
			this.availableResources = this.parentViewModel.scheduler().resourceStore.records.filter(isTechnician);
			this.availableTools = this.parentViewModel.scheduler().resourceStore.records.filter(isArticle);

			this.assignments = this.parentViewModel.scheduler().project.assignmentStore.getAssignmentsForEvent(this.eventRecord) as Assignment[];
			if (this.parentViewModel.profile().ClientConfig.AllowSchedulingForPast === false && window.moment(this.eventRecord.StartTime).isBefore(new Date(), "minute")) {
				this.warnings.push(window.Helper.String.getTranslatedString("EditingDispatchesInPastWarning"));
			}
			//Find the id of those technicians who are assigned but are not in the loaded profile
			const notLoadedTechnicianIds = this.assignments
				.filter(assignment => !assignment.resource && !window.Helper.String.isGuid(assignment.OriginalData.ResourceKey))
				.map(assignment => assignment.OriginalData.ResourceKey);
			//Fill the technician's map for them
			const missedTechnicians = await this.parentViewModel.loadTechnicians(notLoadedTechnicianIds);
			await SchedulerDetailsViewModel.applyResourceCalendars(self.parentViewModel.profile(), missedTechnicians);
			missedTechnicians.forEach(t => this.techniciansMap.set(t.OriginalData.Id, t));
			(await this.parentViewModel.loadTechnicians(notLoadedTechnicianIds))
				.forEach(t => this.techniciansMap.set(t.OriginalData.Id, t));
			//Fill the technician's map from assignments
			this.assignments
				.filter(a => isTechnician(a.resource as ResourceModel))
				.forEach(a => this.techniciansMap.set(a.OriginalData.ResourceKey, a.resource as Technician));
			//Add those not in resourceStore to the list of available resources
			this.availableResources.push(...notLoadedTechnicianIds.filter(id => this.techniciansMap.has(id)).map(id => this.techniciansMap.get(id)));
			//Fill assigneds based on the map which now contains only assigneds
			this.assignedResources(this.availableResources.filter(it => this.techniciansMap.has(it.OriginalData.Id)).map(r => r.OriginalData));
			this.initialAssignedResources([...this.assignedResources()]);
			//Add the rest (those in the profile) to the map
			this.availableResources
				.filter(r => !this.techniciansMap.has(r.OriginalData.Id))
				.forEach(r => this.techniciansMap.set(r.OriginalData.Id, r));

			//Tool's Map to hold both in and out of store resource
			const toolsMap = new Map<string, Article>();
			//Find the id of those tools that are assigned but are not in the loaded profile
			const notLoadedToolIds = this.assignments
				.filter(assignment => !assignment.resource && window.Helper.String.isGuid(assignment.OriginalData.ResourceKey))
				.map(assignment => assignment.OriginalData.ResourceKey);
			//Fill the tool's map for them
			const notLoadedArticles = await this.parentViewModel.loadArticles(notLoadedToolIds);
			notLoadedArticles.forEach(t => toolsMap.set(t.id as string, t));
			//Fill the tool's map from assignments
			this.assignments
				.filter(a => a.resource && isArticle(a.resource as ResourceModel))
				.forEach(a => toolsMap.set(a.resourceId as string, a.resource as Article));
			//Add those not in resourceStore to the list of available tools
			this.availableTools.push(...notLoadedToolIds.filter(id => toolsMap.has(id)).map(id => toolsMap.get(id)));
			//Fill assigneds based on the map which now contains only assigneds
			this.assignedTools(this.availableTools.filter(it => toolsMap.has(it.OriginalData.Id)).map(r => r.OriginalData));
			this.initialAssignedTools([...this.assignedTools()]);
			//Add the rest (thos in the profile) to the map
			this.availableTools
				.filter(r => !toolsMap.has(r.OriginalData.Id))
				.forEach(r => toolsMap.set(r.OriginalData.Id, r));

			const jobIds = this.eventRecord.ServiceOrderTimeDispatches?.map(x => x.ServiceOrderTimeId) ?? [];
			let assignedJobs = await window.database.CrmService_ServiceOrderTime.filter(function (it) {
				return it.Id in this.ids;
			}, {ids: jobIds}).toArray();
			this.assignedJobs(assignedJobs);

			if (dispatch.StatusKey() === 'Scheduled') {
				this.isReleased(false);
			}

			window.ko.validation.addRule(dispatch.Date, {
				message: window.Helper.String.getTranslatedString("MissingSkillsOrAssetsValidation"),
				//@ts-ignore
				validator: (val: Date) => {
					if (isDispatch(self.eventRecord)) {
						if (!self.assignedResources().some(a => self.techniciansMap.has(a.Id))) return true;

						const teamLeader = window.Helper.Scheduler.determineTeamLeader(self.eventRecord, self.assignedResources().map(it => ({
							resourceId: it.Id,
							resource: self.techniciansMap.get(it.Id)
						})));
						const mainResource = self.techniciansMap.get(teamLeader);

						const endDate = self.dispatch().EndDate();
						return window.Helper.Scheduler.hasSkillsForOrder(mainResource, self.eventRecord.ServiceOrder.OriginalData, val, endDate)
							&& window.Helper.Scheduler.hasAssetsForOrder(mainResource, self.eventRecord.ServiceOrder.OriginalData, val, endDate);
					}
				},
				onlyIf: () => self.assignedResources().length > 0,
				propertyName: "Date"
			});

			const calculateCurrentDurationInMinutes = () => {
				let duration: number;

				if (self.manuallyScheduled) {
					duration = moment(dispatch.EndDate()).diff(dispatch.Date(), "minutes");
				} else {
					try {
						if (isDispatch(self.eventRecord) && self.assignedResources().length > 0) {
							const teamLeader = window.Helper.Scheduler.determineTeamLeader(self.eventRecord, self.assignedResources().map(it => ({
								resourceId: it.Id,
								resource: self.techniciansMap.get(it.Id)
							})));
							const mainResource = self.techniciansMap.get(teamLeader);

							let calendar = mainResource.calendar as CalendarModel;
							if (!calendar || typeof calendar.calculateDurationMs !== 'function') {
								calendar = self.parentViewModel.scheduler().project.calendar as CalendarModel;
							}
							if (!calendar || typeof calendar.calculateDurationMs !== 'function') {
								duration = moment(self.dispatch().EndDate()).diff(self.dispatch().Date(), "minutes");
							} else {
								duration = moment.duration(calendar.calculateDurationMs(self.dispatch().Date(), self.dispatch().EndDate()), "milliseconds").asMinutes();
							}
						}
					} catch (e) {
						duration = 0;
						window.Log.error(e);
						window.swal(window.Helper.String.getTranslatedString("UnknownError"),
							window.Helper.String.getTranslatedString("SomethingWentWrong"),
							"error");
					}
				}

				return duration;
			}
			const updateNetWorkMinutes = () => dispatch.NetWorkMinutes(calculateCurrentDurationInMinutes());

			const calculateEndDate = () => {
				let endDate: Date = null;

				if (self.manuallyScheduled) {
					endDate = moment(dispatch.Date()).add(dispatch.NetWorkMinutes(), "minutes").toDate();
				} else {
					try {
						if (isDispatch(self.eventRecord) && self.assignedResources().length > 0) {
							const teamLeader = window.Helper.Scheduler.determineTeamLeader(self.eventRecord, self.assignedResources().map(it => ({
								resourceId: it.Id,
								resource: self.techniciansMap.get(it.Id)
							})));
							const mainResource = self.techniciansMap.get(teamLeader);

							let calendar = mainResource.calendar as CalendarModel;
							if (!calendar || typeof calendar.calculateEndDate !== 'function') {
								calendar = self.parentViewModel.scheduler().project.calendar as CalendarModel;
							}
							if (!calendar || typeof calendar.calculateEndDate !== 'function') {
								window.Log.warn("No valid calendar found for end date calculation, using default duration");
								endDate = moment(self.dispatch().Date()).add(self.dispatch().NetWorkMinutes(), "minutes").toDate();
							} else {
								endDate = calendar.calculateEndDate(self.dispatch().Date(), window.moment.duration(self.dispatch().NetWorkMinutes(), "minutes").asMilliseconds());
							}
						}
					} catch (e) {
						window.Log.error(e);
						window.swal(window.Helper.String.getTranslatedString("UnknownError"),
							window.Helper.String.getTranslatedString("SomethingWentWrong"),
							"error");
					}
				}

				return endDate;
			}
			const updateEndDate = () => {
				const endDate = calculateEndDate();

				if (endDate) {
					dispatch.EndDate(endDate);
				}
			}

			//By this design duration is more important thant end date
			if (dispatch.NetWorkMinutes() == null) {
				//Net work in minutes will be null for migrated data.
				updateNetWorkMinutes();
			} else {
				//It is safer to always calculate end date maybe something has changed.
				updateEndDate();
			}

			self.duration = ko.pureComputed<string>({
				read: () => {
					const value = dispatch.NetWorkMinutes();
					const duration = moment.duration(value, "minutes");
					return duration.toISOString();
				},
				write: (value) => {
					const duration = moment.duration(value);
					dispatch.NetWorkMinutes(duration.asMinutes());
				}
			});
			self.subscriptions.push(self.duration);

			self.subscriptions.push((this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch).NetWorkMinutes.subscribe(value => {
				const currentNetWorkMinutes = calculateCurrentDurationInMinutes();
				if (value != currentNetWorkMinutes && currentNetWorkMinutes > 0) {
					updateEndDate();
				}
			}));

			self.subscriptions.push((this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch).Date.subscribe(updateNetWorkMinutes));
			self.subscriptions.push((this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch).EndDate.subscribe(updateNetWorkMinutes));
			self.subscriptions.push(self.assignedResources.subscribe(updateEndDate));

			window.ko.validation.addRule(self.duration, {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("NetWorkMinutes")),
				//@ts-ignore
				validator: (value) => {
					if (value) {
						const duration = window.moment.duration(value, "minutes");
						if (duration.isValid() && duration.asMilliseconds() > 0) {
							return true;
						}
					}
					return false;
				},
				propertyName: "NetWorkMinutes"
			});

			const originalDuration = moment.duration(this.eventRecord.OriginalData.NetWorkMinutes, "minutes");
			window.ko.validation.addRule(self.duration, {
				message: window.Helper.String.getTranslatedString("RuleViolation.Less")
					.replace("{0}", window.Helper.String.getTranslatedString("NetWorkMinutes"))
					.replace("{1}", window.Helper.String.getTranslatedString("MaximumDuration")),
				//@ts-ignore
				validator: (value) => {
					if (value) {
						const duration = window.moment.duration(value, "minutes");
						if (duration.asMinutes() <= originalDuration.asMinutes() || !self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchForceMaximumDuration ||
							duration.asMinutes() <= self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration) {
							return true;
						}
					}
					return false;
				},
				onlyIf: () => self.parentViewModel.profile().ClientConfig.ServiceOrderDispatchMaximumDuration && moment.duration(self.duration()).asMinutes() > originalDuration.asMinutes(),
				propertyName: "NetWorkMinutes"
			});

			window.ko.validation.addRule(this.assignedResources, {
				message: window.Helper.String.getTranslatedString("MissingSkillsOrAssetsValidation"),
				//@ts-ignore
				validator: () => {
					if (isDispatch(self.eventRecord)) {
						if (!self.assignedResources().some(a => self.techniciansMap.has(a.Id))) return true;

						const teamLeader = window.Helper.Scheduler.determineTeamLeader(self.eventRecord, self.assignedResources().map(it => ({
							resourceId: it.Id,
							resource: self.techniciansMap.get(it.Id)
						})));
						const mainResource = self.techniciansMap.get(teamLeader);

						const endDate = self.dispatch().EndDate();
						return window.Helper.Scheduler.hasSkillsForOrder(mainResource, self.eventRecord.ServiceOrder.OriginalData, self.dispatch().Date(), endDate)
							&& window.Helper.Scheduler.hasAssetsForOrder(mainResource, self.eventRecord.ServiceOrder.OriginalData, self.dispatch().Date(), endDate);
					}
				},
				onlyIf: () => self.assignedResources().length > 0,
				propertyName: "assignedResources"
			});
		} else if (isAbsence(this.eventRecord)) {
			this.type = "Absence";
			let dispatch = this.dispatch();
			window.ko.validation.addRule(dispatch.From, {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("From")),
				//@ts-ignore
				validator: (value) => !!value,
				propertyName: "From"
			});
			window.ko.validation.addRule(dispatch.To, {
				message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("To")),
				//@ts-ignore
				validator: (value) => !!value,
				propertyName: "To"
			});
			window.ko.validation.addRule(dispatch.To, {
				message: `${window.Helper.String.getTranslatedString("RuleViolation.Greater").replace("{0}", window.Helper.String.getTranslatedString("To")).replace("{1}", window.Helper.String.getTranslatedString("From"))}`,
				//@ts-ignore
				validator: (val: Date) => val > dispatch.From(),
				onlyIf: () => dispatch.From() && dispatch.To(),
				propertyName: "To"
			});

			if (this.dispatch().innerInstance instanceof Sms.Scheduler.Rest.Model.SmsScheduler_Absence) {
				window.ko.validation.addRule(this.dispatch().TimeEntryTypeKey, {
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("TimeEntryTypeKey")),
					//@ts-expect-error
					validator: (value) => !!value,
					propertyName: "TimeEntryTypeKey"
				});
			} else if (this.dispatch().innerInstance instanceof Crm.Article.Rest.Model.CrmArticle_ArticleDowntime) {
				window.ko.validation.addRule(this.dispatch().DowntimeReasonKey, {
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}", window.Helper.String.getTranslatedString("ArticleDowntimeReason")),
					//@ts-expect-error
					validator: (value) => !!value,
					propertyName: "DowntimeReasonKey"
				});
			}
		}
		this.assignedResources.subscribe(newValue => {
			if (newValue.length > 1 && !this.canSelectMultipleTechnicians()) {
				this.assignedResources([newValue[newValue.length - 1]]);
			}
		});
	}

	mapToolForSelect2Display(item: Crm.Article.Rest.Model.CrmArticle_Article) {
		return {
			id: item.Id,
			item: item,
			text: item.ItemNo + " - " + item.Description
		}
	}


	jobFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		let dispatch = this.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
		query = query.filter(function (it) {
			return it.OrderId === this.orderId;
		}, {orderId: dispatch.OrderId()});
		if (term) {
			query = query.filter("it.PosNo.contains(this.term) || " +
				"it.ItemNo.contains(this.term) || " +
				"it.ItemNo.contains(this.term) || " +
				"it.Description.contains(this.term) || " +
				"it.Installation.InstallationNo.contains(this.term) || " +
				"it.Installation.Description.contains(this.term)", {term: term});
		}
		return query
			.orderBy("it.PosNo")
			.orderBy("it.Description");
	}

	getUsersByIds(query: $data.Queryable<Main.Rest.Model.Main_User>, ids: string[]): $data.Queryable<Main.Rest.Model.Main_User> {
		return query.filter(function (it) {
			return it.Id in this.ids;
		},
			{ ids: ids });
	};

	resourceFilter(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string): $data.Queryable<Main.Rest.Model.Main_User> {
		query = query.filter("t => t.Id in profileResources", {profileResources: this.availableResources.map(r => r.id)});
		if (term) {
			query = query.filter("it.Id.indexOf(this.term) !== -1", {term: term});
		}
		return query
			.orderBy("it.LastName")
			.orderBy("it.FirstName");
	}

	toolsFilter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		query = query.filter("t => t.Id in profileResources", {profileResources: this.availableTools.map(r => r.id)});
		if (term) {
			query = query.filter("it.ItemNo.contains(this.term) || it.Description.contains(this.term)",	{ term: term });
		}
		return query
			.orderBy("it.ItemNo");
	}

	onArticleSelected() {
		this.checkArticleUserRelationshipValidation()
	}

	checkArticleUserRelationshipValidation() {
		const self = this;
		const assignedTools = self.parentViewModel.scheduler().resourceStore.records.filter(r => self.assignedTools().map(r => r.Id).includes(r.id as string)) as Article[];
		const assignedUsers = self.parentViewModel.scheduler().resourceStore.records.filter(r => r instanceof Technician && self.assignedResources().map(r => r.Id).includes(r.OriginalData.Id)) as Technician[];

		if (assignedUsers.length === 0) {
			return;
		}

		let dispatch = self.dispatch() as Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch;
		const endDate = dispatch.EndDate();
		let conflictingRelationships: Crm.Article.Rest.Model.CrmArticle_ArticleUserRelationship[] = [];
		assignedTools.forEach(v => {
			if (v.AssignedUsers) {
				conflictingRelationships = conflictingRelationships.concat(v.AssignedUsers.filter(au => 
					!self.assignedResources().map(r => r.Id).includes(au.UserKey) && 
					dispatch.Date() < DateHelper.endOf(au.To) && 
					DateHelper.startOf(au.From) < endDate
				));
			}
		});
		let errorItems: string = "";
		for (const conflict of conflictingRelationships) {
			errorItems += StringHelper.xss`<p>${window.Helper.getTranslatedString('OverlappingByArticleUserRelationship')
				.replace("{0}", conflict.Article.ItemNo + ' - ' + conflict.Article.Description)
				.replace("{1}", window.Globalize.formatDate(conflict.From, {date: "short"}))
				.replace("{2}", window.Globalize.formatDate(conflict.To, {date: "short"}))
				.replace("{3}", window.Helper.User.getDisplayName(conflict.User))}</p>`;
		}
		if (conflictingRelationships.length > 0) {
			self.articleUserRelationshipValidationText(`
			<div id="articleRelationships-validations" class="alert alert-warning" role="alert">
				<h4>${window.Helper.getTranslatedString('Warning')}</h4>
				${errorItems}
			</div>`);
		} else {
			self.articleUserRelationshipValidationText('');
		}
	}

	async save(data, event): Promise<void> {
		$(event.target).closest(".modal-content").data("keepchanges", true);

		try {
			this.loading(true);
			this.errors = ko.validation.group([this.dispatch, this.assignedResources, this.assignedTools, this.duration], {deep: true});
			if (this.errors().length > 0) {
				this.loading(false);
				this.errors.showAllMessages();
				this.errors.scrollToError();
				return;
			}
			let dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch | Sms.Scheduler.Rest.Model.SmsScheduler_Absence | Crm.Article.Rest.Model.CrmArticle_ArticleDowntime;
			if (this.dispatch().innerInstance instanceof Sms.Scheduler.Rest.Model.SmsScheduler_Absence) {
				dispatch = this.dispatch().innerInstance as Sms.Scheduler.Rest.Model.SmsScheduler_Absence;
				this.eventRecord.setStartDate(dispatch.From, false);
				this.eventRecord.setEndDate(dispatch.To, false);
				this.eventRecord.set({
					Description: dispatch.Description,
					TimeEntryTypeKey: dispatch.TimeEntryTypeKey
				});
			} else if (this.dispatch().innerInstance instanceof Crm.Article.Rest.Model.CrmArticle_ArticleDowntime) {
				dispatch = this.dispatch().innerInstance as Crm.Article.Rest.Model.CrmArticle_ArticleDowntime;
				this.eventRecord.setStartDate(dispatch.From, false);
				this.eventRecord.setEndDate(dispatch.To, false);
				this.eventRecord.set({
					Description: dispatch.Description,
					DowntimeReasonKey: dispatch.DowntimeReasonKey
				});
			} else if (isDispatch(this.eventRecord)) {
				const eventRecord = this.eventRecord;
				if (this.manuallyScheduled != this.eventRecord.manuallyScheduled) {
					this.eventRecord.manuallyScheduled = this.manuallyScheduled;
				}


				let dispatch = this.dispatch().innerInstance as Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch;

				this.eventRecord.setStartDate(new Date(dispatch.Date.getFullYear(), dispatch.Date.getMonth(), dispatch.Date.getUTCDate(), dispatch.Date.getHours(), dispatch.Date.getMinutes()));
				this.eventRecord.setEndDate(new Date(dispatch.EndDate.getFullYear(), dispatch.EndDate.getMonth(), dispatch.EndDate.getUTCDate(), dispatch.EndDate.getHours(), dispatch.EndDate.getMinutes()));
				if (this.isReleased() && dispatch.StatusKey != "Released") {
					dispatch.StatusKey = "Released";
				} else if (!this.isReleased() && dispatch.StatusKey != "Scheduled") {
					dispatch.StatusKey = "Scheduled";
				}

				let assignedIds = this.assignedResources().map(x => x.Id).concat(this.assignedTools().map(x => x.Id));
				let initialAssignedIds = this.initialAssignedResources().map(x => x.Id).concat(this.initialAssignedTools().map(x => x.Id));
				let added = window._.difference(assignedIds, initialAssignedIds);
				let removed = window._.difference(initialAssignedIds, assignedIds);

				let addedAssignments = added.map(resourceId => {
					let assignment = window.Helper.String.isGuid(resourceId) ? window.database.SmsScheduler_DispatchArticleAssignment.defaultType.create() : window.database.SmsScheduler_DispatchPersonAssignment.defaultType.create();
					assignment.Id = window.$data.createGuid().toString().toLowerCase();
					assignment.DispatchKey = dispatch.Id;
					assignment.ResourceKey = resourceId;
					return new Assignment(assignment);
				});

				const removedJobs = eventRecord.ServiceOrderTimeDispatches.filter(x => !this.assignedJobs().some(y => y.Id === x.ServiceOrderTimeId));
				removedJobs.forEach(removedJob => window.database.remove(removedJob));
				const newServiceOrderTimeDispatches = window._.difference(eventRecord.ServiceOrderTimeDispatches, removedJobs);

				const newJobs = this.assignedJobs().filter(x => !eventRecord.ServiceOrderTimeDispatches.some(y => y.ServiceOrderTimeId === x.Id));
				for (const missingJob of newJobs) {
					const serviceOrderTimeDispatch = window.database.CrmService_ServiceOrderTimeDispatch.defaultType.create();
					serviceOrderTimeDispatch.Id = window.$data.createGuid().toString().toLowerCase();
					serviceOrderTimeDispatch.ServiceOrderDispatchId = dispatch.Id;
					serviceOrderTimeDispatch.ServiceOrderTimeId = missingJob.Id;
					newServiceOrderTimeDispatches.push(serviceOrderTimeDispatch);
				}

				this.parentViewModel.scheduler().project.assignmentStore.add(addedAssignments);
				this.parentViewModel.scheduler().project.assignmentStore.remove(this.assignments.filter(it => removed.includes(it.resourceId as string)));

				const remainingAssignments = this.parentViewModel.scheduler().project.assignmentStore.getAssignmentsForEvent(this.eventRecord as TimeSpan) as Assignment[];
				const newTeamLeader = window.Helper.Scheduler.determineTeamLeader(this.eventRecord, remainingAssignments, this.techniciansMap);
				if (added.length !== 0) {
					this.handleResourceAssignments(added, newTeamLeader, dispatch);
				}

				if (removed.length !== 0) {
					for (const resourceId of removed) {
						if (window.Helper.String.isGuid(resourceId)) {
							const articleUserRelationships = await window.database.CrmArticle_ArticleUserRelationship.filter(function (it) {
								return it.ArticleKey == this.id && it.UserKey == this.userKey;
							}, {
								id: resourceId,
								userKey: newTeamLeader
							}).toArray();

							if (articleUserRelationships?.length > 0) {
								for (const relationship of articleUserRelationships) {
									window.database.remove(relationship);
								}
							}
						}
					}
				}
				const eventData = {
					IsFixed: dispatch.IsFixed,
					ServiceOrderTimeDispatches: newServiceOrderTimeDispatches,
					StatusKey: dispatch.StatusKey,
					Username: newTeamLeader,
					Remark: dispatch.Remark,
					InfoForTechnician: dispatch.InfoForTechnician
				};
				
				this.extendEventData(eventData, dispatch);
				
				this.eventRecord.set(eventData);
			}

			queueMicrotask(() => {
				$(".modal:visible").modal("hide");
			});
		} catch (e) {
			window.Log.error((e as Error));
			window.swal(window.Helper.String.getTranslatedString("UnknownError"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
		} finally {
			this.loading(false);
			this.parentViewModel.scheduler().renderContents();
		}
	}

	protected handleResourceAssignments(added: string[], newTeamLeader: string, dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		for (const resourceId of added) {
			if (window.Helper.String.isGuid(resourceId)) {
				const articleUserRelationship = window.database.CrmArticle_ArticleUserRelationship.defaultType.create();
				articleUserRelationship.UserKey = newTeamLeader;
				articleUserRelationship.ArticleKey = resourceId;
				articleUserRelationship.From = dispatch.Date;
				articleUserRelationship.To = dispatch.EndDate;
				window.database.add(articleUserRelationship);
			}
		}
	}

	protected extendEventData(eventData: any, dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		// Base implementation - extensions can override to add additional data
	}

	cancel(data, event): void {
		$(event.target).closest(".modal-content").data("keepchanges", true);
		$(".modal:visible").modal("hide");
	}

	dispose() {
		this.subscriptions.forEach(s => s.dispose());
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerEditModalViewModel = SchedulerEditModalViewModel;