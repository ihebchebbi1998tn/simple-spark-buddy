import {namespace} from "@Main/namespace";

export class DispatchAppointmentModalViewModelContactPerson {
	Avatar: string;
	Name: string;
	Type: string;
	Email: string;
	Fax: string;
	Mobile: string;
	Phone: string;
}

export class DispatchAppointmentModalViewModel extends window.Main.ViewModels.ViewModelBase {
	action = ko.observable<string>(null);
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	currentUser = ko.observable<Main.Rest.Model.Main_User>(null);
	lookups: LookupType = {
		countries: {$tableName: "Main_Country"}
	}
	currentUserDropboxAddress = ko.pureComputed<string>(() => {
		return this.currentUser() && this.currentUser().ExtensionValues.DropboxToken && window.Crm.Settings.DropboxDomain
			? "?BCC=" + this.currentUser().ExtensionValues.DropboxToken + "@" + window.Crm.Settings.DropboxDomain + "&"
			: "?";
	});

	contactPersons = ko.pureComputed<DispatchAppointmentModalViewModelContactPerson[]>(() => {
		const contactPersons: DispatchAppointmentModalViewModelContactPerson[] = [];
		const serviceOrder = this.dispatch().ServiceOrder();
		if (serviceOrder.ServiceLocationEmail() ||
			serviceOrder.ServiceLocationFax() ||
			serviceOrder.ServiceLocationMobile() ||
			serviceOrder.ServiceLocationPhone()) {
			contactPersons.push({
				Avatar: serviceOrder.ServiceLocationResponsiblePerson() ? window.Helper.Person.getInitials(serviceOrder.ServiceLocationResponsiblePerson()) : null,
				Name: serviceOrder.ServiceLocationResponsiblePerson(),
				Type: window.Helper.String.getTranslatedString("ServiceLocationResponsibleContact"),
				Email: serviceOrder.ServiceLocationEmail(),
				Fax: serviceOrder.ServiceLocationFax(),
				Mobile: serviceOrder.ServiceLocationMobile(),
				Phone: serviceOrder.ServiceLocationPhone()
			});
		}
		if (serviceOrder.InitiatorPerson()) {
			contactPersons.push({
				Avatar: window.Helper.Person.getInitials(serviceOrder.InitiatorPerson()),
				Name: window.Helper.Person.getDisplayName(serviceOrder.InitiatorPerson()),
				Type: window.Helper.String.getTranslatedString("Initiator"),
				Email: this.getPrimaryEmail(serviceOrder.InitiatorPerson().Emails()),
				Mobile: null,
				Fax: this.getPrimaryPhone(serviceOrder.InitiatorPerson().Faxes()),
				Phone: this.getPrimaryPhone(serviceOrder.InitiatorPerson().Phones())
			});
		}
		if (serviceOrder.Installation() && serviceOrder.Installation().Person()) {
			contactPersons.push({
				Avatar: window.Helper.Person.getInitials(serviceOrder.Installation().Person()),
				Name: window.Helper.Person.getDisplayName(serviceOrder.Installation().Person()),
				Type: window.Helper.String.getTranslatedString("Installation"),
				Email: this.getPrimaryEmail(serviceOrder.Installation().Person().Emails()),
				Fax: this.getPrimaryPhone(serviceOrder.Installation().Person().Faxes()),
				Mobile: null,
				Phone: this.getPrimaryPhone(serviceOrder.Installation().Person().Phones())
			});
		}
		return contactPersons;
	});
	subject = ko.pureComputed<string>(() => {
		let subject = null;

		switch (this.action()) {
			case "request":
				subject = window.Helper.String.getTranslatedString("AppointmentRequestSubject");
				break;
			case "confirm":
				subject = window.Helper.String.getTranslatedString("AppointmentConfirmationSubject");
				break;
		}

		if (subject) {
			const dispatch = this.dispatch();
			const dateText = window.Globalize.formatDate(dispatch.Date(), { date: "full" });
			const timeText = window.Globalize.formatDate(dispatch.Date(), { time: "short" });
			const serviceOrderNo = dispatch.ServiceOrder().OrderNo();
			const serviceObjectName = dispatch.ServiceOrder().ServiceObject()?.Name() ?? null;
			const orderInfo = serviceOrderNo + (serviceObjectName ? " - " + serviceObjectName : "");

			subject = subject.replace("{0}", orderInfo)
				.replace("{1}", dateText)
				.replace("{2}", timeText);
		}

		return subject;
	});

	getPrimaryEmail(emails: Crm.Rest.Model.ObservableCrm_Email[]): string {
		if (emails.length === 0) {
			return null;
		}
		return ko.unwrap(window.Helper.Address.getPrimaryCommunication(emails).Data);
	};

	getPrimaryPhone(phones: Crm.Rest.Model.ObservableCrm_Phone[]): string {
		const phone = window.Helper.Address.getPrimaryCommunication(phones);
		return window.Helper.Address.getPhoneNumberAsString(phone, true, this.lookups.countries);
	};

	composeBody(person) {
		var viewModel = this;

		const dateText = window.Globalize.formatDate(viewModel.dispatch().Date(), { date: "full" });
		const timeText = window.Globalize.formatDate(viewModel.dispatch().Date(), { time: "short" });

		let body = null;

		switch (viewModel.action()) {
			case "request":
				body = window.Helper.String.getTranslatedString("AppointmentRequestBody");
				break;
			case "confirm":
				body = window.Helper.String.getTranslatedString("AppointmentConfirmationBody");
				break;
		}

		if (body) {
			body = body.replace("{0}", dateText)
				.replace("{1}", timeText);
		}

		return body;
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		this.action(params.action);
		await window.Helper.Lookup.getLocalizedArrayMaps(this.lookups);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ServiceOrder")
			.include("ServiceOrder.ServiceObject")
			.include("ServiceOrder.Installation.Person.Emails")
			.include("ServiceOrder.Installation.Person.Faxes")
			.include("ServiceOrder.Installation.Person.Phones")
			.include("ServiceOrder.InitiatorPerson.Emails")
			.include("ServiceOrder.InitiatorPerson.Faxes")
			.include("ServiceOrder.InitiatorPerson.Phones")
			.find(id);
		this.dispatch(dispatch.asKoObservable());
		let currentUser = await window.Helper.User.getCurrentUser();
		this.currentUser(currentUser);
	};
}

namespace("Crm.Service.ViewModels").DispatchAppointmentModalViewModel = DispatchAppointmentModalViewModel;