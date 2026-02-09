namespace Main.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	using LMobile.Unicore;

	[RestTypeFor(DomainType = typeof(PermissionSchemaRole))]
	public class PermissionSchemaRoleRest : IRestEntity, IRestEntityWithExtensionValues
	{
		public string CompilationName { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		public bool FromConfiguration { get; set; }
		public string Group { get; set; }
		public bool IgnoreCircles { get; set; }
		public DateTime ModifiedAt { get; set; }
		public string Name { get; set; }
		public Guid PermissionSchemaId { get; set; }
		public Guid? SourcePermissionSchemaRoleId { get; set; }
		[NavigationProperty(nameof(SourcePermissionSchemaRoleId))] public PermissionSchemaRoleRest SourcePermissionSchemaRole { get; set; }
		public Guid UId { get; set; }
	}
}
