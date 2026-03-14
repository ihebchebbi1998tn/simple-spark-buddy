import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsViewModel} from "./ServiceOrderDetailsViewModel";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import 'moment-duration-format';
import moment from "moment";
import {HelperDatabase} from "@Main/helper/Helper.Database";

export class AdditionalTimePostingsValidatorError {
	username: string;
	message: string;
}

export class ServiceOrderTimePostingEditModalViewModel extends window.Main.ViewModels.ViewModelBase {
	isAllDay: KnockoutComputed<boolean>;
	articleAutocomplete = ko.observable<string>("");
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	enableUserGroupSelection = ko.observable<boolean>(true);
	initialServiceOrderTimeId = ko.observable<string>(null);
	minDate: KnockoutComputed<Date | false>;
	maxDate = ko.observable<Date>(window.moment().startOf("day").add(24, "hours").add(1, "millisecond").toDate());
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;
	serviceOrderTime = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime>(null);
	serviceOrderTimePosting = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>(null);
	showDispatchSelection = ko.observable<boolean>(false);
	showTechnicianSelection = ko.observable<boolean>(window.Crm.Service.Settings.ServiceOrderTimePosting.ShowTechnicianSelection);
	multipleTechnicianSelection = ko.observable<boolean>(window.Crm.Service.Settings.ServiceOrderTimePosting.MultipleTechnicianSelection);
	showUserGroupAndRoleSelection = ko.observable<boolean>(window.Crm.Service.Settings.ServiceOrderTimePosting.ShowUserGroupAndRoleSelection);
	selectedTravelType = ko.observable<Crm.PerDiem.Rest.Model.Lookups.ObservableCrmPerDiem_TravelType>(null);
	customLicensePlate = ko.observable<boolean>(false);
	selectedVehicleId = ko.observable<string>(null);
	selectedUsernames = ko.observableArray<string>([]);
	additionalTimePostings = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimePosting>([]);
	validItemNosAfterCustomerSignature = ko.observableArray<string>([]);
	parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel;
	errors = ko.validation.group(this.serviceOrderTimePosting, {deep: true});
	showKilometerSelection = ko.observable<boolean>(false);
	isNewServiceOrderExpensePosting = ko.observable<boolean>(true)
	prePlanned = ko.observable<boolean>(false);
	invalidPostings = ko.observable<boolean>(false);
	userGroupId = ko.observable<string>(null);
	roleId = ko.observable<string>(null);
	additionalTimePostingsValidatorErrors = ko.observableArray<AdditionalTimePostingsValidatorError>([]);
	showDriverTypeSelection = ko.pureComputed<boolean>(() => {
		return this.showKilometerSelection() && this.selectedTravelType() ? this.selectedTravelType().ShowDriverType() : false
	});
	showLicensePlateSelection = ko.pureComputed<boolean>(() => {
		return this.showKilometerSelection() && this.selectedTravelType() ? this.selectedTravelType().ShowLicensePlate() : false
	});
	getDurationLabel = ko.pureComputed<string>(() => {
		let durationLabel = window.Helper.String.getTranslatedString("Duration");
		let planned = null;
		if (this.prePlanned()) {
			durationLabel = window.Helper.String.getTranslatedString("PlannedDuration");
		} else if (this.serviceOrderTimePosting().ParentServiceOrderTimePosting() && this.serviceOrderTimePosting().ParentServiceOrderTimePosting().ServiceOrderTimePostingType() === window.Crm.Service.ServiceOrderTimePostingType.Preplanned) {
			planned = this.serviceOrderTimePosting().ParentServiceOrderTimePosting().Duration();
		} else if (this.serviceOrderTime() && this.serviceOrderTime().EstimatedDuration()) {
			planned = this.serviceOrderTime().EstimatedDuration();
		}
		if (planned) {
			const duration = window.moment.duration(planned);
			const durationText = duration.isValid() ? duration.format("hh:mm", {stopTrim: "h"}) : "";
			durationLabel += " (" + window.Helper.String.getTranslatedString('L_EstimatedDuration') + ": " + durationText + " " + window.Helper.String.getTranslatedString("HourAbbreviation") + ")";
		}
		return durationLabel;
	});
	lookups: LookupType = {
		installationHeadStatuses: {$tableName: "CrmService_InstallationHeadStatus"},
		serviceOrderTypes: {$tableName: "CrmService_ServiceOrderType"},
		servicePriorities: { $tableName: "CrmService_ServicePriority" },
		travelType: { $tableName: "CrmPerDiem_TravelType" },
		driverType: { $tableName: "CrmPerDiem_DriverType" },
		distanceUnits: { $tableName: "CrmPerDiem_DistanceUnit" }

	}

