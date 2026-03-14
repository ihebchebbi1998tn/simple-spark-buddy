namespace Crm.Service.Team.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	using Autofac;
	using Autofac.Core;

	using AutoMapper;

	using Crm.Article.Model;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Rest;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.PerDiem;
	using Crm.PerDiem.Model.Lookups;
	using Crm.Service.Team.Model;
	using Crm.Services.Interfaces;

	using LinqKit;

	public class ArticleUserGroupRelationshipSyncService : DefaultSyncService<ArticleUserGroupRelationship, Guid>
	{
		private readonly IEnumerable<IContactSyncService> articleSyncServices;
		private readonly ISyncService<Usergroup> usergroupSyncService;
		private readonly IAuthorizationManager authorizationManager;
		private readonly ILookupService lookupService;
		private readonly IAppSettingsProvider appSettingsProvider;
		public ArticleUserGroupRelationshipSyncService(IRepositoryWithTypedId<ArticleUserGroupRelationship, Guid> repository, RestTypeProvider restTypeProvider, IRestSerializer restSerializer, IMapper mapper, IComponentContext context, ISyncService<Usergroup> usergroupSyncService, IAuthorizationManager authorizationManager, ILookupService lookupService, IAppSettingsProvider appSettingsProvider)
			: base(repository,
				restTypeProvider,
				restSerializer,
				mapper)
		{
			this.usergroupSyncService = usergroupSyncService;
			this.authorizationManager = authorizationManager;
			this.lookupService = lookupService;
			this.appSettingsProvider = appSettingsProvider;
			var articleSyncServiceRegistrations = context.ComponentRegistry.Registrations.Where(x => x.Services.OfType<TypedService>().Any(s => s.ServiceType.IsGenericType && s.ServiceType.GetGenericTypeDefinition() == typeof(ISyncService<>) && typeof(Article).IsAssignableFrom(s.ServiceType.GetGenericArguments().First())));
			articleSyncServices = articleSyncServiceRegistrations.Where(x => x.Activator.LimitType != GetType()).Select(x => context.Resolve(x.Activator.LimitType) as IContactSyncService).ToList();
		}
		public override Type[] ClientSyncDependencies
		{
			get
			{
				var clientSyncDependencies = articleSyncServices.Select(x => ((ISyncService)x).SyncedType)
					.Concat(new[] { typeof(Usergroup) })
					.ToArray();
				return clientSyncDependencies;
			}
		}
		public override Type[] SyncDependencies => new[] { typeof(Article), typeof(User) };
		public override ArticleUserGroupRelationship Save(ArticleUserGroupRelationship entity)
		{
			return repository.SaveOrUpdate(entity);
		}
		public override IQueryable<ArticleUserGroupRelationship> GetAll(User user, IDictionary<string, int?> groups, IDictionary<string, Guid> clientIds)
		{
			var entities = repository.GetAll();

			Expression<Func<ArticleUserGroupRelationship, bool>> predicate = null;
			foreach (var articleSyncService in articleSyncServices.Where(x => authorizationManager.IsAuthorizedForAction(user,
						PermissionGroup.WebApi,
						((ISyncService)x).SyncedType.Name)))
			{
				var articleIds = articleSyncService.GetAllContactIds(user,
					groups,
					clientIds);
				predicate = predicate == null ? x => articleIds.Contains(x.ArticleKey) : predicate.Or(x => articleIds.Contains(x.ArticleKey));
			}

			entities = predicate == null ? entities.Where(x => false) : entities.Where(predicate);

			var usergroups = usergroupSyncService.GetAll(user,
				groups,
				clientIds);

			var historySince = lookupService.GetLookupQuery<TimeEntryType>().Max(x => x.AllowedDaysIntoPast) ?? 0;
			historySince = Math.Max(historySince, appSettingsProvider.GetValue(PerDiemPlugin.Settings.TimeEntry.MaxDaysAgo));
			historySince = Math.Max(historySince, appSettingsProvider.GetValue(ServicePlugin.Settings.ServiceOrderTimePosting.MaxDaysAgo));
			historySince++;

			return entities
				.Where(x => usergroups.Any(y => y.Id == x.UserGroupKey))
				.Where(x => x.To == null || x.To >= DateTime.UtcNow.AddDays(-1 * historySince));
		}
	}
}
