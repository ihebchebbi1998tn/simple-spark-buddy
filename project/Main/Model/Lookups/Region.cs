namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[Region]")]
	public class Region : EntityLookup<string>
	{
		public static readonly Region None = new Region { Key = string.Empty, Value = "None" };
	}
}
