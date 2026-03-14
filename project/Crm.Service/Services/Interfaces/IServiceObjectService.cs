using Crm.Library.AutoFac;
using System.Collections.Generic;

namespace Crm.Service.Services.Interfaces
{
	using System;

	using Crm.Service.Model;

	public interface IServiceObjectService : IDependency
	{
		IEnumerable<string> GetUsedServiceObjectCategories();
		ServiceObject GetServiceObject(Guid serviceObjectId);
	}
}
