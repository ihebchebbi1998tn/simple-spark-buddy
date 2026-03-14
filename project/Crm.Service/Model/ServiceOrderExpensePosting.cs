namespace Crm.Service.Model
{
	using System;

	using Crm.Article.Model;
	using Crm.Model;
	using Crm.PerDiem.Model;

	public class ServiceOrderExpensePosting : Expense
	{
		public virtual Guid OrderId { get; set; }
		public virtual ServiceOrderHead ServiceOrderHead { get; set; }
		public virtual Guid? ServiceOrderTimeId { get; set; }
		public virtual ServiceOrderTime ServiceOrderTime { get; set; }
		public virtual Boolean IsExported { get; set; }

		public virtual Guid? ArticleId { get; set; }
		public virtual Article Article { get; set; }

		public virtual string Description { get; set; }
		public virtual string ExpenseTypeKey { get; set; }
		public virtual Guid? FileResourceKey { get; set; }
		public virtual FileResource FileResource { get; set; }
		public virtual string ItemNo { get; set; }

		public virtual Guid? DispatchId { get; set; }
		public virtual ServiceOrderDispatch ServiceOrderDispatch { get; set; }

	}
}
