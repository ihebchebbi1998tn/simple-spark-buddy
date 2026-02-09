namespace Crm.Service.Extensions
{
	using Crm.Library.Globalization.Lookup;
	using Crm.Service.Model;
	using Crm.Service.Model.Extensions;

	using Main.Model.Lookups;

	public static class LumpSumExtensions
	{
		public static void TrySetLumpSumData(this ServiceOrderHead order, ILookupManager lookupManager)
		{
			if (order.InvoicingTypeKey != null && lookupManager.Get<InvoicingType>(order.InvoicingTypeKey)?.GetExtension<InvoicingTypeExtension>() is { } lumpSumData)
			{
				order.IsCostLumpSum = lumpSumData.IsCostLumpSum;
				order.IsMaterialLumpSum = lumpSumData.IsMaterialLumpSum;
				order.IsTimeLumpSum = lumpSumData.IsTimeLumpSum;
			}
		}
		public static void TrySetLumpSumData(this ServiceOrderTime time, ILookupManager lookupManager)
		{
			if (time.InvoicingTypeKey != null && lookupManager.Get<InvoicingType>(time.InvoicingTypeKey)?.GetExtension<InvoicingTypeExtension>() is { } lumpSumData)
			{
				time.IsCostLumpSum = lumpSumData.IsCostLumpSum;
				time.IsMaterialLumpSum = lumpSumData.IsMaterialLumpSum;
				time.IsTimeLumpSum = lumpSumData.IsTimeLumpSum;
			}
		}
		public static void TrySetLumpSumData(this ServiceOrderTime time, ServiceOrderHead order)
		{
			time.InvoicingTypeKey = order.InvoicingTypeKey;
			time.IsCostLumpSum = order.IsCostLumpSum;
			time.IsMaterialLumpSum = order.IsMaterialLumpSum;
			time.IsTimeLumpSum = order.IsTimeLumpSum;
		}
	}
}
