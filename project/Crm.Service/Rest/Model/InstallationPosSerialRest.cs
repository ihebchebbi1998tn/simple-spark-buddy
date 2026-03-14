namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.BaseModel;
	using Crm.Library.Rest;
	using Crm.Library.Rest.Interfaces;
	using Crm.Service.Model;

	[RestrictedType]
	[RestTypeFor(DomainType = typeof(InstallationPosSerial))]
	public class InstallationPosSerialRest : IRestEntityWithExtensionValues, IRestEntity, IAuthorisedRestEntity
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
		public Guid InstallationPosId { get; set; }
		public string SerialNo { get; set; }
		public bool IsInstalled { get; set; }
	}
}
