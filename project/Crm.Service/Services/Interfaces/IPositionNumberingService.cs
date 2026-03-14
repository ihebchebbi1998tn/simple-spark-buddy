namespace Crm.Service.Services.Interfaces
{
	using Crm.Library.AutoFac;
	using System;

	public interface IPositionNumberingService : IDependency
	{
		string GetNextPositionNumber(Guid orderId);
	}
}