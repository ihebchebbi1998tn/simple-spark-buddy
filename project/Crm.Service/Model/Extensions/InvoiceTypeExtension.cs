namespace Crm.Service.Model.Extensions
{
	using Crm.Library.BaseModel;
	using Crm.Library.Globalization.Lookup;

	using Main.Model.Lookups;

	public class InvoicingTypeExtension : EntityExtension<InvoicingType>
	{
		[LookupProperty(Shared = true)] public bool IsCostLumpSum { get; set; }
		[LookupProperty(Shared = true)] public bool IsMaterialLumpSum { get; set; }
		[LookupProperty(Shared = true)] public bool IsTimeLumpSum { get; set; }
	}
}
