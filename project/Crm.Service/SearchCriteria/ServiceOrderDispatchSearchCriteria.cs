namespace Crm.Service.SearchCriteria
{
	using System;

	using Main.SearchCriteria;

	public class ServiceOrderDispatchSearchCriteria : TimeSpanSearchCriteria
	{
		//public string Query { get; set; } // mixed query for simple search
		public string SortBy { get; set; }
		public string SortOrder { get; set; }
		public Guid? OrderId { get; set; }
		public string OrderNo { get; set; }
		public string Username { get; set; }
	}
}
