namespace Crm.Services.Interfaces
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.AutoFac;
	using Crm.Library.Model;

	public interface IFileResourceReferenceSyncService : IDependency
	{
		IQueryable<Guid> GetAllFileResourceIds(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds);
	}
}
