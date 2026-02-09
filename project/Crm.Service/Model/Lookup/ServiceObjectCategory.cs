namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ServiceObjectCategory]")]
	public class ServiceObjectCategory : EntityLookup<string>, ILookupWithColor
	{
		[LookupProperty(Shared = true)]
		public virtual string Color { get; set; } = "#9E9E9E";
	}
}
