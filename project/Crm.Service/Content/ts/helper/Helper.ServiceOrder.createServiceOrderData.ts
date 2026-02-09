export class HelperCreateServiceOrderData {
	serviceOrder: Crm.Service.Rest.Model.CrmService_ServiceOrderHead;
	serviceOrderTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderHead;
	invoicingTypes: { [key: string]: Main.Rest.Model.Lookups.Main_InvoicingType };
	installationIds: string[];
	serviceOrderData: any[];
	dispatchStartDate?: Date;
	dispatchEndDate?: Date;

	constructor(
		serviceOrder: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead,
		serviceOrderTemplate: Crm.Service.Rest.Model.ObservableCrmService_ServiceOrderHead,
		installationIds: string[],
		dispatchStartDate?: Date,
		dispatchEndDate?: Date
	) {
		this.serviceOrder = window.Helper.Database.getDatabaseEntity(serviceOrder);
		this.serviceOrderTemplate = window.Helper.Database.getDatabaseEntity(serviceOrderTemplate);

		this.invoicingTypes = {};
		this.installationIds = installationIds;
		this.serviceOrderData = [];
		this.dispatchStartDate = dispatchStartDate;
		this.dispatchEndDate = dispatchEndDate;
	}

	async create(): Promise<void> {
		this.invoicingTypes = await window.Helper.Lookup.getLocalizedArrayMap("Main_InvoicingType");
		await (this.serviceOrderTemplate ? this.createTemplateData() : this.createJobs());
		await this.setPositionNumbers();
	}

	createDocumentAttributeFromTemplate(documentAttributeTemplate: Crm.Rest.Model.Crm_DocumentAttribute, materialId: string = null, serviceOrderTimeId: string = null): Crm.Rest.Model.Crm_DocumentAttribute {
		const documentAttribute = window.database.Crm_DocumentAttribute.defaultType.create();
		documentAttribute.Description = documentAttributeTemplate.Description;
		documentAttribute.DocumentCategoryKey = documentAttributeTemplate.DocumentCategoryKey;
		documentAttribute.ExtensionValues = documentAttributeTemplate.ExtensionValues;
		if (materialId != null) {
			documentAttribute.ExtensionValues.ServiceOrderMaterialId = materialId;
		}
		if (serviceOrderTimeId != null) {
			documentAttribute.ExtensionValues.ServiceOrderTimeId = serviceOrderTimeId;
		}
		documentAttribute.FileName = documentAttributeTemplate.FileName;
		documentAttribute.FileResourceKey = documentAttributeTemplate.FileResourceKey;
		documentAttribute.Length = documentAttributeTemplate.Length;
		documentAttribute.LongText = documentAttributeTemplate.LongText;
		documentAttribute.OfflineRelevant = documentAttributeTemplate.OfflineRelevant;
		documentAttribute.ReferenceKey = this.serviceOrder.Id;
		documentAttribute.ContactType = "ServiceOrder";
		documentAttribute.ContactName = window.Helper.ServiceOrder.getDisplayName(this.serviceOrder);
		documentAttribute.UseForThumbnail = documentAttributeTemplate.UseForThumbnail;
		window.database.add(documentAttribute);
		this.serviceOrderData.push(documentAttribute);
		return documentAttribute;
	}

	async createJobs(): Promise<Crm.Service.Rest.Model.CrmService_ServiceOrderTime[]> {
		const serviceOrderTimes: Crm.Service.Rest.Model.CrmService_ServiceOrderTime[] = [];
		let estimatedDuration = null;

		if (this.dispatchStartDate && this.dispatchEndDate) {
			const msDuration = this.dispatchEndDate.getTime() - this.dispatchStartDate.getTime();
			estimatedDuration = this.msToISODuration(msDuration);
		}

		if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode === "JobPerInstallation") {
			this.installationIds.forEach((installationId) => {
				const serviceOrderTime = window.database.CrmService_ServiceOrderTime.defaultType.create();
				serviceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
				serviceOrderTime.InstallationId = installationId;
				serviceOrderTime.OrderId = this.serviceOrder.Id;
				serviceOrderTime.InvoicingTypeKey = this.serviceOrder.InvoicingTypeKey;
				serviceOrderTime.EstimatedDuration = estimatedDuration;

				window.Helper.Service.onInvoicingTypeSelected(serviceOrderTime, this.invoicingTypes[serviceOrderTime.InvoicingTypeKey]);
				window.database.add(serviceOrderTime);
				this.serviceOrderData.push(serviceOrderTime);
				serviceOrderTimes.push(serviceOrderTime);
			});
		}
		return serviceOrderTimes;
	}
	
	private msToISODuration(ms: number): string {
		const duration = window.moment.duration(ms);
		return `PT${duration.hours()}H${duration.minutes()}M${duration.seconds()}S`;
	}

	async createServiceOrderMaterialFromTemplate(serviceOrderMaterialTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial): Promise<Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial> {
		const serviceOrderMaterial = window.database.CrmService_ServiceOrderMaterial.defaultType.create();
		serviceOrderMaterial.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
		serviceOrderMaterial.ArticleId = serviceOrderMaterialTemplate.ArticleId;
		serviceOrderMaterial.Description = serviceOrderMaterialTemplate.Description;
		// @ts-ignore
		serviceOrderMaterial.ExtensionValues = serviceOrderMaterialTemplate.ExtensionValues;
		serviceOrderMaterial.ExternalRemark = serviceOrderMaterialTemplate.ExternalRemark;
		serviceOrderMaterial.InternalRemark = serviceOrderMaterialTemplate.InternalRemark;
		serviceOrderMaterial.IsSerial = serviceOrderMaterialTemplate.IsSerial;
		serviceOrderMaterial.ItemNo = serviceOrderMaterialTemplate.ItemNo;
		serviceOrderMaterial.OrderId = this.serviceOrder.Id;
		serviceOrderMaterial.Price = serviceOrderMaterialTemplate.Price;
		serviceOrderMaterial.Quantity = serviceOrderMaterialTemplate.Quantity;
		serviceOrderMaterial.QuantityUnitKey = serviceOrderMaterialTemplate.QuantityUnitKey;
		serviceOrderMaterial.ServiceOrderMaterialType = serviceOrderMaterialTemplate.ServiceOrderMaterialType;
		window.database.add(serviceOrderMaterial);
		this.serviceOrderData.push(serviceOrderMaterial);
		let documentAttributeTemplates = await window.database.Crm_DocumentAttribute.filter(function (it) {
				return it.ExtensionValues.ServiceOrderMaterialId === this.Id;
			},
			{Id: serviceOrderMaterialTemplate.Id})
			.toArray();
		documentAttributeTemplates.forEach((documentAttributeTemplate) => {
			this.createDocumentAttributeFromTemplate(documentAttributeTemplate, serviceOrderMaterial.Id);
		});
		return serviceOrderMaterial;
	}

	createServiceOrderTimePostingFromTemplate(serviceOrderTimePostingTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting): Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting {
		serviceOrderTimePostingTemplate = window.Helper.Database.getDatabaseEntity(serviceOrderTimePostingTemplate);
		const serviceOrderTimePosting = window.database.CrmService_ServiceOrderTimePosting.defaultType.create();
		serviceOrderTimePosting.Description = serviceOrderTimePostingTemplate.Description;
		serviceOrderTimePosting.Duration = serviceOrderTimePostingTemplate.Duration;
		// @ts-ignore
		serviceOrderTimePosting.ExtensionValues = serviceOrderTimePostingTemplate.ExtensionValues;
		serviceOrderTimePosting.InternalRemark = serviceOrderTimePostingTemplate.InternalRemark;
		serviceOrderTimePosting.ItemNo = serviceOrderTimePostingTemplate.ItemNo;
		serviceOrderTimePosting.OrderId = this.serviceOrder.Id;
		serviceOrderTimePosting.ServiceOrderTimePostingType = serviceOrderTimePostingTemplate.ServiceOrderTimePostingType;
		window.database.add(serviceOrderTimePosting);
		this.serviceOrderData.push(serviceOrderTimePosting);
		return serviceOrderTimePosting;
	}

	async createServiceOrderTimeFromTemplate(serviceOrderTimeTemplate: Crm.Service.Rest.Model.CrmService_ServiceOrderTime): Promise<Crm.Service.Rest.Model.CrmService_ServiceOrderTime> {
		serviceOrderTimeTemplate = window.Helper.Database.getDatabaseEntity(serviceOrderTimeTemplate);
		const serviceOrderTime = window.database.CrmService_ServiceOrderTime.defaultType.create();
		serviceOrderTime.DiscountType = Crm.Article.Model.Enums.DiscountType.Absolute;
		serviceOrderTime.ArticleId = serviceOrderTimeTemplate.ArticleId;
		serviceOrderTime.Comment = serviceOrderTimeTemplate.Comment;
		serviceOrderTime.Description = serviceOrderTimeTemplate.Description;
		serviceOrderTime.EstimatedDuration = serviceOrderTimeTemplate.EstimatedDuration;
		serviceOrderTime.ExtensionValues = serviceOrderTimeTemplate.ExtensionValues;
		serviceOrderTime.InvoicingTypeKey = serviceOrderTimeTemplate.InvoicingTypeKey;
		Helper.Service.onInvoicingTypeSelected(serviceOrderTime, this.invoicingTypes[serviceOrderTime.InvoicingTypeKey]);
		serviceOrderTime.ItemNo = serviceOrderTimeTemplate.ItemNo;
		serviceOrderTime.OrderId = this.serviceOrder.Id;
		window.database.add(serviceOrderTime);
		this.serviceOrderData.push(serviceOrderTime);
		let serviceOrderMaterialTemplates = await window.database.CrmService_ServiceOrderMaterial
			.filter("ServiceOrderTimeId", "===", serviceOrderTimeTemplate.Id)
			.toArray()

		for (const serviceOrderMaterialTemplate of serviceOrderMaterialTemplates) {
			const serviceOrderMaterial = await this.createServiceOrderMaterialFromTemplate(serviceOrderMaterialTemplate);
			serviceOrderMaterial.ServiceOrderTimeId = serviceOrderTime.Id;
		}
		let serviceOrderTimePostingTemplates = await window.database.CrmService_ServiceOrderTimePosting
			.filter("ServiceOrderTimeId", "===", serviceOrderTimeTemplate.Id)
			.toArray()

		for (const template of serviceOrderTimePostingTemplates) {
			const timePosting = this.createServiceOrderTimePostingFromTemplate(template);
			timePosting.ServiceOrderTimeId = serviceOrderTime.Id;
		}
		let documentAttributeTemplates = await window.database.Crm_DocumentAttribute.filter(function (it) {
				return it.ReferenceKey === this.serviceOrderTemplateId && it.ExtensionValues.ServiceOrderTimeId === this.serviceOrderTimeId;
			},
			{serviceOrderTemplateId: serviceOrderTimeTemplate.OrderId, serviceOrderTimeId: serviceOrderTimeTemplate.Id})
			.toArray();

		for (const documentAttributeTemplate of documentAttributeTemplates) {
			if (!documentAttributeTemplate.ExtensionValues.ServiceOrderMaterialId)
				this.createDocumentAttributeFromTemplate(documentAttributeTemplate, null, serviceOrderTime.Id);
		}
		return serviceOrderTime;
	}

	async createTemplateData(): Promise<Crm.Service.Rest.Model.CrmService_ServiceOrderTime[]> {
		window.Helper.Service.onInvoicingTypeSelected(this.serviceOrder, this.invoicingTypes[this.serviceOrder.InvoicingTypeKey]);
		let serviceOrderTimeTemplates = await window.database.CrmService_ServiceOrderTime.filter(function (it) {
				return it.OrderId === this.serviceOrderTemplateId;
			},
			{serviceOrderTemplateId: this.serviceOrderTemplate.Id})
			.orderBy("it.PosNo")
			.toArray();
		let serviceOrderTimes = [];

		if (serviceOrderTimeTemplates.length === 0) {
			serviceOrderTimes = await this.createJobs();
		} else {
			for (const serviceOrderTimeTemplate of serviceOrderTimeTemplates) {
				if (window.Crm.Service.Settings.ServiceContract.MaintenanceOrderGenerationMode ===
					"OrderPerInstallation") {
					let serviceOrderTime = await this.createServiceOrderTimeFromTemplate(serviceOrderTimeTemplate);
					serviceOrderTimes.push(serviceOrderTime);
				} else {
					if (this.installationIds.length === 0) {
						let serviceOrderTime = await this.createServiceOrderTimeFromTemplate(serviceOrderTimeTemplate);
						serviceOrderTimes.push(serviceOrderTime);
					}
					for (const installationId of this.installationIds) {
						let serviceOrderTime = await this.createServiceOrderTimeFromTemplate(serviceOrderTimeTemplate);
						serviceOrderTime.InstallationId = installationId;
						serviceOrderTimes.push(serviceOrderTime);
					}
				}
			}
		}
		let serviceOrderMaterialTemplates = await window.database.CrmService_ServiceOrderMaterial.filter(function (it) {
				return it.OrderId === this.serviceOrderTemplateId && it.ServiceOrderTimeId === null;
			},
			{serviceOrderTemplateId: this.serviceOrderTemplate.Id})
			.toArray();
		serviceOrderMaterialTemplates.forEach((serviceOrderMaterialTemplate) => {
			this.createServiceOrderMaterialFromTemplate(serviceOrderMaterialTemplate);
		});
		await window.database.CrmService_ServiceOrderTimePosting
			.filter("OrderId", "===", this.serviceOrderTemplate.Id)
			.filter("ServiceOrderTimeId", "===", null)
			.forEach((template) => this.createServiceOrderTimePostingFromTemplate(template));
		let documentAttributeTemplates = await window.database.Crm_DocumentAttribute.filter(function (it) {
				return it.ReferenceKey === this.serviceOrderTemplateId && it.ExtensionValues.ServiceOrderTimeId === null && it.ExtensionValues.ServiceOrderMaterialId === null;
			},
			{serviceOrderTemplateId: this.serviceOrderTemplate.Id})
			.toArray();
		documentAttributeTemplates.forEach((documentAttributeTemplate) => {
			if (!documentAttributeTemplate.ExtensionValues.ServiceOrderMaterialId)
				this.createDocumentAttributeFromTemplate(documentAttributeTemplate);
		});
		return serviceOrderTimes;
	}

	setPositionNumbers(): void {
		let posNo = 0;
		this.serviceOrderData.filter(function (x) {
			return x instanceof window.database.CrmService_ServiceOrderTime.defaultType;
		}).forEach(function (x) {
			x.PosNo = window.Helper.ServiceOrder.formatPosNo(++posNo);
		});
		this.serviceOrderData.filter(function (x) {
			return x instanceof window.database.CrmService_ServiceOrderMaterial.defaultType;
		}).forEach(function (x) {
			x.PosNo = window.Helper.ServiceOrder.formatPosNo(++posNo);
		});
	}
}