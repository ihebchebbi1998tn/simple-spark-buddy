namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ServiceOrderType))]
	public class ServiceOrderTypeRest : RestEntityLookupWithExtensionValues
	{
		public string Color { get; set; } = "#9E9E9E";
		public string NumberingSequence { get; set; } = "SMS.ServiceOrderHead.ServiceOrder";
		public bool MaintenanceOrder { get; set; }
		public bool ShowInMobileClient { get; set; }
		public string DefaultInvoicingTypeKey { get; set; }
	}
}
