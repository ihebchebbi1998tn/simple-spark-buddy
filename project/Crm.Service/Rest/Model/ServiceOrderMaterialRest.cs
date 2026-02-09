namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Model.Enums;
	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderMaterial))]
	public class ServiceOrderMaterialRest : RestEntityWithExtensionValues
	{
		public string PosNo { get; set; }
		public string ItemNo { get; set; }
		public string Description { get; set; }
		public string ArticleTypeKey { get; set; }
		public string ExternalRemark { get; set; }
		public string InternalRemark { get; set; }
		public bool IsSerial { get; set; }
		[NavigationProperty(nameof(ParentServiceOrderMaterialId), InverseProperty = nameof(ChildServiceOrderMaterials))]
		public ServiceOrderMaterialRest ParentServiceOrderMaterial { get; set; }
		public Guid? ParentServiceOrderMaterialId { get; set; }
		public long? ParentServiceOrderMaterialVersion { get; set; }
		public decimal Quantity { get; set; }
		public string QuantityUnitKey { get; set; }
		public decimal? Price { get; set; }
		public string ServiceOrderMaterialType { get; set; }
		public decimal? TotalValue { get; set; }
		public decimal Discount { get; set; }
		public DiscountType DiscountType { get; set; }
		public string CommissioningStatusKey { get; set; }
		public Guid? DispatchId { get; set; }
		[NavigationProperty(nameof(DispatchId))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }
		public Guid OrderId { get; set; }

		[NavigationProperty(nameof(OrderId), nameof(ServiceOrderHeadRest.ServiceOrderMaterials))]
		public ServiceOrderHeadRest ServiceOrderHead { get; set; }

		public bool SignedByCustomer { get; set; }
		public string FromWarehouse { get; set; }
		public string FromStorageArea { get; set; }
		public string FromLocation { get; set; }
		public string ToWarehouse { get; set; }
		public string ToLocation { get; set; }
		public virtual string SerialNo { get; set; }
		public string PreviousSerialNo { get; set; }
		public Guid? SerialId { get; set; }
		[NavigationProperty(nameof(SerialId))]
		public SerialRest Serial { get; set; }
		public string NoPreviousSerialNoReasonKey { get; set; }
		public bool? IsInstalled { get; set; }
		public Guid? ArticleId { get; set; }
		public string BatchNo { get; set; }
		public bool IsBatch { get; set; }
		[NavigationProperty(nameof(ArticleId))]
		public ArticleRest Article { get; set; }

		public Guid? ReplenishmentOrderItemId { get; set; }

		[NavigationProperty(nameof(ReplenishmentOrderItemId), nameof(ReplenishmentOrderItemRest.ServiceOrderMaterials))]
		public ReplenishmentOrderItemRest ReplenishmentOrderItem { get; set; }
		public Guid? ServiceOrderTimeId { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeId), nameof(ServiceOrderTimeRest.ServiceOrderMaterials))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }

		public Guid? QuantityUnitEntryKey { get; set; }

		[NavigationProperty(nameof(QuantityUnitEntryKey))]
		public QuantityUnitEntryRest QuantityUnitEntry { get; set; }

		[NotReceived]
		public long Version { get; set; }

		[NavigationProperty(nameof(ParentServiceOrderMaterialId), nameof(ParentServiceOrderMaterial))]
		public ServiceOrderMaterialRest[] ChildServiceOrderMaterials { get; set; }

		[NavigationProperty("ExtensionValues__" + nameof(DocumentAttributeExtension.ServiceOrderMaterialId))]
		public DocumentAttributeRest[] DocumentAttributes { get; set; }
	}
}
