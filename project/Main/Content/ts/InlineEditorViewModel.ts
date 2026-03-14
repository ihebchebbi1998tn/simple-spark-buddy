import {namespace} from "./namespace";
import {ViewModelBase} from "./ViewModelBase";
import {HelperString} from "./helper/Helper.String";
import {HelperDatabase} from "./helper/Helper.Database";
import type {GenericListViewModel} from "./GenericListViewModel";

export class InlineEditorViewModel extends window.Main.ViewModels.ViewModelBase {

	private params: any;
	viewContext: KnockoutObservable<any>;
	editContext: KnockoutObservable<any>;
	errors: any;
	isTransient = namespace("Main.ViewModels").PmbbViewModel.prototype.isTransient;

	constructor(params) {
		super();
		this.errors = null;
		this.params = params;
		this.viewContext = ko.observable();

		this.viewContext.subscribe(() => {
			this.switchToEditContext();
		});
		this.viewContext(this.params.context);

		if (this.params.onInit) {
			this.params.onInit(this);
		}
	}

	switchToEditContext(): void {
		let clone = HelperDatabase.createClone(this.viewContext());
		clone = clone ? clone.asKoObservable() : clone
		this.editContext = window.ko.observable(clone);
		this.editContext().innerInstance.addValidationRules();

		if (this.isTransient(this.viewContext().innerInstance) === false) {
			window.database.attachOrGet(clone.innerInstance);
		} else {
			window.database.add(clone.innerInstance);
		}
	}

	transferChangedProperties(component: InlineEditorViewModel): void {
		const innerInstance = component.viewContext().innerInstance;
		const editedInnerInstance = component.editContext().innerInstance;
		const changedProperties = (editedInnerInstance.changedProperties || []).map(prop => prop.name);
		let changedExtensionProperties;

		if (editedInnerInstance.ExtensionValues) {
			changedExtensionProperties = editedInnerInstance.ExtensionValues.changedProperties;
		}
		changedExtensionProperties = (changedExtensionProperties || []).map(prop => prop.name);
		if (changedProperties.length || changedExtensionProperties.length) {
			HelperDatabase.transferData(changedProperties, editedInnerInstance, innerInstance);
		}
		if (editedInnerInstance.ExtensionValues && innerInstance.ExtensionValues) {
			HelperDatabase.transferData(changedExtensionProperties, editedInnerInstance.ExtensionValues, innerInstance.ExtensionValues);
		}
	}

	async submit(genericListViewModel: GenericListViewModel<$data.Entity, $data.Entity>, component: InlineEditorViewModel): Promise<void> {
		genericListViewModel.loading(true);
		try {
			if (component.params.onBeforeSave) {
				await component.params.onBeforeSave(component);
			}

			component.errors = ko.validation.group(component.editContext, {deep: true});

			await component.errors.awaitValidation();
			if (component.errors().length > 0) {
				component.errors.showAllMessages();
				component.errors.scrollToError();
				genericListViewModel.loading(false);
				return;
			}

			if (component.params.onSave) {
				await component.params.onSave(component);
			}
			component.transferChangedProperties(component)
			await window.database.saveChanges();

			if (component.params.onAfterSave) {
				component.params.onAfterSave(component);
			}

			component.reset(genericListViewModel);
			$(document).trigger("inlineEditorClosed");
			genericListViewModel.loading(false);
			super.showSnackbar("Saved");

		} catch (e) {
			genericListViewModel.loading(false);
			swal(HelperString.getTranslatedString("Error"), (e as Error).message, "error");
		}

	}

	reset(genericListViewModel: GenericListViewModel<$data.Entity, $data.Entity>): void {
		genericListViewModel.isEditing(null);
	}

	cancel(genericListViewModel: GenericListViewModel<$data.Entity, $data.Entity>, component: InlineEditorViewModel): void {
		if (component.params.onCancel) {
			component.params.onCancel(component);
		}
		if (window.database.stateManager.trackedEntities.length > 0) {
			HelperDatabase.clearTrackedEntities();
		}
		component.reset(genericListViewModel);
		$(document).trigger("inlineEditorClosed");
	}

}

namespace("Main.ViewModels").InlineEditorViewModel = InlineEditorViewModel;


