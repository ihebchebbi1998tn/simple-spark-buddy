namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;

	using LMobile.Unicore;

	using Microsoft.AspNetCore.OData.Query;

	using User = Crm.Library.Model.User;

	public class ODataQueryPermissionsFilter : IODataQueryFunction, IDependency
	{
		private readonly Lazy<IGrantedPermissionStore> grantedPermissionStore;
		private readonly IAccessControlManager accessControlManager;
		public ODataQueryPermissionsFilter(Lazy<IGrantedPermissionStore> grantedPermissionStore, IAccessControlManager accessControlManager)
		{
			this.grantedPermissionStore = grantedPermissionStore;
			this.accessControlManager = accessControlManager;
		}
		protected virtual IQueryable<User> FilterByPermissions(IQueryable<User> query, List<string> permissions)
		{
			var userIds = new List<Guid>();
			foreach (var permission in permissions)
			{
				userIds.AddRange(grantedPermissionStore.Value.ListAdvancedByPermission(accessControlManager.FindPermission(permission).UId).Select(x => x.UserId));
			}
			return query.Where(x => userIds.Contains(x.UserId));
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			if (typeof(User).IsAssignableFrom(typeof(T)) == false)
			{
				return query;
			}

			var parameters = options.Request.Query;
			var permissions = parameters.Keys.Where(x => x.StartsWith("filterByPermissions")).Select(x => parameters[x]).ToList();
			if (permissions.Any())
			{
				return (IQueryable<T>)FilterByPermissions((IQueryable<User>)query, permissions.Select(x => x.ToString()).ToList());
			}

			return query;
		}
	}
}
