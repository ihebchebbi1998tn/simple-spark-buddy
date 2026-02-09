namespace Main.Model.Lookups
{
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[ReplicationGroup]")]
	[NotEditable]
	public class ReplicationGroup : EntityLookup<string>
	{
		[LookupProperty(Shared = false)]
		public virtual int? DefaultValue { get; set; }
		[LookupProperty(Shared = false)]
		public virtual string Description { get; set; }
		[LookupProperty(Shared = true)]
		public virtual bool HasParameter { get; set; }
		[LookupProperty(Shared = true)]
		public virtual string TableName { get; set; }
	}
}
