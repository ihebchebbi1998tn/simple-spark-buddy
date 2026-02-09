namespace Crm.Service.Model.Lookup
{
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderType]", "ServiceOrderType")]
	public class ServiceOrderType : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string NumberingSequence { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual bool MaintenanceOrder { get; set; }
		[LookupProperty(Shared = true)]
		public virtual bool ShowInMobileClient { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string DefaultInvoicingTypeKey { get; set; }

		public ServiceOrderType()
		{
			NumberingSequence = "SMS.ServiceOrderHead.ServiceOrder";
		}
	}

	// Extension methods
	public static class ServiceOrderTypeExtensions
	{
		public static bool IsMaintenance(this ServiceOrderType serviceOrderType)
		{
			return serviceOrderType.MaintenanceOrder;
		}
	}
}
