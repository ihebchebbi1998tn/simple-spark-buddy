namespace Crm.Service.SearchCriteria
{
	using System;

	public class ServiceOrderMaterialSearchCriteria
	{
		public Guid Id { get; set; }
		public string TimeEntryTypeKey { get; set; }
		public DateTime FromDate { get; set; }
		public DateTime ToDate { get; set; }
		public string Username { get; set; }
		public Guid? OrderId { get; set; }
		public string PosNo { get; set; }
		public string ItemNo { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public float EstimatedQty { get; set; }
		public float ActualQty { get; set; }
		public float InvoiceQty { get; set; }
		public string QuantityUnitKey { get; set; }
		public Decimal? Price { get; set; }
		public Decimal? TotalValue { get; set; }
		public string FromWarehouse { get; set; }
		public string FromLocation { get; set; }
		public string ToWarehouse { get; set; }
		public string ToLocation { get; set; }
		public int Status { get; set; }
		public DateTime? TransferDate { get; set; }
		public bool BuiltIn { get; set; }
		public bool IsSerial { get; set; }
		public bool CreatedLocal { get; set; }
		public int SerialCount { get; set; }
		public int DocsCount { get; set; }
		public string ItemDescription { get; set; }
		public bool? IsExported { get; set; }
		public Guid? DispatchId { get; set; }
		public Guid ParentId { get; set; }

		public string SortBy { get; set; }
		public string SortOrder { get; set; }
	}
}