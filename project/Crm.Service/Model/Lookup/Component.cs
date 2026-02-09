namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[Components]")]
	public class Component : EntityLookup<string>
	{
		// Members
		public static readonly Component None = new Component { Key = null, Value = "None" };
	}
}