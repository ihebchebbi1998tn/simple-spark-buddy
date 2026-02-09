namespace Main.Model.Lookups
{
	using System;

	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[Country]")]
	public class Country : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string Iso2Code { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string Iso3Code { get; set; }

		[LookupProperty(Shared = true)]
		public virtual string CallingCode { get; set; }

		public virtual string NameWithCallingCode
		{
			get { return String.Format("{0} (+{1})", Value, CallingCode); }
		}

		public static readonly Country None = new Country { Key = String.Empty, Value = "None" };
	}
}
