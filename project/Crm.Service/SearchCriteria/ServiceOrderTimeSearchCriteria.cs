namespace Crm.Service.SearchCriteria
{
	using System;

	public class ServiceOrderTimeSearchCriteria
	{
		public Guid Id { get; set; }
		public Guid? OrderId { get; set; }
		public string PosNo { get; set; }
		public string ItemNo { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public float? EstimatedDuration { get; set; }
		public float? ActualDuration { get; set; }
		public float? InvoiceDuration { get; set; }
		public Decimal? Price { get; set; }
		public Decimal? TotalValue { get; set; }
		public float? DiscountPercent { get; set; }
		public float? DiscountCurrency { get; set; }
		public DateTime? TransferDate { get; set; }
		public bool CreatedLocal { get; set; }
		public bool HasTool { get; set; }
		public Guid? InstallationPosId { get; set; }
		public bool HasMaterialAllocated { get; set; }
		public int DocsCount { get; set; }
		public string ItemDescription { get; set; }
		public bool? IsExported { get; set; }

		public string SortBy { get; set; }
		public string SortOrder { get; set; }
	}
}