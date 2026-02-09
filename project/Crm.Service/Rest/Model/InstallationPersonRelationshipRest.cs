namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;
	using Crm.Service.Model.Relationships;

	[RestTypeFor(DomainType = typeof(InstallationPersonRelationship))]
	public class InstallationPersonRelationshipRest : RestEntityWithExtensionValues
	{
		public Guid ChildId { get; set; }
		public string Information { get; set; }
		public Guid ParentId { get; set; }
		public string RelationshipTypeKey { get; set; }
		[NavigationProperty(nameof(ChildId))] 
		public PersonRest Child { get; set; }
		[NavigationProperty(nameof(ParentId))] 
		public InstallationRest Parent { get; set; }
	}
}
