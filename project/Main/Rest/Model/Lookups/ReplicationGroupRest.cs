namespace Main.Rest.Model.Lookups
{
	using Crm.Library.Rest;

	using Main.Model.Lookups;

	[RestTypeFor(DomainType = typeof(ReplicationGroup))]
	public class ReplicationGroupRest : RestEntityLookupWithExtensionValues
	{
		public int? DefaultValue { get; set; }
		public string Description { get; set; }
		public bool HasParameter { get; set; }
		public string TableName { get; set; }
	}
}
