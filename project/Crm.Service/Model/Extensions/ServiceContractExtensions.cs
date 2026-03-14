namespace Crm.Service.Model.Extensions
{
	using System;

	public static class ServiceContractExtensions
	{
		public static bool IsValidOnDate(this ServiceContract serviceContract, DateTime date)
		{
			return
					(!serviceContract.ValidFrom.HasValue && !serviceContract.ValidTo.HasValue) || 
					(serviceContract.ValidFrom < date && !serviceContract.ValidTo.HasValue) || 
					(serviceContract.ValidFrom < date && serviceContract.ValidTo > date);
		}
	}
}