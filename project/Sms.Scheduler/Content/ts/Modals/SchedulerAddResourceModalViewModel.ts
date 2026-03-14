import {namespace} from "@Main/namespace";
import type { SchedulerDetailsViewModel } from "../SchedulerDetailsViewModel";
import {isTechnician, Technician} from "../Model/Technicians";
import { Article, isTool, isVehicle } from "../Model/Article";


export class SchedulerAddResourceModalViewModel extends window.Main.ViewModels.ViewModelBase {
	parentViewModel: SchedulerDetailsViewModel = null;
	loading: KnockoutObservable<boolean> = ko.observable<boolean>(false);
	addedResources: KnockoutObservableArray<Main.Rest.Model.Main_User> = ko.observableArray<Main.Rest.Model.Main_User>([]);
	addedTools: KnockoutObservableArray<any> = ko.observableArray<any>([]);
	existingTechnicianResources: KnockoutObservableArray<Main.Rest.Model.Main_User> = ko.observableArray<Main.Rest.Model.Main_User>([]);
	existingToolResources: KnockoutObservableArray<Crm.Article.Rest.Model.CrmArticle_Article> = ko.observableArray<Crm.Article.Rest.Model.CrmArticle_Article>([]);
	userIds: KnockoutObservableArray<string> = ko.observableArray<string>([]);
	toolIds: KnockoutObservableArray<string> = ko.observableArray<string>([]);

	constructor(parentViewModel: SchedulerDetailsViewModel) {
		super();
		this.parentViewModel = parentViewModel;
	}
	async init(id: string): Promise<void> {
		let self = this;
		//@ts-ignore
		self.existingTechnicianResources(this.parentViewModel.scheduler().resourceStore.records.filter(it => isTechnician(it)).map(it => it.OriginalData).filter(Boolean));
		//@ts-ignore
		self.existingToolResources(this.parentViewModel.scheduler().resourceStore.records.filter(it => isTool(it) || isVehicle(it)).map(it => it.OriginalData).filter(Boolean));
	}
	async save(data, event): Promise<void> {
		$(event.target).closest(".modal-content").data("keepchanges", true);

		this.loading(true);
		try {
			const resourcesToAdd: (Technician | Article)[] = [];

			if (this.userIds().length > 0) {
				const technicians = await this.parentViewModel.loadTechnicians(this.userIds());
				technicians.forEach(t => t.SortOrder += this.parentViewModel.profile().ResourceKeys.length);
				resourcesToAdd.push(...technicians);
			}

			if (this.toolIds().length > 0) {
				const articles = await this.parentViewModel.loadArticles(this.toolIds());
				resourcesToAdd.push(...articles);
			}

			if (resourcesToAdd.length > 0) {
				window.Helper.Scheduler.Profile.AddResourceToProfile(this.parentViewModel.profile(), ...resourcesToAdd.map(t => t.ResourceKey as string));
				await this.parentViewModel.scheduler().resourceStore.addAsync(resourcesToAdd);
				queueMicrotask(() => this.parentViewModel.schedulerComponent().reloadDateRange());
			}
			$(".modal:visible").modal("hide");
		} catch (e) {
			swal(window.Helper.String.getTranslatedString("Error"), window.Helper.String.getTranslatedString("SomethingWentWrong"), "error");
			window.Log.error((e as Error));
		} finally {
			this.loading(false);
		}
	}

	OnResourceSelected(resource: Main.Rest.Model.Main_User | Crm.Article.Rest.Model.CrmArticle_Article): void {
		resource instanceof Crm.Article.Rest.Model.CrmArticle_Article ? this.addedTools.push(resource) : this.addedResources.push(resource);
	}
	cancel(): void {
		$(".modal:visible").modal("hide");
	}
	resourceFilter(query: $data.Queryable<Main.Rest.Model.Main_User>, term: string) {
		let self = this;
		query = query.filter("!(it.Id in this.resources)", { resources: self.existingTechnicianResources().map(it => it.Id)});
		// @ts-ignore
		if (query.specialFunctions.filterByPermissions[window.database.storageProvider.name]) {
			query = query.filter("filterByPermissions", "WebApiWrite::ServiceOrderDispatch");
		}
		if (term) {
			query = query.filter("it.Id.contains(this.term)",	{ term: term });
		}
		return query
			.orderBy("it.LastName")
			.orderBy("it.FirstName");
	}
	toolFilter(query: $data.Queryable<Crm.Article.Rest.Model.CrmArticle_Article>, term: string) {
		let self = this;
		query = query.filter("!(it.Id in this.resources) && it.ArticleTypeKey in this.articleTypeKeys", { resources: self.existingToolResources().map(it => it.Id), articleTypeKeys: ["Tool", "Vehicle"] });
		if (term) {
			query = query.filter("it.ItemNo.contains(this.term) || it.Description.contains(this.term)",	{ term: term });
		}
		return query.orderBy("it.ItemNo");
	}
	mapToolsForSelect2Display(tool: Crm.Article.Rest.Model.CrmArticle_Article) {
		return {
			id: tool.Id,
			item: tool,
			text: window.Helper.Article.getArticleAutocompleteDisplay(tool)
		};
	}
	mapUsersForSelect2Display(user: Main.Rest.Model.Main_User): Select2AutoCompleterResult {
		let text = `${window.Helper.User.getDisplayName(user)}`;
		if(user.Discharged) {
			text = text.concat(` - (${window.Helper.String.getTranslatedString('Inactive')})`);
		}
		return {
			id: user.Id,
			item: user,
			text: text
		};
	}
}

namespace("Sms.Scheduler.ViewModels").SchedulerAddResourceModalViewModel = SchedulerAddResourceModalViewModel;