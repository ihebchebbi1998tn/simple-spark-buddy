namespace Crm.Service.Services
{
	using System.Collections.Generic;
	using System.Linq;
	using System.Reflection;

	using Crm.Article.Model;
	using Crm.Article.Model.Lookups;
	using Crm.Library.Api.Controller;
	using Crm.Library.AutoFac;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model;

	using Microsoft.AspNetCore.OData.Query;

	public class ODataQueryArticleDescriptionFilter : IODataQueryFunction, IDependency
	{
		private readonly IRepository<ArticleDescription> articleDescriptionRepository;
		private readonly IRepository<Article> articleRepository;
		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		protected static MethodInfo FilterByForTimePostingInfo = typeof(ODataQueryArticleDescriptionFilter)
			.GetMethod(nameof(FilterByForTimePosting), BindingFlags.Instance | BindingFlags.NonPublic);
		protected static MethodInfo FilterByForMaterialInfo = typeof(ODataQueryArticleDescriptionFilter)
			.GetMethod(nameof(FilterByForMaterial), BindingFlags.Instance | BindingFlags.NonPublic);
		public ODataQueryArticleDescriptionFilter(IRepository<ArticleDescription> articleDescriptionRepository, IRepository<Article> articleRepository, IAuthorizationManager authorizationManager, IUserService userService)
		{
			this.articleDescriptionRepository = articleDescriptionRepository;
			this.articleRepository = articleRepository;
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		protected virtual IQueryable<ServiceOrderTimePosting> FilterByForTimePosting(IQueryable<ServiceOrderTimePosting> query, string language, string filter)
		{
			var articleDescriptionItemNos = GetArticleDescriptionItemNos(language, filter);
			var articleItemNos = GetArticleItemNos(filter);
			return query.Where(a => articleDescriptionItemNos.Contains(a.ItemNo) || articleItemNos.Contains(a.ItemNo));
		}
		protected virtual IQueryable<ServiceOrderMaterial> FilterByForMaterial(IQueryable<ServiceOrderMaterial> query, string language, string filter)
		{
			var articleDescriptionItemNos = GetArticleDescriptionItemNos(language, filter);
			var articleItemNos = GetArticleItemNos(filter);
			return query.Where(a => articleDescriptionItemNos.Contains(a.ItemNo) || articleItemNos.Contains(a.ItemNo));
		}
		protected virtual IEnumerable<string> GetArticleItemNos(string filter)
		{
			var articleItemNos = Enumerable.Empty<string>();
			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.WebApiRead, typeof(Article).Name))
			{
				articleItemNos = articleRepository.GetAll()
					.Where(x => x.ContactType == nameof(Article) && x.Description.Contains(filter))
					.Select(x => x.ItemNo);
			}

			return articleItemNos;
		}
		protected virtual IEnumerable<string> GetArticleDescriptionItemNos(string language, string filter)
		{
			var articleDescriptionItemNos = Enumerable.Empty<string>();
			if (authorizationManager.IsAuthorizedForAction(userService.CurrentUser, PermissionGroup.WebApiRead, typeof(ArticleDescription).Name))
			{
				articleDescriptionItemNos = articleDescriptionRepository.GetAll()
					.Where(x => x.Language == language)
					.Where(x => x.Value.Contains(filter))
					.Select(x => x.Key);
			}

			return articleDescriptionItemNos;
		}
		public virtual IQueryable<T> Apply<T, TRest>(ODataQueryOptions<TRest> options, IQueryable<T> query)
			where T : class, IEntityWithId
			where TRest : class
		{
			MethodInfo filterBy;
			switch (typeof(T).Name)
			{
				case nameof(ServiceOrderTimePosting):
					filterBy = FilterByForTimePostingInfo;
					break;
				case nameof(ServiceOrderMaterial):
					filterBy = FilterByForMaterialInfo;
					break;
				default: return query;
			}
			var language = options.Request.GetQueryParameter("filterByArticleDescriptionLanguage")?.Trim();
			var filter = options.Request.GetQueryParameter("filterByArticleDescriptionFilter")?.Trim();
			if (string.IsNullOrEmpty(language) == false && string.IsNullOrEmpty(filter) == false)
			{
				return (IQueryable<T>)filterBy.Invoke(this, new object[] { query, language, filter });
			}
			return query;
		}
	}
}
