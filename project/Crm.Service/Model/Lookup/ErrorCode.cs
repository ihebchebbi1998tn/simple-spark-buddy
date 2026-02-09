namespace Crm.Service.Model.Lookup
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Globalization.Lookup;

	[Lookup("[SMS].[ErrorCode]")]
	public class ErrorCode : EntityLookup<string>
	{
		// Members
		public static readonly ErrorCode None = new ErrorCode { Key = null, Value = "None" };
		public virtual int ErrorCodeId { get; set; }
		public virtual string Description { get; set; }
		public virtual string Remark { get; set; }
		public virtual int Priority { get; set; }
		public virtual int? Component { get; set; }
		public virtual int? QualityPlanId { get; set; }
		public virtual int? BPChecklistId { get; set; }
		public virtual string MonitoringCode { get; set; }
		public virtual string InstallationType { get; set; }
		public virtual string RdsPpClassification { get; set; }
		public virtual string StandardAction { get; set; }
		public virtual int? StandardActionExecuteCount { get; set; }
		public virtual string StandardActionExecuteTimeout { get; set; }
		public virtual string TemplateOrderNo { get; set; }
		public virtual int? ReactionTime { get; set; }
		public virtual string ReactionTimeType { get; set; }
		public virtual string Code { get; set; }
		public virtual int CountOfNotifications { get; set; }
	}
}
