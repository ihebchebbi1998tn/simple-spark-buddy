namespace Crm.Service.Services
{
	using System;
	using System.Linq;

	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Model;
	using Crm.Service.Enums;
	using Crm.Service.Extensions;
	using Crm.Service.Model;
	using Crm.Service.Model.Relationships;
	using Crm.Service.Services.Interfaces;

	public class DefaultServiceOrderTemplateService : IServiceOrderTemplateService
	{
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly Func<ServiceOrderTime> serviceOrderTimeFactory;
		private readonly Func<ServiceOrderMaterial> serviceOrderMaterialFactory;
		private readonly Func<ServiceOrderTimePosting> serviceOrderTimePostingFactory;
		private readonly Func<DocumentAttribute> documentAttributeFactory;
		private readonly ILookupManager lookupManager;

		public DefaultServiceOrderTemplateService(
			IAppSettingsProvider appSettingsProvider,
			Func<ServiceOrderTime> serviceOrderTimeFactory,
			Func<ServiceOrderMaterial> serviceOrderMaterialFactory,
			Func<ServiceOrderTimePosting> serviceOrderTimePostingFactory,
			Func<DocumentAttribute> documentAttributeFactory,
			ILookupManager lookupManager)
		{
			this.appSettingsProvider = appSettingsProvider;
			this.serviceOrderTimeFactory = serviceOrderTimeFactory;
			this.serviceOrderMaterialFactory = serviceOrderMaterialFactory;
			this.serviceOrderTimePostingFactory = serviceOrderTimePostingFactory;
			this.documentAttributeFactory = documentAttributeFactory;
			this.lookupManager = lookupManager;
		}

		public virtual int Priority => 100;

		public virtual DocumentAttribute CreateDocumentAttributeFromTemplate(DocumentAttribute documentAttributeTemplate)
		{
			var documentAttribute = documentAttributeFactory();
			documentAttribute.Description = documentAttributeTemplate.Description;
			documentAttribute.DocumentCategoryKey = documentAttributeTemplate.DocumentCategoryKey;
			documentAttribute.ExtensionValues = documentAttributeTemplate.ExtensionValues;
			documentAttribute.FileName = documentAttributeTemplate.FileName;
			documentAttribute.FileResourceKey = documentAttributeTemplate.FileResourceKey;
			documentAttribute.Id = Guid.NewGuid();
			documentAttribute.Length = documentAttributeTemplate.Length;
			documentAttribute.LongText = documentAttributeTemplate.LongText;
			documentAttribute.OfflineRelevant = documentAttributeTemplate.OfflineRelevant;
			documentAttribute.UseForThumbnail = false;
			return documentAttribute;
		}

		public virtual ServiceOrderMaterial CreateServiceOrderMaterialFromTemplate(ServiceOrderMaterial serviceOrderMaterialTemplate)
		{
			var serviceOrderMaterial = serviceOrderMaterialFactory();
			serviceOrderMaterial.ArticleId = serviceOrderMaterialTemplate.ArticleId;
			serviceOrderMaterial.Description = serviceOrderMaterialTemplate.Description;
			serviceOrderMaterial.ExtensionValues = serviceOrderMaterialTemplate.ExtensionValues;
			serviceOrderMaterial.ExternalRemark = serviceOrderMaterialTemplate.ExternalRemark;
			serviceOrderMaterial.Id = Guid.NewGuid();
			serviceOrderMaterial.InternalRemark = serviceOrderMaterialTemplate.InternalRemark;
			serviceOrderMaterial.IsSerial = serviceOrderMaterialTemplate.IsSerial;
			serviceOrderMaterial.ItemNo = serviceOrderMaterialTemplate.ItemNo;
			serviceOrderMaterial.PosNo = serviceOrderMaterialTemplate.PosNo;
			serviceOrderMaterial.Price = serviceOrderMaterialTemplate.Price;
			serviceOrderMaterial.Quantity = serviceOrderMaterialTemplate.Quantity;
			serviceOrderMaterial.QuantityUnitKey = serviceOrderMaterialTemplate.QuantityUnitKey;
			serviceOrderMaterial.ServiceOrderMaterialType = serviceOrderMaterialTemplate.ServiceOrderMaterialType;
			return serviceOrderMaterial;
		}

		public virtual ServiceOrderTimePosting CreateServiceOrderTimePostingFromTemplate(ServiceOrderTimePosting serviceOrderTimePostingTemplate)
		{
			var serviceOrderTimePosting = serviceOrderTimePostingFactory();
			serviceOrderTimePosting.ArticleId = serviceOrderTimePostingTemplate.ArticleId;
			serviceOrderTimePosting.Description = serviceOrderTimePostingTemplate.Description;
			serviceOrderTimePosting.DurationInMinutes = serviceOrderTimePostingTemplate.DurationInMinutes;
			serviceOrderTimePosting.ExtensionValues = serviceOrderTimePostingTemplate.ExtensionValues;
			serviceOrderTimePosting.Id = Guid.NewGuid();
			serviceOrderTimePosting.InternalRemark = serviceOrderTimePostingTemplate.InternalRemark;
			serviceOrderTimePosting.ItemNo = serviceOrderTimePostingTemplate.ItemNo;
			serviceOrderTimePosting.ServiceOrderTimePostingType = serviceOrderTimePostingTemplate.ServiceOrderTimePostingType;
			return serviceOrderTimePosting;
		}
		public virtual string GetNextJobPosNo(ServiceOrderHead serviceOrder)
		{
			var posNo = 0;
			var results = serviceOrder.ServiceOrderTimes
			.OrderByDescending(it => it.PosNo)
			.Take(1)
			.ToList();
			if (results.Count > 0)
			{
				posNo = Math.Max(posNo, int.Parse(results[0].PosNo));
			}

			return FormatPosNo(posNo + 1);
		}
		public virtual string FormatPosNo(int posNo)
		{
			return posNo < 10000
			? posNo.ToString("D4")
			: posNo.ToString();
		}
		public virtual ServiceOrderTime CreateServiceOrderTimeFromTemplate(ServiceOrderTime serviceOrderTimeTemplate, ServiceOrderHead serviceOrder)
		{
			var serviceOrderTime = serviceOrderTimeFactory();
			serviceOrderTime.ArticleId = serviceOrderTimeTemplate.ArticleId;
			serviceOrderTime.Comment = serviceOrderTimeTemplate.Comment;
			serviceOrderTime.Description = serviceOrderTimeTemplate.Description;
			serviceOrderTime.EstimatedDuration = serviceOrderTimeTemplate.EstimatedDuration;
			serviceOrderTime.ExtensionValues = serviceOrderTimeTemplate.ExtensionValues;
			serviceOrderTime.Id = Guid.NewGuid();
			serviceOrderTime.ItemNo = serviceOrderTimeTemplate.ItemNo;
			serviceOrderTime.PosNo = GetNextJobPosNo(serviceOrder);
			serviceOrderTime.InvoicingTypeKey = serviceOrderTimeTemplate.InvoicingTypeKey;
			serviceOrderTime.TrySetLumpSumData(lookupManager);
			return serviceOrderTime;
		}

		public virtual void CreateTemplateData(ServiceOrderHead serviceOrder, ServiceOrderHead serviceOrderTemplate, Installation installation, ServiceContractInstallationRelationship relationship = null)
		{
			var maintenanceOrderGenerationMode = appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceContract.MaintenanceOrderGenerationMode);

			var serviceOrderTimeTemplates = serviceOrderTemplate.ServiceOrderTimes;
			foreach (var serviceOrderTimeTemplate in serviceOrderTimeTemplates)
			{
				if (installation == null && maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
				{
					continue;
				}

				var serviceOrderTime = CreateServiceOrderTimeFromTemplate(serviceOrderTimeTemplate, serviceOrder);
				serviceOrderTime.OrderId = serviceOrder.Id;
				if (relationship != null)
				{
					serviceOrderTime.EstimatedDuration = (float?)relationship.TimeAllocation?.TotalHours;
				}
				if (installation != null && maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.JobPerInstallation)
				{
					serviceOrderTime.InstallationId = installation.Id;
				}

				serviceOrder.ServiceOrderTimes.Add(serviceOrderTime);

				foreach (var serviceOrderMaterialTemplate in serviceOrderTemplate.ServiceOrderMaterials.Where(x => x.ServiceOrderTimeId == serviceOrderTimeTemplate.Id))
				{
					var serviceOrderMaterial = CreateServiceOrderMaterialFromTemplate(serviceOrderMaterialTemplate);
					serviceOrderMaterial.OrderId = serviceOrder.Id;
					serviceOrderMaterial.ServiceOrderTimeId = serviceOrderTime.Id;
					serviceOrder.ServiceOrderMaterials.Add(serviceOrderMaterial);
				}
				foreach (var serviceOrderTimePostingTemplate in serviceOrderTemplate.ServiceOrderTimePostings.Where(x => x.OrderTimesId == serviceOrderTimeTemplate.Id))
				{
					var serviceOrderTimePosting = CreateServiceOrderTimePostingFromTemplate(serviceOrderTimePostingTemplate);
					serviceOrderTimePosting.OrderId = serviceOrder.Id;
					serviceOrderTimePosting.OrderTimesId = serviceOrderTime.Id;
					serviceOrder.ServiceOrderTimePostings.Add(serviceOrderTimePosting);
				}
			}

			if (installation == null)
			{
				foreach (var serviceOrderMaterialTemplate in serviceOrderTemplate.ServiceOrderMaterials.Where(x => x.ServiceOrderTimeId == null))
				{
					var serviceOrderMaterial = CreateServiceOrderMaterialFromTemplate(serviceOrderMaterialTemplate);
					serviceOrderMaterial.OrderId = serviceOrder.Id;
					serviceOrder.ServiceOrderMaterials.Add(serviceOrderMaterial);
				}

				foreach (var serviceOrderTimePostingTemplate in serviceOrderTemplate.ServiceOrderTimePostings.Where(x => x.OrderTimesId == null))
				{
					var serviceOrderTimePosting = CreateServiceOrderTimePostingFromTemplate(serviceOrderTimePostingTemplate);
					serviceOrderTimePosting.OrderId = serviceOrder.Id;
					serviceOrder.ServiceOrderTimePostings.Add(serviceOrderTimePosting);
				}
			}

			if (maintenanceOrderGenerationMode == MaintenanceOrderGenerationMode.OrderPerInstallation || installation == null)
			{
				foreach (var documentAttributeTemplate in serviceOrderTemplate.DocumentAttributes)
				{
					var documentAttribute = CreateDocumentAttributeFromTemplate(documentAttributeTemplate);
					documentAttribute.ReferenceKey = serviceOrder.Id;
					serviceOrder.DocumentAttributes.Add(documentAttribute);
				}
			}
		}
	}
}
