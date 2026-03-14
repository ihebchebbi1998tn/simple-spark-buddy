namespace Main.Model.Lookups
{
	using System;

	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[UserStatus]")]
	public class UserStatus : EntityLookup<string>
	{
		public static readonly UserStatus None = new UserStatus { Key = String.Empty, Value = "None" };
	}
}