	constructor(parentViewModel: ServiceOrderDetailsViewModel | DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
		this.minDate = ko.pureComputed(() => {
			if (window.AuthorizationManager.isAuthorizedForAction("ServiceOrderTimePosting", "NoMinDateLimit")) {
				return false;
			}
			return window.Crm.Service.Settings.ServiceOrderTimePosting.MaxDaysAgo ? window.moment().startOf("day").add(-parseInt(window.Crm.Service.Settings.ServiceOrderTimePosting.MaxDaysAgo), "days").toDate() : false
		});
		this.dispatch = parentViewModel && parentViewModel.dispatch || window.ko.observable(null);
		this.serviceOrder = parentViewModel && parentViewModel.serviceOrder || window.ko.observable(null);
		
		this.showKilometerSelection.subscribe(showKilometerSelection => {
			if (showKilometerSelection === false && this.serviceOrderTimePosting()) {
				this.serviceOrderTimePosting().TravelTypeKey(null);
				this.serviceOrderTimePosting().DriverTypeKey(null);
				this.serviceOrderTimePosting().Distance(null);
				this.serviceOrderTimePosting().DistanceUnitKey(window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.distanceUnits, this.serviceOrderTimePosting().DistanceUnitKey()));
				this.serviceOrderTimePosting().LicensePlate(null);
				this.serviceOrderTimePosting().LocationFrom(null);
				this.serviceOrderTimePosting().LocationTo(null);

			}
		});
	}

	async init(id?: string, params?: { [key: string]: string }): Promise<void> {
		await super.init(id, params);
		if (params.fromJobs) {
			const prePlanned = window.Helper.ServiceOrder.canEditEstimatedQuantitiesSync(this.serviceOrder().StatusKey(), this.parentViewModel.lookups.serviceOrderStatuses as {
				[key: string]: Crm.Service.Rest.Model.Lookups.CrmService_ServiceOrderStatus
			});
			this.prePlanned(prePlanned);
		} else {
			this.prePlanned(!!params.prePlanned);
		}
		let user = await window.Helper.User.getCurrentUser()
		this.currentUser(user);
		let serviceOrderTimePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting;
		if (id) {
			serviceOrderTimePosting = await window.database.CrmService_ServiceOrderTimePosting
				.include("ParentServiceOrderTimePosting")
				.include("ServiceOrderTime")
				.include("ServiceOrderDispatch")
				.find(id);
			if (serviceOrderTimePosting.Username) {
				this.selectedUsernames.push(serviceOrderTimePosting.Username);
			}
			this.isNewServiceOrderExpensePosting(false);
		} else {
			this.isNewServiceOrderExpensePosting(true);
			this.serviceOrderTime(this.dispatch() ? this.dispatch().CurrentServiceOrderTime() : null);
			serviceOrderTimePosting = window.database.CrmService_ServiceOrderTimePosting.defaultType.create();
			serviceOrderTimePosting.ArticleId = params.articleId || (this.serviceOrderTime() ? this.serviceOrderTime().ArticleId() : null);
			serviceOrderTimePosting.DispatchId = params.dispatchId ?? this.dispatch()?.Id() ?? null;
			serviceOrderTimePosting.OrderId = this.serviceOrder() ? this.serviceOrder().Id() : null;
			serviceOrderTimePosting.ServiceOrderTimeId = params.serviceOrderTimeId || (this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null) || null
			serviceOrderTimePosting.ServiceOrderTimePostingType = this.prePlanned() ? window.Crm.Service.ServiceOrderTimePostingType.Preplanned : window.Crm.Service.ServiceOrderTimePostingType.Used;
			if (!this.prePlanned()) {
				serviceOrderTimePosting.Username = this.currentUser().Id;
				this.selectedUsernames.push(serviceOrderTimePosting.Username);
				const latestTimeEntry = await window.Helper.TimeEntry.getLatestTimeEntry(serviceOrderTimePosting.Username, window.moment(params.from || new Date()).utc(true).toDate());
				serviceOrderTimePosting.From = params.from ? window.moment(params.from).toDate() : window.Helper.TimeEntry.getTimeEntryToOrDefault(latestTimeEntry, window.moment(params.from || new Date()).utc(true).toDate());
				serviceOrderTimePosting.TimeZoneFrom = latestTimeEntry ? latestTimeEntry.TimeZoneTo : Helper.Date.getTimeZoneForUser(this.currentUser());
				serviceOrderTimePosting.TimeZoneTo = serviceOrderTimePosting.TimeZoneFrom;
				serviceOrderTimePosting.To = params.to ? window.moment(params.to).toDate() : null;
			}
		}

		if (id) {
			window.database.attachOrGet(serviceOrderTimePosting);
		} else {
			window.database.add(serviceOrderTimePosting)
		}

		if (!this.prePlanned() && window.Helper.ServiceOrderTimePosting.isPrePlanned(serviceOrderTimePosting)) {
			window.database.detach(serviceOrderTimePosting);
			let newServiceOrderTimePosting = window.Helper.Database.createClone(serviceOrderTimePosting);
			if (params.dispatchId) {
				newServiceOrderTimePosting.DispatchId = params.dispatchId;
			}
			newServiceOrderTimePosting.Id = window.$data.createGuid().toString().toLowerCase();
			newServiceOrderTimePosting.DispatchId = params.dispatchId ?? this.dispatch()?.Id() ?? null;
			newServiceOrderTimePosting.Username = this.currentUser().Id;
			this.selectedUsernames.push(newServiceOrderTimePosting.Username);
			const latestTimeEntry = await window.Helper.TimeEntry.getLatestTimeEntry(newServiceOrderTimePosting.Username, window.moment(params.from || new Date()).utc(true).toDate());
			newServiceOrderTimePosting.From = params.from ? window.moment(params.from).toDate() : window.Helper.TimeEntry.getTimeEntryToOrDefault(latestTimeEntry, window.moment(params.from || new Date()).utc().toDate());
			newServiceOrderTimePosting.ParentServiceOrderTimePostingId = serviceOrderTimePosting.Id;
			newServiceOrderTimePosting.ParentServiceOrderTimePostingVersion = serviceOrderTimePosting.Version;
			newServiceOrderTimePosting.ServiceOrderTimePostingType = window.Crm.Service.ServiceOrderTimePostingType.Used;
			newServiceOrderTimePosting.TimeZoneFrom = latestTimeEntry ? latestTimeEntry.TimeZoneTo : Helper.Date.getTimeZoneForUser(this.currentUser());
			newServiceOrderTimePosting.TimeZoneTo = newServiceOrderTimePosting.TimeZoneFrom;
			if (params.to) {
				newServiceOrderTimePosting.To = window.moment(params.to).toDate();
				const duration = moment.duration(newServiceOrderTimePosting.To.getTime() - newServiceOrderTimePosting.From.getTime());
				newServiceOrderTimePosting.Duration = duration.toString();
			} else if (serviceOrderTimePosting.Duration && serviceOrderTimePosting.Duration !== "PT0") {
				newServiceOrderTimePosting.Duration = serviceOrderTimePosting.Duration;
				newServiceOrderTimePosting.To = window.moment(newServiceOrderTimePosting.From).add(window.moment.duration(serviceOrderTimePosting.Duration)).toDate();
			} else {
				newServiceOrderTimePosting.Duration = "PT0";
				newServiceOrderTimePosting.To = null;
			}
			window.database.add(newServiceOrderTimePosting);
			serviceOrderTimePosting = newServiceOrderTimePosting;
		}

		this.isAllDay = ko.pureComputed(() => {
			let fromMoment = moment(this.serviceOrderTimePosting().From());
			let toMoment = moment(this.serviceOrderTimePosting().To());

			return fromMoment.dayOfYear() != toMoment.dayOfYear() && fromMoment.hour() == 0 && fromMoment.minute() == 0 &&
				toMoment.hour() == 0 && toMoment.minute() == 0;
		});

		this.serviceOrderTimePosting(serviceOrderTimePosting.asKoObservable());
		if (!this.serviceOrderTime()) {
			this.serviceOrderTime(this.serviceOrderTimePosting().ServiceOrderTime() || (this.dispatch() ? this.dispatch().CurrentServiceOrderTime() : null));
		}
		this.serviceOrderTimePosting().To.subscribe(() => {
			this.serviceOrderTimePosting().From.valueHasMutated();
		});
		this.serviceOrderTimePosting().Username.subscribe(() => {
			this.serviceOrderTimePosting().From.valueHasMutated();
		});
		this.serviceOrderTimePosting().From.subscribe(from => Helper.TimeEntry.updateToAndDuration(this.serviceOrderTimePosting(), from));
		this.serviceOrderTimePosting().To.subscribe(to => Helper.TimeEntry.updateFromAndDuration(this.serviceOrderTimePosting(), to));
		this.serviceOrderTimePosting().Duration.subscribe(duration => Helper.TimeEntry.initFromAndTo(this.serviceOrderTimePosting(), duration));
		this.serviceOrderTimePosting().TimeZoneFrom.subscribe(tz => Helper.TimeEntry.updateDuration(this.serviceOrderTimePosting()));
		this.serviceOrderTimePosting().TimeZoneTo.subscribe(tz => Helper.TimeEntry.updateDuration(this.serviceOrderTimePosting()));
		this.serviceOrderTimePosting().Username.subscribe(username => this.onUserSelect(username));
		if (this.serviceOrderTimePosting().DispatchId() === null && this.serviceOrderTimePosting().OrderId() === null) {
			this.showDispatchSelection(true);
			this.serviceOrderTimePosting().DispatchId.extend({
				required: {
					params: true,
					message: window.Helper.String.getTranslatedString("RuleViolation.Required").replace("{0}",
						window.Helper.String.getTranslatedString("ServiceOrderDispatch"))
				}
			});
		}
		this.initialServiceOrderTimeId(this.serviceOrderTimePosting().ServiceOrderTimeId());
		if (id || this.dispatch() || this.serviceOrder()) {
			await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		}
		const validItemNo = ko.pureComputed(() => {
			return !this.dispatch() ||
				!this.serviceOrderTimePosting().ItemNo() ||
				this.dispatch().StatusKey() !== "SignedByCustomer" ||
				this.validItemNosAfterCustomerSignature()
					.indexOf(this.serviceOrderTimePosting().ItemNo()) !==
				-1;
		});
		this.showKilometerSelection.subscribe(showKilometerSelection => {

			if (showKilometerSelection && !this.serviceOrderTimePosting().TravelTypeKey()) {
				this.serviceOrderTimePosting().TravelTypeKey(window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.travelType, this.serviceOrderTimePosting().TravelTypeKey()));
			}
			if (showKilometerSelection && this.showDriverTypeSelection() && !this.serviceOrderTimePosting().DriverTypeKey()) {
				this.serviceOrderTimePosting().DriverTypeKey(window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.driverType, this.serviceOrderTimePosting().DriverTypeKey()));
			}
			
		});
		this.selectedTravelType.subscribe(async newSelectedTravelType => {
			if (newSelectedTravelType && newSelectedTravelType.ShowLicensePlate() && this.serviceOrderTimePosting() && !this.serviceOrderTimePosting().LicensePlate()) {
				const userLicensePlate = this.currentUser().ExtensionValues.LicensePlate;
				let assignedVehicleIds = await window.Helper.Article.getAssignedVehicleIds(this.currentUser().Id, this.serviceOrderTimePosting().From(), this.serviceOrderTimePosting().To());
				let results = await window.database.CrmArticle_Article.filter(function (it) {
					return it.Id in this.assignedVehicleIds;
				}, {assignedVehicleIds: assignedVehicleIds})
					.filter("it.ArticleTypeKey === 'Vehicle' && it.LicensePlate !== null")
					.take(1)
					.toArray();
				if (results.length > 0) {
					this.serviceOrderTimePosting().LicensePlate(results[0].LicensePlate);
				} else if (userLicensePlate) {
					this.serviceOrderTimePosting().LicensePlate(userLicensePlate);
				}
				this.customLicensePlate(true);
			}
		});
		this.selectedUsernames.subscribe(userKeys => {
			if (userKeys.length > 0) {
				this.serviceOrderTimePosting().Username(userKeys[0]);
			}

		});
		this.serviceOrderTimePosting().ArticleId.extend({
			validation: {
				validator: function () {
					return validItemNo();
				},
				message: window.Helper.String.getTranslatedString("SelectionNotPossible")
			}
		});
		this.serviceOrderTimePosting().To.extend({
			validation: [
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeAfterDate")
						.replace("{0}", window.Helper.String.getTranslatedString("To"))
						.replace("{1}", window.Helper.String.getTranslatedString("Today")),
					onlyIf: () => !!this.serviceOrderTimePosting().To(),
					validator: (val) => moment(this.maxDate()).isAfter(val)
				},
				{
					message: window.Helper.String.getTranslatedString("RuleViolation.DateCanNotBeBeforeDate")
						.replace("{0}", window.Helper.String.getTranslatedString("To"))
						.replace("{1}", window.Helper.String.getTranslatedString("From")),
					onlyIf: () => !!this.serviceOrderTimePosting().To(),
					validator: (val) => moment(val).isAfter(this.serviceOrderTimePosting().From())
				}
			]
		});
		this.serviceOrderTimePosting().DistanceUnitKey.extend({
			required: {
				params: true,
				onlyIf: () => !!this.serviceOrderTimePosting().Distance(),
				message: window.Helper.String.getTranslatedString("RuleViolation.Required")
					.replace("{0}", window.Helper.String.getTranslatedString("DistanceUnit"))
			}
		});
		validItemNo.subscribe(() => this.serviceOrderTimePosting().ArticleId.valueHasMutated());
		let lastTimePostings = await window.database.CrmService_ServiceOrderTimePosting
			.include("ServiceOrderDispatch")
			.filter("it.Username === this.username", {username: this.serviceOrderTimePosting().Username()})
			.orderByDescending("it.To")
			.take(1)
			.toArray();
		const lastTimePosting = lastTimePostings.length > 0 ? lastTimePostings[0] : null;
		const preSelectDispatchFromLastTimePosting = lastTimePosting !== null &&
			lastTimePosting.ServiceOrderDispatch !== null &&
			lastTimePosting.ServiceOrderDispatch.StatusKey !== "ClosedNotComplete" &&
			lastTimePosting.ServiceOrderDispatch.StatusKey !== "ClosedComplete" &&
			this.serviceOrderTimePosting().DispatchId() === null &&
			!this.prePlanned();
		if (preSelectDispatchFromLastTimePosting) {
			this.serviceOrderTimePosting().DispatchId(lastTimePosting.DispatchId);
			this.serviceOrderTimePosting().ServiceOrderTimeId(lastTimePosting.ServiceOrderDispatch.CurrentServiceOrderTimeId || lastTimePosting.ServiceOrderTimeId);
		}
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let itemNos = await window.database.CrmArticle_Article.filter(function (it) {
			return it.ArticleTypeKey === "Service" && (it.ExtensionValues.CanBeAddedAfterCustomerSignature || it.ExtensionValues.CanBeEditedAfterCustomerSignature);
		}).map(function (it) {
			return it.ItemNo;
		}).toArray();
		this.validItemNosAfterCustomerSignature(itemNos);

		if (this.serviceOrderTimePosting().LicensePlate()) {
			return window.database.CrmArticle_Article
				.filter(function (it) {
					return it.LicensePlate === this.licensePlate;
				}, {licensePlate: this.serviceOrderTimePosting().LicensePlate()})
				.map("it.Id")
				.toArray()
				.then(ids => {
					if (ids.length > 0) {
						this.selectedVehicleId(ids[0]);
					} else {
						this.customLicensePlate(true);
					}
				});
		}
		this.serviceOrderTimePosting().DistanceUnitKey(window.Helper.Lookup.getDefaultLookupValueSingleSelect(this.lookups.distanceUnits, this.serviceOrderTimePosting().DistanceUnitKey()));

	};

	canEditArticle(): boolean {
		if (!this.serviceOrderTimePosting().ArticleId()) {
			return true;
		}
		if (window.Helper.ServiceOrderTimePosting.wasPrePlanned(this.serviceOrderTimePosting())) {
			return false;
		}
		return !this.serviceOrderTime()?.ArticleId() || this.serviceOrderTime().IsTimeLumpSum() || this.serviceOrder()?.StatusKey() === "PostProcessing";
	}

	async setAllDay() {
		let from = this.serviceOrderTimePosting().From();
		let to = this.serviceOrderTimePosting().To();

		if (!from && !to) {
			from = new Date();
			to = new Date();
		} else if (from && !to) {
			to = new Date(from);
		} else if (!from && to) {
			from = new Date(to);
		}

		from = moment(from).set({"hour": 0, "minute": 0}).toDate();
		to = moment(to).set({"hour": 0, "minute": 0}).add(24, "hour").toDate();

		this.serviceOrderTimePosting().From(from);
		this.serviceOrderTimePosting().To(to);
	}

	dispatchFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch> {
		if (term) {
			query = window.Helper.String.contains(query, term, ["ServiceOrder.OrderNo"]);
		}
		if (this.serviceOrder()) {
			query = query.filter("OrderId", "===", this.serviceOrder().Id());
		}
		if (this.serviceOrderTimePosting().Username()) {
			query = query.filter("it.Username === this.username", {username: this.serviceOrderTimePosting().Username()});
		}
		query = query.filter("it.StatusKey in this.statusKeys",
			{statusKeys: ["Released", "Read", "InProgress", "SignedByCustomer"]});
		return query;
	};

	dispose(): void {
		window.database.detach(this.serviceOrderTimePosting().innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);
		this.invalidPostings(false);
		this.additionalTimePostings().forEach((posting) => {
			window.database.detach(posting);
		})
		this.additionalTimePostings.removeAll();
		this.additionalTimePostingsValidatorErrors.removeAll();
		const username = this.serviceOrderTimePosting().Username();
		if (username && !this.selectedUsernames().find(it => it === username)) {
			window.database.detach(this.serviceOrderTimePosting().innerInstance);
		};
		const filteredUsernames = this.selectedUsernames().filter(function (it) {
			return it !== this.username;
		}, { username: username });
		for (const username of filteredUsernames) {
			//@ts-ignore
			let newTimePosting = HelperDatabase.createClone(this.serviceOrderTimePosting()) as Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting;
			newTimePosting.Id = window.$data.createGuid().toString().toLowerCase();
			newTimePosting.Username = username;
			window.database.add(newTimePosting);
			let errors = window.ko.validation.group(newTimePosting.asKoObservable(), {deep: true});
			await errors.awaitValidation();
			if (errors().length > 0) {
				const user = await window.database.Main_User
					.find(username);
				this.additionalTimePostingsValidatorErrors().push({
					username: Helper.User.getDisplayName(user),
					message: errors()[0]
				})
				window.database.detach(newTimePosting);
			} else {
				this.additionalTimePostings.push(newTimePosting.asKoObservable());
			}
		}
		;
		if (this.errors().length > 0 || this.additionalTimePostingsValidatorErrors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			if (this.additionalTimePostingsValidatorErrors().length > 0) {
				this.invalidPostings(true);
			}
			return;
		}
		try {
			if (!this.prePlanned() && this.dispatch() && ["Released", "Read"].indexOf(this.dispatch().StatusKey()) !== -1) {
				await this.workOnDispatch();
			}
			if (window.Helper.ServiceOrderTimePosting.isPrePlanned(this.serviceOrderTimePosting())) {
				await this.updateEstimatedDuration();
			}
			await window.database.saveChanges();
			this.loading(false);
			if (this.isNewServiceOrderExpensePosting()) {
				this.showSnackbar(window.Helper.String.getTranslatedString("Added"));
			}
			else {
				this.showSnackbar(window.Helper.String.getTranslatedString("Modified"));
			}
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	getArticleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article> {
		query = query.filter(function (it) {
			return it.ArticleTypeKey === "Service" && it.ExtensionValues.IsHidden === false;
		});
		if (this.dispatch() && ["SignedByCustomer", "ClosedNotComplete", "ClosedComplete"].indexOf(this.dispatch().StatusKey()) !== -1) {
			query = query.filter(function (it) {
				return it.ExtensionValues.CanBeAddedAfterCustomerSignature;
			});
		}
		return window.Helper.Article.getArticleAutocompleteFilter(query, term, this.currentUser().DefaultLanguageKey);
	};

	getServiceOrderTimeAutocompleteDisplay(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		return window.Helper.ServiceOrderTime.getAutocompleteDisplay(serviceOrderTime,
			this.dispatch() ? this.dispatch().CurrentServiceOrderTimeId() : null);
	};

	getServiceOrderTimeAutocompleteFilter(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>, term: string): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = query.filter(function (it) {
				return it.OrderId === this.orderId;
			},
			{orderId: this.serviceOrderTimePosting().OrderId()});

		if (term) {
			query = window.Helper.String.contains(query, term, ["Description.toLowerCase()", "ItemNo.toLowerCase()", "PosNo.toLowerCase()"]);
		}
		return query;
	};

	onArticleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		this.showKilometerSelection(article !== null && article.ExtensionValues.ShowDistanceInput);
		if (article) {
			this.serviceOrderTimePosting().ArticleId(article.Id);
			this.serviceOrderTimePosting().ItemNo(article.ItemNo);
		} else {
			this.serviceOrderTimePosting().ArticleId(null);
			this.serviceOrderTimePosting().ItemNo(null);
		}
	};

	onDispatchSelect(dispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch): void {
		if (dispatch) {
			this.dispatch(dispatch.asKoObservable());
			this.serviceOrder(dispatch.ServiceOrder.asKoObservable());
			if (this.serviceOrderTimePosting().DispatchId() !== dispatch.Id) {
				this.serviceOrderTimePosting().DispatchId(dispatch.Id);
				this.serviceOrderTimePosting().ServiceOrderTimeId(null);
			}
			this.serviceOrderTimePosting().OrderId(dispatch.OrderId);
			this.serviceOrderTimePosting().Username(dispatch.Username);
		} else {
			this.serviceOrder(null);
			this.dispatch(null);
			this.serviceOrderTimePosting().DispatchId(null);
			this.serviceOrderTimePosting().OrderId(null);
			this.serviceOrderTimePosting().ServiceOrderTimeId(null);
		}
	};

	onUserSelect(user: string): void {
		if (user) {
			const date = this.serviceOrderTimePosting().To() || new Date();
			window.Helper.TimeEntry.getLatestTimeEntryToOrDefault(user, date).then(latestTo => {
				this.serviceOrderTimePosting().From(latestTo);
				const defaultDuration = this.serviceOrderTimePosting().ParentServiceOrderTimePosting() ? this.serviceOrderTimePosting().ParentServiceOrderTimePosting().Duration() : "P0M";
				this.serviceOrderTimePosting().Duration(defaultDuration);
				this.errors.showAllMessages(false);
			});
		}
	};

	onJobSelected(serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): void {
		if (serviceOrderTime === null) {
			this.serviceOrderTime(null);
			this.serviceOrderTimePosting().ServiceOrderTime(null);
			this.serviceOrderTimePosting().ServiceOrderTimeId(null);
		} else {
			this.serviceOrderTime(serviceOrderTime.asKoObservable());
			if (this.serviceOrderTime().ArticleId()) {
				this.serviceOrderTimePosting().ArticleId(this.serviceOrderTime().ArticleId());
			}
		}
	};

	onSelectTravelType(travelType: Crm.PerDiem.Rest.Model.Lookups.CrmPerDiem_TravelType): void {
		this.selectedTravelType(travelType !== null ? travelType.asKoObservable() : null);
	}

	isJobEditable(): boolean {
		return this.serviceOrderTimePosting().OrderId() &&
			(!window.Helper.ServiceOrderTimePosting.wasPrePlanned(this.serviceOrderTimePosting()) || window.AuthorizationManager.currentUserHasPermission("ServiceOrder::TimePostingPrePlannedEditJob")) &&
			(this.dispatch() == null || !Crm.Service.Settings.Dispatch.RestrictEditingToActiveJob);
	};

	async updateEstimatedDuration(): Promise<void> {
		let updateInitialServiceOrderTime = !!this.initialServiceOrderTimeId() && this.initialServiceOrderTimeId() !== this.serviceOrderTimePosting().ServiceOrderTimeId() && (this.serviceOrderTimePosting().Duration.isModified() || this.serviceOrderTimePosting().Duration() !== null);
		if (updateInitialServiceOrderTime) {
			await window.Helper.ServiceOrderTime.calculateEstimatedDuration(this.initialServiceOrderTimeId(), this.serviceOrderTimePosting().Id(), null);
		}
		let updateServiceOrderTime = this.serviceOrderTimePosting().Duration.isModified() && !!this.serviceOrderTimePosting().ServiceOrderTimeId();
		if (updateServiceOrderTime) {
			await window.Helper.ServiceOrderTime.calculateEstimatedDuration(this.serviceOrderTimePosting().ServiceOrderTimeId(), this.serviceOrderTimePosting().Id(), this.serviceOrderTimePosting().Duration());
		}
	}

	async getVehicleSelect2Filter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string): Promise<$data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>> {
		let assignedVehicleIds = await window.Helper.Article.getAssignedVehicleIds(this.serviceOrderTimePosting().Username(), this.serviceOrderTimePosting().From(), this.serviceOrderTimePosting().To());
		query = query.filter(function (it) {
			return it.Id in this.assignedVehicleIds;
		}, {assignedVehicleIds: assignedVehicleIds});
		query = query.filter("it.ArticleTypeKey === 'Vehicle' && it.LicensePlate !== null");
		if (term) {
			query = window.Helper.String.contains(query, term, ["LicensePlate.toLowerCase()"]);
		}

		return query;
	};

	toggleCustomLicensePlate() {
		this.customLicensePlate(!this.customLicensePlate());
		if (this.customLicensePlate()) {
			this.selectedVehicleId(null);
		}
		this.serviceOrderTimePosting().LicensePlate(null);
	}

	async onVehicleResult(result) {
		if (this.currentUser().ExtensionValues.LicensePlate) {
			result.unshift({
				Id: window.Helper.String.emptyGuid(),
				LicensePlate: this.currentUser().ExtensionValues.LicensePlate
			});
		}
		return result;
	};

	onVehicleSelect(article: Crm.Article.Rest.Model.CrmArticle_Article): void {
		if (article) {
			this.serviceOrderTimePosting().LicensePlate(article.LicensePlate);
			this.customLicensePlate(null);
		} else {
			this.serviceOrderTimePosting().LicensePlate(null);
		}
	};

	workOnDispatch = window.Crm.Service.ViewModels.DispatchDetailsViewModel.prototype.workOnDispatch;
}

namespace("Crm.Service.ViewModels").ServiceOrderTimePostingEditModalViewModel = ServiceOrderTimePostingEditModalViewModel;