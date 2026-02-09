import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";
import {namespace} from "@Main/namespace";
import type {ServiceOrderDetailsJobsTabItemViewModel} from "./ServiceOrderDetailsJobsTabViewModel";
import {HelperDispatch} from "@Crm.Service/helper/Helper.Dispatch";

export class DispatchDetailsJobsTabViewModel extends window.Crm.Service.ViewModels.ServiceOrderDetailsJobsTabViewModelBase<DispatchDetailsViewModel> {
	currentServiceOrderTimeId: string;
	dispatch: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>;
	serviceOrderTimeDispatches = ko.observableArray<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTimeDispatch>([]);
	timesCanBeAdded = ko.pureComputed(() => {
		return HelperDispatch.dispatchIsEditable(this.dispatch()) &&
			window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation";
	});
	canEditActualQuantities = ko.observable<boolean>(false);
	serviceOrder: KnockoutObservable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead>;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super(parentViewModel);
		this.currentServiceOrderTimeId =
			parentViewModel.dispatch && parentViewModel.dispatch() && parentViewModel.dispatch().CurrentServiceOrderTimeId()
				? parentViewModel.dispatch().CurrentServiceOrderTimeId()
				: null;
		this.parentViewModel = parentViewModel;
		this.dispatch = parentViewModel.dispatch;
		this.serviceOrder = parentViewModel.serviceOrder;
	}
	
	async init(): Promise<void> {
		let canEditActualQuantities = await window.Helper.ServiceOrder.canEditActualQuantities(this.serviceOrder().Id());
		this.canEditActualQuantities(canEditActualQuantities);
		if (window.database.CrmService_ServiceOrderTimeDispatch) {
			let serviceOrderTimeDispatches = await window.database.CrmService_ServiceOrderTimeDispatch.filter(function (it) {
				return it.ServiceOrderDispatchId === this.dispatchId;
			}, {dispatchId: this.dispatch().Id()}).toArray();
			this.serviceOrderTimeDispatches(serviceOrderTimeDispatches.map(x => x.asKoObservable()));
		}
		await super.init();
	}

	applyFilters(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		query = super.applyFilters(query);
		if (this.serviceOrderTimeDispatches().length > 0){
			let serviceOrderTimeIds = this.serviceOrderTimeDispatches().map(x => x.ServiceOrderTimeId());
			query = query.filter(function(it){
				return it.Id in this.serviceOrderTimeIds || it.CreateUser === this.createUser;
			}, { serviceOrderTimeIds: serviceOrderTimeIds, createUser: this.currentUser().Id });
		}
		return query;
	};

	applyOrderBy(query: $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime>): $data.Queryable<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		// @ts-ignore
		query = query.orderByDescending("orderByCurrentServiceOrderTime", {currentServiceOrderTimeId: this.currentServiceOrderTimeId});
		return super.applyOrderBy(query);
	};

	canCompleteJob(jobSummary: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		return jobSummary.StatusKey() !== "Finished";
	};

	canDeleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		const hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", window.ko.unwrap(serviceOrderTime.CreateUser) === window.Helper.User.getCurrentUserName() ? "TimeDeleteSelfCreated" : "TimeDelete");
		return hasPermission && HelperDispatch.dispatchIsEditable(this.dispatch());
	};

	canEditServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		return HelperDispatch.dispatchIsEditable(this.dispatch());
	};

	canReportScheduledMaterial(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): boolean {
		const hasPermission = window.AuthorizationManager.isAuthorizedForAction("ServiceOrder", "EditMaterial");
		//@ts-ignore
		return hasPermission && this.canEditActualQuantities() && (HelperDispatch.dispatchIsEditable(this.dispatch()) || serviceOrderTime.addableMaterialsAfterSignatureCount() > 0);
	};

	async completeJob(jobSummary: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel): Promise<void> {
		if (!this.canCompleteJob(jobSummary)) {
			return;
		}
		if (jobSummary.closedServiceCasesCount() < jobSummary.serviceCasesCount()) {
			let confirm = await window.Helper.Confirm.genericConfirmAsync({
				title: window.Helper.String.getTranslatedString("Complete"),
				text: window.Helper.String.getTranslatedString("CompleteJobWithOpenServiceCasesConfirmation"),
				type: "warning",
				confirmButtonText: window.Helper.String.getTranslatedString("Complete")
			});
			if (!confirm) {
				return;
			}
		}

		const serviceOrderTime = jobSummary;
		this.loading(true);
		if (serviceOrderTime.Id() === this.dispatch().CurrentServiceOrderTimeId()) {
			window.database.attachOrGet(this.dispatch().innerInstance);
			this.dispatch().CurrentServiceOrderTimeId(null);
			this.dispatch().CurrentServiceOrderTime(null);
		}
		window.database.attachOrGet(serviceOrderTime.innerInstance);
		serviceOrderTime.CompleteDate(new Date());
		serviceOrderTime.TimeZone(window.Helper.Date.getTimeZoneForUser(await window.Helper.User.getCurrentUser()));
		serviceOrderTime.CompleteUser(window.Helper.User.getCurrentUserName());
		serviceOrderTime.StatusKey("Finished");
		await window.database.saveChanges()
		this.loading(false);
	};

	async toggleCurrentJob(selectedServiceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): Promise<void> {
		this.loading(true);
		await window.Helper.Dispatch.toggleCurrentJob(this.dispatch, selectedServiceOrderTime.Id());
		this.loading(false);
	};

	async deleteServiceOrderTime(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): Promise<void> {
		window.database.attachOrGet(this.dispatch().innerInstance);
		const serviceOrderTimeId = window.ko.unwrap(serviceOrderTime.Id);
		if (this.dispatch().CurrentServiceOrderTimeId() === serviceOrderTimeId) {
			this.dispatch().CurrentServiceOrderTimeId(null);
			this.dispatch().CurrentServiceOrderTime(null);
		}
		await super.deleteServiceOrderTime(serviceOrderTime);
	};

	getAvatarColor(serviceOrderTime: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime): string {
		if (this.dispatch() && serviceOrderTime.Id() === this.dispatch().CurrentServiceOrderTimeId()) {
			return "#2196f3";
		}
		return super.getAvatarColor(serviceOrderTime);
	};
	async initItems(items: (Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel)[]): Promise<(Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel)[]> {
		const queries = [];
		const dispatchId = this.dispatch().Id();
		await super.initItems(items);
		let result = await window.Helper.ServiceOrderMaterial.getValidItemNosAfterSignature();
		let validMaterialItemNosAfterCustomerSignature = result.validMaterialItemNosAfterCustomerSignature;
		let validCostItemNosAfterCustomerSignature = result.validCostItemNosAfterCustomerSignature;
		const alreadyUsedMaterials = await window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
			return it.ParentServiceOrderMaterialId !== null && it.DispatchId === this.id && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Used;
		}, { id: this.dispatch().Id })
			.map(x => x.ParentServiceOrderMaterialId)
			.toArray();
		const alreadyUsedTimePostings = await window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
			return it.ParentServiceOrderTimePostingId !== null && it.DispatchId === this.id && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used;
		}, { id: this.dispatch().Id })
			.map(x => x.ParentServiceOrderTimePostingId)
			.toArray();
		items.forEach(function (jobSummary: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderTime & ServiceOrderDetailsJobsTabItemViewModel) {
			const getTimePostingsQueryable = () => window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
				return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Used && it.DispatchId === this.id;
			},
				{ serviceOrderTimeId: jobSummary.Id(), id: dispatchId });

			queries.push({
				queryable: window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
					return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Used && it.DispatchId === this.id;
				},
					{ serviceOrderTimeId: jobSummary.Id(), id: dispatchId }),
				method: "toArray",
				handler: async function (materials: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial[]) {
					jobSummary.serviceOrderMaterialsCount ||= ko.observable<number>(null);
					jobSummary.serviceOrderMaterialsCount(materials.length);
					jobSummary.addableMaterialsAfterSignatureCount ||= ko.observable<number>(null);
					jobSummary.addableMaterialsAfterSignatureCount(materials.filter(m => validMaterialItemNosAfterCustomerSignature.indexOf(m.ItemNo) !== -1 || validCostItemNosAfterCustomerSignature.indexOf(m.ItemNo) !== -1).length);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
					return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderMaterialType === window.Crm.Service.ServiceOrderMaterialType.Preplanned && !(it.Id in this.alreadyUsedMaterials);
				},
					{ serviceOrderTimeId: jobSummary.Id(), alreadyUsedMaterials: alreadyUsedMaterials }),
				method: "count",
				handler: function (count: number) {
					jobSummary.preplannedServiceOrderMaterialsCount ||= ko.observable<number>(null);
					jobSummary.preplannedServiceOrderMaterialsCount(count);
					return items;
				}
			});
			queries.push({
				queryable: getTimePostingsQueryable(),
				method: "count",
				handler: function (count: number) {
					jobSummary.postingsCount ||= ko.observable<number>(null);
					jobSummary.postingsCount(count);
					return items;
				}
			});
			queries.push({
				queryable: window.database.CrmService_ServiceOrderTimePosting.filter(function (it) {
					return it.ServiceOrderTimeId === this.serviceOrderTimeId && it.ServiceOrderTimePostingType === window.Crm.Service.ServiceOrderTimePostingType.Preplanned && !(it.Id in this.alreadyUsedTimePostings);
				},
					{ serviceOrderTimeId: jobSummary.Id(), alreadyUsedTimePostings: alreadyUsedTimePostings }),
				method: "count",
				handler: function (count: number) {
					jobSummary.preplannedPostingsCount ||= ko.observable<number>(null);
					jobSummary.preplannedPostingsCount(count);
					return items;
				}
			});
			queries.push({
				queryable: getTimePostingsQueryable(),
				method: "toArray",
				handler: function (timePostings: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting[]) {
					const durations = timePostings.map((timeposting) => window.moment(timeposting.To).diff(window.moment(timeposting.From)))
					const totalDuration = durations.reduce((sum, duration) => sum + duration, 0);
					jobSummary.ActualDuration(totalDuration.toString());
					return items;
				}
			});
		});
		await window.Helper.Batch.Execute(queries)
		return items;
	};

}

namespace("Crm.Service.ViewModels").DispatchDetailsJobsTabViewModel = DispatchDetailsJobsTabViewModel;