namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ServiceContractType]")]
	public class ServiceContractType : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; }

		public const string UnknownKey = "0";

		[LookupProperty(Shared = true, Required = true)]
		public virtual int GracePeriodInDays { get; set; }
		// Constructor
		public ServiceContractType()
		{
			Color = "#AAAAAA";
			GracePeriodInDays = 0;
		}
	}
}
