namespace Main.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	using LMobile.Unicore;

	[RestrictedType]
	[RestTypeFor(DomainType = typeof(Permission))]
	public class PermissionRest : IRestEntity, IRestEntityWithExtensionValues
	{
		public CircleConstraint CircleConstraint { get; set; }
		public string CompilationName { get; set; }
		public DomainAccessMergeType DomainAccessMergeType { get; set; }
		public DomainAccessOptions DomainAccessOptions { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		public string Group { get; set; }
		public string[] ImportedPermissions { get; set; }
		public DateTime ModifiedAt { get; set; }
		public string Name { get; set; }
		public string SchemaUsage { get; set; }
		public Guid UId { get; set; }
	}
}
