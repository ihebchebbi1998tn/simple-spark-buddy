import { namespace } from "@Main/namespace";
import type { Technician } from "../Model/Technicians";
import type { SchedulerDetailsViewModel } from "../SchedulerDetailsViewModel";
import type {CalendarModel} from "@bryntum/schedulerpro";

export class SchedulerAdHocModalViewModel extends window.Crm.Service.ViewModels.DispatchAdHocViewModel {
	parentViewModel: SchedulerDetailsViewModel;
	date: Date;
	endDate: Date;
	resourceRecord: Technician;

	constructor(data) {
		super();
		this.date = data?.date ?? window.moment().startOf('minute').toDate();
		this.date.setUTCMinutes(Math.ceil(this.date.getUTCMinutes() / 5) * 5);
		this.resourceRecord = data?.resourceRecord;
		this.parentViewModel = data?.parentViewModel ?? data;
		this.isModal(true);
		let calendar = (this.resourceRecord && this.resourceRecord["calendar"] as CalendarModel) ?? (this.parentViewModel.scheduler().project.calendar as CalendarModel);
		this.endDate = this.parentViewModel.profile().ClientConfig.ServiceOrderDispatchIgnoreCalculatedDuration ? window.moment(this.date).add(this.parentViewModel.profile().ClientConfig.ServiceOrderDispatchDefaultDuration, "m").toDate() : calendar.calculateEndDate(this.date, window.moment.duration(this.parentViewModel.profile().ClientConfig.ServiceOrderDispatchDefaultDuration, "m").asMilliseconds());
	}

	async init(): Promise<void> {
		await super.init();
		const profileResources = await window.database.SmsScheduler_ProfileResource.filter("it.ProfileKey == profile.Id", { profile: this.parentViewModel.profile() })
			.select(pr => pr.ResourceKey)
			.toArray();
		this.technicianFilterQuery = (query, term) => Helper.User.filterUserQuery(query, term)
			.filter("t => t.Id in profileResources", { profileResources: profileResources });
		this.dispatch().Username(this.resourceRecord?.ResourceKey ?? undefined);
		this.dispatch().Username.isModified(false);
		this.dispatch().Date(this.date);
		this.dispatch().EndDate(this.endDate);
		
		if(this.resourceRecord != null){
			this.serviceOrder().StationKey(this.resourceRecord.OriginalData.ExtensionValues?.StationIds?.length === 1 ? this.resourceRecord.OriginalData.ExtensionValues.StationIds[0] : null);
		}
	}

	override async submit() {
		this.loading(true);
		let dispatchNo = await window.NumberingService.createNewNumberBasedOnAppSettings(window.Crm.Service.Settings.Dispatch.DispatchNoIsGenerated, window.Crm.Service.Settings.Dispatch.DispatchNoIsCreateable, this.dispatch().DispatchNo(), "SMS.ServiceOrderDispatch", window.database.CrmService_ServiceOrderDispatch, "DispatchNo");
		if (dispatchNo !== null) {
			this.dispatch().DispatchNo(dispatchNo)
		}
		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.errors.expandCollapsiblesWithErrors();
			return;
		}
		this.serviceOrder().StatusKey("Released");
		if (!this.serviceOrder().OrderNo()) {
			this.dispatch().OrderId(this.serviceOrder().Id());
			let serviceOrderNo = await window.NumberingService.getNextFormattedNumber(window.Crm.Service.Settings.AdHoc.AdHocNumberingSequenceName);
			this.serviceOrder().OrderNo(serviceOrderNo);
		}
		const dispatch = this.dispatch();
		const startDate = dispatch.Date();
		const endDate = dispatch.EndDate();
		
		await new window.Helper.ServiceOrder.CreateServiceOrderData(
			this.serviceOrder(),
			this.serviceOrder().ServiceOrderTemplate(),
			this.installationIds(),
			startDate,
			endDate
		).create();

		try {
			await window.database.saveChanges();

			$(".modal:visible").modal("hide");

			if (this.parentViewModel.onNewServiceOrder) {
				await this.parentViewModel.onNewServiceOrder(this.serviceOrder().Id());
			}
		} catch (e) {
			window.Log.error((e as Error));
			swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
		} finally {
			this.loading(false);
		}
	};

	async cancel() {
		$(".modal:visible").modal("hide");
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerAdHocModalViewModel = SchedulerAdHocModalViewModel;