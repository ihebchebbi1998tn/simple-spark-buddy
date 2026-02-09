 namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[Lu].[PasswordStrength]")]
	[NotEditable]
	
	public class PasswordStrength : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string Example { get; set; }
	}
}
