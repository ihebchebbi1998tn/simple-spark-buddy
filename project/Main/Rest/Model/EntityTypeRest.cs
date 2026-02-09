namespace Main.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;

	using LMobile.Unicore;

	[RestrictedType]
	[RestTypeFor(DomainType = typeof(EntityType))]
	public class EntityTypeRest : IRestEntity, IRestEntityWithExtensionValues
	{
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		public DateTime ModifiedAt { get; set; }
		public string Name { get; set; }
		public bool IsDeleted { get; set; }
		public Guid UId { get; set; }
	}
}
