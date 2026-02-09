namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Service.Model;

	[RestrictedType]
	[RestTypeFor(DomainType = typeof(RdsPpStructure))]
	public class RdsPpStructureRest : IRestEntityWithExtensionValues, IRestEntity, IAuthorisedRestEntity
	{
		public Guid Id { get; set; }
		public SerializableDictionary<string, object> ExtensionValues { get; set; }
		[NotReceived]
		public DateTime CreateDate { get; set; }
		[NotReceived]
		public DateTime ModifyDate { get; set; }
		[NotReceived]
		public string CreateUser { get; set; }
		[NotReceived]
		public string ModifyUser { get; set; }
		[RestrictedField]
		public Guid? DomainId { get; set; }
		public Guid? ParentRdsPpStructureKey { get; set; }
		public string RdsPpClassification { get; set; }
		public string Description { get; set; }
	}
}
