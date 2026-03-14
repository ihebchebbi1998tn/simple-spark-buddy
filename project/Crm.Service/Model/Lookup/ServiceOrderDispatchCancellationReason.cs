namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceOrderDispatchCancellationReason]")]
	public class ServiceOrderDispatchCancellationReason : EntityLookup<string>
	{
		public const string BadWeather = "BadWeather";
		public const string Customer = "Customer";
		public const string ContactPerson = "ContactPerson";
		public const string Other = "Other";
	}
}
