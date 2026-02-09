namespace Crm.Service.Model
{
	using System;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;

	public class ServiceOrderStatisticsKey : EntityBase<Guid>, ISoftDelete
	{
		public virtual ServiceOrderDispatch Dispatch { get; set; }
		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderHead ServiceOrder { get; set; }
		public virtual Guid ServiceOrderId { get; set; }
		public virtual string ProductTypeKey { get; set; }
		public virtual string MainAssemblyKey { get; set; }
		public virtual string SubAssemblyKey { get; set; }
		public virtual string AssemblyGroupKey { get; set; }
		public virtual string FaultImageKey { get; set; }
		public virtual string RemedyKey { get; set; }
		public virtual string CauseKey { get; set; }
		public virtual string WeightingKey { get; set; }
		public virtual string CauserKey { get; set; }
	}
}
