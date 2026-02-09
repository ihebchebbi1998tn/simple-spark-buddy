namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using AutoMapper;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Site;
	using Crm.Library.Rest;
	using Crm.Library.Services;

	using Main.Services.Interfaces;

	public class SiteSyncService : DefaultSyncService<Site, Guid>
	{
		private readonly ISiteService siteService;

		public SiteSyncService(IRepositoryWithTypedId<Site, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, ISiteService siteService)
			: base(repository, restTypeProvider, restSerializer, mapper)
		{
			this.siteService = siteService;
		}
		public override IQueryable<Site> Get(object[] ids)
		{
			return (ids?.Length > 0 ? new List<Site> { siteService.CurrentSite } : new List<Site>()).AsQueryable();
		}
		public override IQueryable<Site> GetAll(User user)
		{
			return new List<Site> { siteService.CurrentSite }.AsQueryable();
		}
		public override void Remove(Site entity)
		{
			throw new NotImplementedException();
		}
	}
}
