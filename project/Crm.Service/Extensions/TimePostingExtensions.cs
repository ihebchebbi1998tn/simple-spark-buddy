namespace Crm.Service.Extensions
{
	using Crm.Service.Model;

	public static class TimePostingExtensions
	{
		public static bool IsPrePlanned(this ServiceOrderTimePosting entity) => entity.ServiceOrderTimePostingType == "Preplanned";
		public static bool IsInvoiced(this ServiceOrderTimePosting entity) => entity.ServiceOrderTimePostingType == "Invoice";

	}
}
