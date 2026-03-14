import {namespace} from "@Main/namespace";
import type {DispatchDetailsViewModel} from "./DispatchDetailsViewModel";

export class DispatchReportRecipientsModalViewModel extends window.Main.ViewModels.ViewModelBase {
	customRecipient = ko.observable<boolean>(false);
	email = ko.observable<string>(null).extend({
		email: {params: true, message: window.Helper.String.getTranslatedString("RuleViolation.InvalidEmail")}
	});
	customEmail = ko.observable<string>(null).extend({
		email: {params: true, message: window.Helper.String.getTranslatedString("RuleViolation.InvalidEmail")}
	});
	language = ko.observable<string>(null);
	locale = ko.observable<string>(null);
	dispatch = ko.observable<Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatch>(null);
	company = ko.observable<Crm.Rest.Model.ObservableCrm_Company>(null);
	languages = ko.observableArray<Main.Rest.Model.Lookups.Main_Language>([]);
	locales = ko.observableArray<string>([]);
	persons = ko.pureComputed<Crm.Rest.Model.ObservableCrm_Person[]>(() => {
		return this.company() ? this.company().Staff() : [];
	});

	selectableContacts = ko.pureComputed<{ Contact: Crm.Rest.Model.ObservableCrm_Person | Crm.Rest.Model.ObservableCrm_Company, DisplayName: string, Value: string }[]>(() => {
		if (!this.dispatch()) {
			return [];
		}
		const selectedEmails = this.dispatch().ReportRecipients().reduce(function (map, item) {
			map[item.Email()] = true;
			return map;
		}, {});
		const personEmails = this.getPersonsEmails();
		const companyEmails = this.getCompanyEmails();
		const result: { Contact: Crm.Rest.Model.ObservableCrm_Person | Crm.Rest.Model.ObservableCrm_Company, DisplayName: string, Value: string }[] = [];
		Object.keys(personEmails).sort().forEach(function (email) {
			if (selectedEmails[email]) {
				return;
			}
			result.push({
				Contact: personEmails[email].Contact,
				DisplayName: personEmails[email].DisplayName,
				Value: email
			});
		});
		Object.keys(companyEmails).sort().forEach(function (email) {
			if (selectedEmails[email]) {
				return;
			}
			result.push({
				Contact: companyEmails[email].Contact,
				DisplayName: companyEmails[email].DisplayName,
				Value: email
			});
		});
		const serviceLocationEmail = this.dispatch().ServiceOrder().ServiceLocationEmail();
		if (serviceLocationEmail && !personEmails[serviceLocationEmail] && !companyEmails[serviceLocationEmail] && !selectedEmails[serviceLocationEmail]) {
			const serviceLocationResponsiblePerson = this.dispatch().ServiceOrder().ServiceLocationResponsiblePerson();
			result.push({
				Contact: null,
				DisplayName: serviceLocationResponsiblePerson
					? `${serviceLocationResponsiblePerson} (${serviceLocationEmail})`
					: serviceLocationEmail,
				Value: serviceLocationEmail
			});
		}
		return result.sort((a,b) => (a.DisplayName > b.DisplayName) ? 1 : ((b.DisplayName > a.DisplayName) ? -1 : 0));
	});
	selectableLocales = ko.observableArray<string>([]);

	errors = ko.validation.group([this.dispatch, this.email, this.customEmail], {deep: true});
	parentViewModel: DispatchDetailsViewModel;

	constructor(parentViewModel: DispatchDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}

	refreshParentViewModel(): void {
		this.parentViewModel.init(this.dispatch().Id());
	};

	async init(id?: string, params?: {[key:string]:string}): Promise<void> {
		await super.init(id, params);
		let dispatch = await window.database.CrmService_ServiceOrderDispatch
			.include("ReportRecipients")
			.include("ServiceOrder")
			.find(id);

		dispatch.ReportRecipients.sort((a, b) => a.Email.localeCompare(b.Email));
		window.database.attachOrGet(dispatch);
		this.dispatch(dispatch.asKoObservable());
		let company = await window.database.Crm_Company
			.include2("Staff.filter(function(x) { return x.IsRetired === false })")
			.include("Staff.Emails")
			.include("Emails")
			.find(this.dispatch().ServiceOrder().CustomerContactId());
		this.company(company.asKoObservable());
		let languages = await window.Helper.Lookup.queryLookup("Main_Language", null, "it.IsSystemLanguage === true", {}).toArray();
		this.languages(languages);
		let locales = await $.get(window.Helper.Url.resolveUrl("~/Main/Resource/ListLocales?format=json"));
		this.locales(locales);
		this.email.subscribe(value => this.onEmailSelect(value));
		this.language.subscribe(value => this.onLanguageSelect(value));
	};

