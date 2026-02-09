import {namespace} from "./namespace";
import {HelperString} from "./helper/Helper.String";
import {HelperDatabase} from "./helper/Helper.Database";

export class PmbbViewModel {

	caption = ko.observable<string>(null);
	icon = ko.observable<string>(null);
	loading = ko.observable<boolean>(false);
	errors: CustomKnockoutValidationErrors = null;
	isToggled = ko.observable<boolean>(false);
	condition: KnockoutComputed<boolean>;
	css: string;
	showHeader: KnockoutComputed<boolean>;
	showActions = ko.observable<boolean>(true);
	viewContext: any;
	editContext = ko.observable<any>(null);
	buttons: any[];
	params: { onInit?: any; onCancel?: any; onBeforeSave?: any; onSave?: any; onAfterSave?: any; buttons?: any[]; context?: any; showHeader?: boolean; showActions?: boolean;  css?: string; condition?: boolean; caption?: string; icon?: null };

	constructor(params: {
		onInit?: any;
		onCancel?: any;
		onBeforeSave?: any;
		onSave?: any;
		onAfterSave?: any;
		buttons?: any[];
		context?: any;
		showHeader?: boolean;
		showActions?: boolean;
		css?: string;
		condition?: boolean;
		caption?: string;
		icon?: null;
	}) {

		this.params = params;
		this.caption(HelperString.getTranslatedString(ko.unwrap(params.caption), ko.unwrap(params.caption)));
		this.icon(params.icon || null);

		this.condition = ko.computed(() => ko.unwrap(params.condition) !== false);
		this.css = ko.unwrap(params.css) || "";
		this.showHeader = ko.computed(() => ko.unwrap(params.showHeader ?? true) !== false && (this.caption() != null || this.icon() != null));
		this.showActions(ko.unwrap(params.showActions ?? true));
		this.viewContext = ko.unwrap(params.context);
		this.buttons = (params.buttons || [])
			.filter(button => !button.requiredPermission || window.AuthorizationManager.isAuthorizedForAction(button.requiredPermission.group, button.requiredPermission.name))
			.map(button => {
				button.click = button.click || function () {
				};
				button.icon = null;
				button.text = button.text || null;
				button.css = button.css || null;
				return button;
			});
		this.buttons.unshift({
			click: this.toggle.bind(this),
			icon: "edit",
			text: HelperString.getTranslatedString("Edit"),
			css: null
		})
		if (params.onInit) {
			params.onInit(this);
		}

	}

	reset(): void {
		this.editContext(null);

		const editContext = {}
		Object.getOwnPropertyNames(this.viewContext).forEach(name => {
			const clone = HelperDatabase.createClone(this.viewContext[name]);
			editContext[name] = ko.observable(clone ? clone.asKoObservable() : clone);
		});

		this.editContext(editContext);
		// TODO: use CustomKnockout without window prefix
		this.errors = window.ko.validation.group(this.editContext, {deep: true});
		Object.getOwnPropertyNames(this.viewContext).forEach(name => {
			const observableEntity = this.editContext()[name]();
			if (observableEntity) {
				observableEntity.innerInstance.addValidationRules();
			}
		});
	}

	toggle(element, e) {
		this.toggleAndReset(e.target);
	}

	toggleAndReset(element) {
		$(element).closest(".pmb-block").toggleClass("toggled");
		this.isToggled(!this.isToggled());
		this.reset();
	}

	cancel(context, e) {
		if (this.params.onCancel) {
			this.params.onCancel(this);
		}
		this.toggleAndReset(e.target);
	}

	isTransient(element) {

		// @ts-ignore
		const entitySet = window.database.getEntitySetFromElementType(element.getType());
		if (entitySet) {
			const keyProperties = entitySet.elementType.memberDefinitions.getKeyProperties();
			if (keyProperties.length === 1) {

				// @ts-ignore
				const isGuidType = keyProperties[0].dataType === $data.Guid;
				const idProperty = keyProperties[0].name;
				if (isGuidType && element[idProperty] === HelperString.emptyGuid()) {
					return true;
				} else if (!isGuidType && (element[idProperty] === null || element[idProperty] === 0)) {
					return true;
				}
				return false;
			}
		}
		return null;
	};

	async save(context, e: Event): Promise<void> {
		this.loading(true);
		const loading = ko.contextFor((e.target as Node)).$root.loading || function () {
		};
		loading(true);

		if (this.params.onBeforeSave) {
			try {
				await this.params.onBeforeSave(this);
			} catch {
				this.loading(false);
				loading(false);
				return;
			}
		}
		await this.errors.awaitValidation();
		if (this.errors().length > 0) {
			Object.getOwnPropertyNames(this.viewContext).forEach(name => {
				this.editContext()[name].valueHasMutated();
			});
			this.errors.showAllMessages();
			this.errors.scrollToError();
			this.loading(false);
			loading(false);
			return;
		}
		Object.getOwnPropertyNames(this.viewContext).forEach(name => {
			const observableEntity = this.editContext()[name]();
			if (!observableEntity) {
				return;
			}
			const editedInnerInstance = observableEntity.innerInstance;
			const changedProperties = (editedInnerInstance.changedProperties || []).map(function (x) {
				return x.name;
			});
			let changedExtensionProperties;
			if (editedInnerInstance.ExtensionValues) {
				changedExtensionProperties = editedInnerInstance.ExtensionValues.changedProperties;
			}
			changedExtensionProperties = (changedExtensionProperties || []).map(function (x) {
				return x.name;
			});
			if (changedProperties.length || changedExtensionProperties.length) {
				const innerInstance = ko.unwrap(this.viewContext[name]).innerInstance;
				if (this.isTransient(innerInstance) === true) {
					window.database.add(innerInstance);
				} else if (this.isTransient(innerInstance) === false) {
					window.database.attachOrGet(innerInstance);
				}
				HelperDatabase.transferData(changedProperties, editedInnerInstance, innerInstance);
				if (editedInnerInstance.ExtensionValues && innerInstance.ExtensionValues) {
					HelperDatabase.transferData(changedExtensionProperties, editedInnerInstance.ExtensionValues, innerInstance.ExtensionValues);
				}
			}
		});
		if (this.params.onSave) {
			try {
				await this.params.onSave(this);
			} catch(e) {
				window.Log.error(e);
				this.loading(false);
				loading(false);
				return;
			}
		}
		await window.database.saveChanges();
		this.loading(false);
		loading(false);
		this.toggleAndReset(e.target);
		if (this.params.onAfterSave) {
			this.params.onAfterSave(this);
		}
	}


}

namespace("Main.ViewModels").PmbbViewModel = PmbbViewModel;


