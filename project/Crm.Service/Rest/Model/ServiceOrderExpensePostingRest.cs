namespace Crm.Service.Rest.Model
{
	using System;

	using Crm.Article.Rest.Model;
	using Crm.Library.Api.Attributes;
	using Crm.Library.Rest;
		using Crm.PerDiem.Rest.Model;
	using Crm.Rest.Model;
	using Crm.Service.Model;

	[RestTypeFor(DomainType = typeof(ServiceOrderExpensePosting))]
	public class ServiceOrderExpensePostingRest : ExpenseRest
	{

		public virtual Guid OrderId { get; set; }
		public virtual Boolean IsExported { get; set; }
		public Guid? ArticleId { get; set; }
		public virtual string Description { get; set; }
		public virtual string ExpenseTypeKey { get; set; }
		public virtual Guid? FileResourceKey { get; set; }
		public string ItemNo { get; set; }
		public Guid? DispatchId { get; set; }
		public Guid? ServiceOrderTimeId { get; set; }


		[NavigationProperty(nameof(ArticleId))]
		public ArticleRest Article { get; set; }

		[NavigationProperty(nameof(DispatchId), nameof(ServiceOrderDispatchRest.ServiceOrderExpensePostings))]
		public ServiceOrderDispatchRest ServiceOrderDispatch { get; set; }

		[NavigationProperty(nameof(ServiceOrderTimeId), nameof(ServiceOrderTimeRest.ExpensePostings))]
		public ServiceOrderTimeRest ServiceOrderTime { get; set; }

		[NavigationProperty(nameof(OrderId), nameof(ServiceOrderHeadRest.ServiceOrderExpensePostings))]
		public ServiceOrderHeadRest ServiceOrder { get; set; }

		[NavigationProperty(nameof(FileResourceKey))] 
		public virtual FileResourceRest FileResource { get; set; }


	}
}
