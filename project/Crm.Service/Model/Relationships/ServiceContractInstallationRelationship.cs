namespace Crm.Service.Model.Relationships
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class ServiceContractInstallationRelationship : Relationship<ServiceContract, Installation>
	{
		public virtual TimeSpan? TimeAllocation { get; set; }
	}
}
