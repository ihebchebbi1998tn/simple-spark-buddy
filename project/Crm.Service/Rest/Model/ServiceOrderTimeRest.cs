namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Model.Enums;
	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderTime))]
	public class ServiceOrderTimeRest : RestEntityWithExtensionValues
	{
		public Guid? ArticleId { get; set; }
		public DateTime? CompleteDate { get; set; }
		public string TimeZone { get; set; }
		public string CompleteUser { get; set; }
		public Guid OrderId { get; set; }
		public string PosNo { get; set; }
		public string StatusKey { get; set; }
		public string ItemNo { get; set; }
		public string Description { get; set; }
		public string Comment { get; set; }
		public decimal? Price { get; set; }
		public decimal? TotalValue { get; set; }
		public decimal Discount { get; set; }
		public DiscountType DiscountType { get; set; }
		[RestrictedField] public TimeSpan? EstimatedDuration { get; set; }
		[RestrictedField] public TimeSpan? ActualDuration { get; set; }
		[RestrictedField] public TimeSpan? InvoiceDuration { get; set; }
		public string CausingItemNo { get; set; }
		public string CausingItemSerialNo { get; set; }
		public string CausingItemPreviousSerialNo { get; set; }
		public string Diagnosis { get; set; }
		public string NoCausingItemSerialNoReasonKey { get; set; }
		public string NoCausingItemPreviousSerialNoReasonKey { get; set; }
		public Guid? InstallationId { get; set; }
		public bool IsCostLumpSum { get; set; }
		public bool IsMaterialLumpSum { get; set; }
		public bool IsTimeLumpSum { get; set; }
		public string InvoicingTypeKey { get; set; }

		[NavigationProperty(nameof(InstallationId))]
		public InstallationRest Installation { get; set; }

		[NavigationProperty(nameof(ArticleId))]
		public ArticleRest Article { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimePostingRest.ServiceOrderTimeId), nameof(ServiceOrderTimePostingRest.ServiceOrderTime))]
		public ServiceOrderTimePostingRest[] Postings { get; set; }

		[NavigationProperty(nameof(ServiceOrderExpensePostingRest.ServiceOrderTimeId), nameof(ServiceOrderExpensePostingRest.ServiceOrderTime))]
		public ServiceOrderExpensePostingRest[] ExpensePostings { get; set; }

		[NavigationProperty(nameof(ServiceCaseRest.ServiceOrderTimeId), nameof(ServiceCaseRest.ServiceOrderTime))]
		public ServiceCaseRest[] ServiceCases { get; set; }

		[NavigationProperty(nameof(ServiceOrderMaterialRest.ServiceOrderTimeId), nameof(ServiceOrderMaterialRest.ServiceOrderTime))]
		public ServiceOrderMaterialRest[] ServiceOrderMaterials { get; set; }

		[NavigationProperty(nameof(ServiceOrderErrorTypeRest.ServiceOrderTimeId), nameof(ServiceOrderErrorTypeRest.ServiceOrderTime))]
		public ServiceOrderErrorTypeRest[] ServiceOrderErrorTypes { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeDispatchRest.ServiceOrderTimeId), nameof(ServiceOrderTimeDispatchRest.ServiceOrderTime))]
		public ServiceOrderTimeDispatchRest[] ServiceOrderTimeDispatches { get; set; }
	}
}
