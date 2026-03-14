namespace Main.Controllers.OData
{
	using Crm.Library.Model;
	using System;

	public class Transaction
	{
		public string Id { get; set; }
		public int PostingCount { get; set; }
		public PostingState TransactionState { get; set; }
		public DateTime CreateDate { get; set; }
		public string CreateUser { get; set; }
		public int Retries { get; set; }
		public DateTime? RetryAfter { get; set; }
	}
}