	dispose(): void {
		window.database.detach(this.dispatch().innerInstance);
		this.dispatch().ReportRecipients().forEach(x => window.database.detach(x.innerInstance));
	};

	addRecipient(): void {
		let recipientEmail = null;
		if (this.email() && this.email.isValid()) {
			recipientEmail = this.email();
		}
		else if (this.customEmail() && this.customEmail.isValid()) {
			recipientEmail = this.customEmail();
		}
		else {
			return;
		}

		const index = this.dispatch().ReportRecipients
			.indexOf(ko.utils.arrayFirst(this.dispatch().ReportRecipients(),
				item => {
					return item.Email() === recipientEmail && item.Language() === this.language() && item.Locale() === this.locale();
				}));
		if (recipientEmail && index === -1) {
			const recipient = window.database.CrmService_ServiceOrderDispatchReportRecipient.defaultType.create();
			recipient.DispatchId = this.dispatch().Id();
			recipient.Email = recipientEmail;
			recipient.Language = this.language();
			recipient.Locale = this.locale();
			window.database.add(recipient);
			this.dispatch().ReportRecipients.push(recipient.asKoObservable());
			this.dispatch().ReportRecipients.sort((a, b) => a.Email().localeCompare(b.Email()));
			this.email(null);
			this.customEmail(null);
			this.language(null);
			this.locale(null);
		}
	};

	onEmailSelect(email: string): void {
		this.language(null);
		this.locale(null);
		if (!email) {
			return;
		}
		const contact = this.selectableContacts().find(x => x.Value === email);
		if (!contact || !contact.Contact) {
			return;
		}
		this.language(contact.Contact.LanguageKey());
	};

	onLanguageSelect(languageKey: string): void {
		this.locale(null);
		if (!languageKey) {
			this.selectableLocales([]);
			return;
		}
		this.selectableLocales(this.locales().filter(x => x.startsWith(languageKey)));
		const language = this.languages() && this.languages().find(x => x.Key === languageKey);
		if (language) {
			if (language.DefaultLocale) {
				this.locale(language.DefaultLocale);
			} else if (this.selectableLocales().length > 0) {
				this.locale(this.selectableLocales()[0]);
			}
		}
	};

	removeRecipient(recipient: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderDispatchReportRecipient): void {
		const index = this.dispatch().ReportRecipients
			.indexOf(ko.utils.arrayFirst(this.dispatch().ReportRecipients(),
				item => {
					return item === recipient;
				}));
		this.dispatch().ReportRecipients.splice(index, 1);
		window.database.remove(recipient.innerInstance);
	};

	async save(): Promise<void> {
		this.loading(true);

		this.addRecipient();

		if (this.errors().length > 0) {
			this.loading(false);
			this.errors.showAllMessages();
			return;
		}

		try {
			await window.database.saveChanges();
			this.loading(false);
			this.refreshParentViewModel();
			$(".modal:visible").modal("hide");
		} catch {
			this.loading(false);
			window.swal(window.Helper.String.getTranslatedString("UnknownError"),
				window.Helper.String.getTranslatedString("Error_InternalServerError"),
				"error");
		}
	};

	getPersonsEmails(): { [key: string]: { Contact: Crm.Rest.Model.ObservableCrm_Person, DisplayName: string } } {
		return this.persons().reduce(function (emails, person) {
			const email = window.ko.unwrap((window.Helper.Address.getPrimaryCommunication(person.Emails) || {}).Data);
			if (email) {
				const name = window.Helper.Person.getDisplayName(person) + " (" + email + ")";
				if (!emails[email]) {
					emails[email] = {Contact: person, DisplayName: name};
				}
			}
			return emails;
		}, {});
	};

	getCompanyEmails(): { [key: string]: { Contact: Crm.Rest.Model.ObservableCrm_Company, DisplayName: string } } {
		const company = this.company();
		const emails = {};
		if (!company) {
			return emails;
		}
		(ko.unwrap(company.Emails) || []).forEach(function (item) {
			const email = item.Data();
			const name = window.Helper.Company.getDisplayName(company) + " (" + email + ")";
			if (!emails[email]) {
				emails[email] = {Contact: company, DisplayName: name};
			}
		});
		return emails;
	};

	toggleCustomRecipient(): void {
		this.customRecipient(!this.customRecipient());
		this.email(null);
		this.customEmail(null);
	};
}

namespace("Crm.Service.ViewModels").DispatchReportRecipientsModalViewModel = DispatchReportRecipientsModalViewModel;