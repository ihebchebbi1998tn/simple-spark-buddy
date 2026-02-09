namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;

	[RestTypeFor(DomainType = typeof(Service.Model.ReplenishmentOrderItem))]
	public class ReplenishmentOrderItemRest : RestEntityWithExtensionValues
	{
		public Guid? ArticleId { get; set; }
		public string MaterialNo { get; set; }
		public string Description { get; set; }
		public decimal Quantity { get; set; }
		public string QuantityUnitKey { get; set; }
		public string Remark { get; set; }
		public Guid ReplenishmentOrderId { get; set; }

		[NavigationProperty(nameof(ArticleId))]
		public ArticleRest Article { get; set; }
		public Guid? QuantityUnitEntryKey { get; set; }
		[NavigationProperty(nameof(QuantityUnitEntryKey))]
		public QuantityUnitEntryRest QuantityUnitEntry { get; set; }

		[NavigationProperty(nameof(ReplenishmentOrderId), nameof(ReplenishmentOrderRest.Items))]
		public ReplenishmentOrderRest ReplenishmentOrder { get; set; }

		[NavigationProperty(nameof(ServiceOrderMaterialRest.ReplenishmentOrderItemId), nameof(ServiceOrderMaterialRest.ReplenishmentOrderItem))]
		public ServiceOrderMaterialRest[] ServiceOrderMaterials { get; set; }
	}
}
