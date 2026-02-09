window.Helper.Database.addIndex("CrmService_ServiceOrderDispatch", ["Username", "StatusKey", "Date"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderDispatch", ["OrderId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderHead", ["OrderNo"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderHead", ["Planned", "PlannedTime"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderMaterial", ["DispatchId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderMaterial", ["OrderId", "IsActive"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderMaterialSerial", ["OrderMaterialId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderTimePosting", ["DispatchId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderTimePosting", ["OrderId", "DispatchId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderExpensePosting", ["DispatchId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderExpensePosting", ["OrderId", "DispatchId"]);
window.Helper.Database.addIndex("CrmService_ServiceOrderTime", ["OrderId", "IsActive"]);
window.Helper.Database.addIndex("CrmService_InstallationHeadStatus", ["Language"]);
window.Helper.Database.addIndex("CrmService_Installation", ["InstallationNo"]);
window.Helper.Database.addIndex("CrmService_Installation", ["LegacyInstallationId"]);
window.Helper.Database.addIndex("CrmService_Installation", ["InstallationNo", "LegacyInstallationId", "Description", "LocationContactId", "FolderId"]);
window.Helper.Database.addIndex("CrmService_ServiceObject", ["Name"]);
window.Helper.Database.addIndex("Crm_Company", ["Name"]);
window.Helper.Database.addIndex("Crm_Company", ["LegacyId", "Name"]);
window.Helper.Database.addIndex("Crm_Note", ["ContactId", "CreateDate"]);
window.Helper.Database.addIndex("Crm_Note", ["ExtensionValues__DispatchId", "CreateDate"]);
window.Helper.Database.addIndex("Crm_DocumentAttribute", ["ReferenceKey", "IsActive"]);
window.Helper.Database.addIndex("CrmService_InstallationCompanyRelationship", ["ChildId"]);
window.Helper.Database.addIndex("CrmService_InstallationPersonRelationship", ["ChildId"]);

window.Helper.Database.addIndex("Crm_FileResource", ["ParentId", "IsActive"]);
window.Helper.Database.addIndex("Crm_LinkResource", ["ParentId"]);


window.Helper.Database.setTransactionId("CrmService_Installation", async function(installation: Crm.Service.Rest.Model.CrmService_Installation) {
	return installation.Id;
});
window.Helper.Database.setTransactionId("CrmService_InstallationAddressRelationship", async function(installationAddressRelationship: Crm.Service.Rest.Model.CrmService_InstallationAddressRelationship) {
	return [installationAddressRelationship.ChildId, installationAddressRelationship.ParentId];
});
window.Helper.Database.setTransactionId("CrmService_MaintenancePlan",
	async function (maintenancePlan: Crm.Service.Rest.Model.CrmService_MaintenancePlan) {
		return [maintenancePlan.Id, maintenancePlan.ServiceContractId, maintenancePlan.ServiceOrderTemplateId];
	});
window.Helper.Database.setTransactionId("CrmService_InstallationCompanyRelationship",
	function (relationship : Crm.Service.Rest.Model.CrmService_InstallationCompanyRelationship) {
		return Promise.resolve([relationship.ParentId, relationship.ChildId]);
	});
window.Helper.Database.setTransactionId("CrmService_InstallationPersonRelationship",
	function (relationship : Crm.Service.Rest.Model.CrmService_InstallationPersonRelationship) {
		return Promise.resolve([relationship.ParentId, relationship.ChildId]);
	});
window.Helper.Database.setTransactionId("CrmService_ReplenishmentOrder",
	async function(replenishmentOrder: Crm.Service.Rest.Model.CrmService_ReplenishmentOrder) {
		return replenishmentOrder.Id;
	});
window.Helper.Database.setTransactionId("CrmService_ReplenishmentOrderItem",
	async function(replenishmentOrderItem: Crm.Service.Rest.Model.CrmService_ReplenishmentOrderItem) {
		return replenishmentOrderItem.ReplenishmentOrderId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceCaseTemplate",
	async function (serviceCaseTemplate: Crm.Service.Rest.Model.CrmService_ServiceCaseTemplate) {
		return serviceCaseTemplate.Id;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceCase",
	async function (serviceCase: Crm.Service.Rest.Model.CrmService_ServiceCase) {
		return [serviceCase.ServiceCaseTemplateId,
			serviceCase.ServiceObjectId,
			serviceCase.AffectedCompanyKey,
			serviceCase.ContactPersonId,
			serviceCase.ServiceOrderTimeId];
	});
window.Helper.Database.setTransactionId("CrmService_ServiceContractAddressRelationship", async function (serviceContractAddressRelationship: Crm.Service.Rest.Model.CrmService_ServiceContractAddressRelationship) {
	return [serviceContractAddressRelationship.ChildId, serviceContractAddressRelationship.ParentId];
});
window.Helper.Database.setTransactionId("CrmService_ServiceContractInstallationRelationship", async function (serviceContractInstallationRelationship: Crm.Service.Rest.Model.CrmService_ServiceContractInstallationRelationship) {
	return [serviceContractInstallationRelationship.ChildId, serviceContractInstallationRelationship.ParentId];
});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderDispatch",
	async function(serviceOrderDispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch) {
		return serviceOrderDispatch.OrderId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderDispatch",
	async function(serviceOrderDispatch: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatch) {
		let serviceOrder = await window.database.CrmService_ServiceOrderHead.find(serviceOrderDispatch.OrderId);
		return serviceOrder.InstallationId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderDispatchReportRecipient",
	async function(serviceOrderDispatchReportRecipient: Crm.Service.Rest.Model.CrmService_ServiceOrderDispatchReportRecipient) {
		return serviceOrderDispatchReportRecipient.DispatchId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderHead",
	async function(serviceOrderHead: Crm.Service.Rest.Model.CrmService_ServiceOrderHead) {
		return serviceOrderHead.Id;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderHead",
	async function(serviceOrderHead: Crm.Service.Rest.Model.CrmService_ServiceOrderHead) {
		return [
			serviceOrderHead.ServiceOrderTemplateId,
			serviceOrderHead.ServiceObjectId,
			serviceOrderHead.InitiatorId,
			serviceOrderHead.InitiatorPersonId,
			serviceOrderHead.InstallationId,
			serviceOrderHead.CustomerContactId
		];
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderMaterial",
	async function(serviceOrderMaterial: Crm.Service.Rest.Model.CrmService_ServiceOrderMaterial) {
		return serviceOrderMaterial.OrderId;
	});

window.Helper.Database.setTransactionId("CrmService_ServiceOrderTime",
	async function (serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime) {
		return serviceOrderTime.Id;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderTime",
	async function (serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime) {
		return serviceOrderTime.InstallationId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderTime",
	async function (serviceOrderTime: Crm.Service.Rest.Model.CrmService_ServiceOrderTime) {
		return serviceOrderTime.OrderId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderTimePosting",
	async function(serviceOrderTimePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting) {
		return serviceOrderTimePosting.OrderId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderTimePosting",
	async function (timePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderTimePosting) {
		return timePosting.PerDiemReportId;
	});
window.Helper.Database.setTransactionId("CrmService_ServiceOrderExpensePosting",
	async function (serviceOrderExpensePosting: Crm.Service.Rest.Model.CrmService_ServiceOrderExpensePosting) {
		return [
			serviceOrderExpensePosting.FileResourceKey,
			serviceOrderExpensePosting.OrderId,
			serviceOrderExpensePosting.PerDiemReportId
		];
	});
window.Helper.Database.setTransactionId("CrmService_ServiceContract",
	async function (serviceContract: Crm.Service.Rest.Model.CrmService_ServiceContract) {
		return [
			serviceContract.ParentId,
			serviceContract.PayerId,
			serviceContract.InvoiceRecipientId,
			serviceContract.ServiceObjectId];
	});
window.Helper.Database.setTransactionId("CrmService_Installation",
	async function (installation: Crm.Service.Rest.Model.CrmService_Installation) {
		return [installation.LocationContactId, installation.LocationAddressKey, installation.LocationPersonId, installation.FolderId];
	});