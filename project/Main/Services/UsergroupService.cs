namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using NHibernate;
	using NHibernate.Linq;

	public class UsergroupService : IUsergroupService
	{
		private readonly ILog logger;
		private readonly IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository;

		public UsergroupService(IRepositoryWithTypedId<Usergroup, Guid> usergroupRepository, ILog logger)
		{
			this.usergroupRepository = usergroupRepository;
			this.logger = logger;
		}
		public virtual void PurgeCache()
		{
			logger.Info("Purging UsergroupCache");
			usergroupRepository.Session.SessionFactory.ClearSecondLevelCache();
		}

		public virtual IQueryable<Usergroup> GetUsergroupsQuery()
		{
			var queryable = usergroupRepository.GetAll();
			if (queryable is NhQueryable<Usergroup>)
			{
				queryable = queryable.WithOptions(o =>
				{
					o.SetCacheable(true);
					o.SetCacheMode(CacheMode.Normal);
				});
			}
			return queryable;
		}
		public virtual List<Usergroup> GetUsergroups()
		{
			return GetUsergroupsQuery().ToList();
		}
		public virtual Usergroup GetUsergroup(string usergroupName)
		{
			return GetUsergroupsQuery().FirstOrDefault(x => x.Name.ToLower() == usergroupName.ToLower());
		}
		public virtual Usergroup GetUsergroup(Guid usergroupId)
		{
			return GetUsergroupsQuery().FirstOrDefault(u => u.Id == usergroupId);
		}
		public virtual void DeleteUsergroup(Guid userGroupId)
		{
			var usergroup = usergroupRepository.Get(userGroupId);

			usergroupRepository.Delete(usergroup);
		}
		public virtual void SaveUsergroup(Usergroup usergroup)
		{
			usergroupRepository.SaveOrUpdate(usergroup);
		}
		
		public virtual bool DoesUsergroupExist(Guid usergroupId)
		{
			return GetUsergroup(usergroupId) != null;
		}
	}
}
