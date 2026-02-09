namespace Crm.Service.Rest.Model.Lookups
{
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model.Lookup;

	[RestTypeFor(DomainType = typeof(ErrorCode))]
	public class ErrorCodeRest : RestEntityLookupWithExtensionValues
	{
		[RestrictedField]
		public int ErrorCodeId { get; set; }
		[RestrictedField]
		public string Description { get; set; }
		[RestrictedField]
		public string Remark { get; set; }
		[RestrictedField]
		public int Priority { get; set; }
		[RestrictedField]
		public int? Component { get; set; }
		[RestrictedField]
		public int? QualityPlanId { get; set; }
		[RestrictedField]
		public int? BPChecklistId { get; set; }
		[RestrictedField]
		public string MonitoringCode { get; set; }
		[RestrictedField]
		public string InstallationType { get; set; }
		[RestrictedField]
		public string RdsPpClassification { get; set; }
		[RestrictedField]
		public string StandardAction { get; set; }
		[RestrictedField]
		public int? StandardActionExecuteCount { get; set; }
		[RestrictedField]
		public string StandardActionExecuteTimeout { get; set; }
		[RestrictedField]
		public string TemplateOrderNo { get; set; }
		[RestrictedField]
		public int? ReactionTime { get; set; }
		[RestrictedField]
		public string ReactionTimeType { get; set; }
		[RestrictedField]
		public string Code { get; set; }
		[RestrictedField]
		public int CountOfNotifications { get; set; }
	}
}
