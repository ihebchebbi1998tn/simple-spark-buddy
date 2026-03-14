namespace Crm.Service.Model.Lookup
{
	using Crm.Library.BaseModel.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[LU].[StatisticsKeyProductType]")]
	public class StatisticsKeyProductType : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyMainAssembly]")]
	public class StatisticsKeyMainAssembly : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeySubAssembly]")]
	public class StatisticsKeySubAssembly : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string MainAssemblyKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyAssemblyGroup]")]
	public class StatisticsKeyAssemblyGroup : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string MainAssemblyKey { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string SubAssemblyKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyFaultImage]")]
	public class StatisticsKeyFaultImage : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string AssemblyGroupKey { get; set; }

		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string InstallationTypeKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyRemedy]")]
	public class StatisticsKeyRemedy : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyCause]")]
	public class StatisticsKeyCause : EntityLookup<string>
	{
		[LookupProperty(Shared = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }

		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ErrorTypes { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyWeighting]")]
	public class StatisticsKeyWeighting : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
	}

	[Lookup("[LU].[StatisticsKeyCauser]")]
	public class StatisticsKeyCauser : EntityLookup<string>
	{
		[LookupProperty(Shared = true, Required = true)]
		public virtual string Code { get; set; }
		[LookupProperty(Shared = true)]
		[UI(Hidden = true)]
		public virtual string ProductTypeKey { get; set; }
	}
}
