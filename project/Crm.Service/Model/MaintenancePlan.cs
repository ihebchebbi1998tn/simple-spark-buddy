namespace Crm.Service.Model
{
	using System;
	using System.Collections.Generic;

	using Crm.Model;

	using Main.Model.Lookups;

	public class MaintenancePlan : Contact
	{
		public virtual Guid ServiceContractId { get; set; }
		public virtual ServiceContract ServiceContract { get; set; }
		public virtual Guid? ServiceOrderTemplateId { get; set; }
		public virtual ServiceOrderHead ServiceOrderTemplate { get; set; }
		public virtual DateTime? FirstDate { get; set; }
		public virtual DateTime? NextDate { get; set; }
		public virtual int? RhythmValue { get; set; }
		public virtual string RhythmUnitKey { get; set; }
		public virtual TimeUnit RhythmUnit
		{
			get { return RhythmUnitKey != null ? LookupManager.Get<TimeUnit>(RhythmUnitKey) : null; }
		}
		public virtual bool GenerateMaintenanceOrders { get; set; }
		public virtual bool AllowPrematureMaintenance { get; set; }
		public virtual ICollection<ServiceOrderHead> ServiceOrders { get; set; }
		public MaintenancePlan()
		{
			GenerateMaintenanceOrders = true;
			ServiceOrders = new List<ServiceOrderHead>();
		}
	}
}
