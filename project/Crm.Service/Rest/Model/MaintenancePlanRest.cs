namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(MaintenancePlan))]
	public class MaintenancePlanRest : ContactRest
	{
		public virtual DateTime? FirstDate { get; set; }
		public virtual DateTime? NextDate { get; set; }
		public virtual int? RhythmValue { get; set; }
		public virtual string RhythmUnitKey { get; set; }
		public bool GenerateMaintenanceOrders { get; set; }
		public bool AllowPrematureMaintenance { get; set; }
		public virtual Guid ServiceContractId { get; set; }

		[NavigationProperty(nameof(ServiceContractId), nameof(ServiceContractRest.MaintenancePlans))]
		public virtual ServiceContractRest ServiceContract { get; set; }
		public Guid? ServiceOrderTemplateId { get; set; }
		[NavigationProperty(nameof(ServiceOrderTemplateId))] public ServiceOrderHeadRest ServiceOrderTemplate { get; set; }
		[NavigationProperty(nameof(ServiceOrderHeadRest.MaintenancePlanId))] public virtual ServiceOrderHeadRest[] ServiceOrders { get; set; }
	}
}
