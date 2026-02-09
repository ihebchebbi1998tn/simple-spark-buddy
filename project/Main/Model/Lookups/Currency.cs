namespace Main.Model.Lookups
{
	using System;

	using Crm.Library.Extensions;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[Currency]", "ProjectCategoriyId")]
	public class Currency : EntityLookup<string>
	{
		public static readonly string EuroKey = "EUR";
		public static readonly string DollarKey = "USD";
		public static readonly string PoundKey = "GBP";
		public static Func<ILookup, string> DefaultValueTemplate = c => "{0} - {1}".WithArgs(c.Value, c.Key);
	}
}
