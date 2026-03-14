namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;

	public interface IContactSyncService : IDependency
	{
		IQueryable<Guid> GetAllContactIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds);
	}
}
