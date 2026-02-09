namespace Main.ActionFilters
{
	using System;
	using System.Threading.Tasks;

	using Crm.Library.ActionFilterRegistry;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.BaseModel.ViewModels;
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Site;
	using Crm.Library.Services.Interfaces;

	using Main.Services.Interfaces;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Mvc;
	using Microsoft.AspNetCore.Mvc.ViewFeatures;

	public class CrmModelActionFilter : ICrmActionResultExecutor
	{
		private readonly Lazy<ILookupManager> lookupManager;
		private readonly Lazy<IResourceManager> resourceManager;
		private readonly Lazy<IAuthorizationManager> authorizationManager;
		private readonly Lazy<IUserService> userService;
		private readonly Lazy<ISiteService> siteService;
		private readonly Func<Site, SiteViewModel> siteViewModelFactory;

		public CrmModelActionFilter(Lazy<ILookupManager> lookupManager, Lazy<IResourceManager> resourceManager, Lazy<IAuthorizationManager> authorizationManager, Lazy<IUserService> userService, Lazy<ISiteService> siteService, Func<Site, SiteViewModel> siteViewModelFactory)
		{
			this.lookupManager = lookupManager;
			this.resourceManager = resourceManager;
			this.authorizationManager = authorizationManager;
			this.userService = userService;
			this.siteService = siteService;
			this.siteViewModelFactory = siteViewModelFactory;
		}

		public virtual Task ExecuteAsync(ActionContext context, ViewDataDictionary viewData)
		{
			ExecuteFilter(viewData?.Model as ICrmModel);
			return Task.CompletedTask;
		}
		public virtual void ExecuteFilter(ICrmModel model)
		{
			if (model == null)
			{
				return;
			}

			model.AuthorizationManager = authorizationManager.Value;

			var crmModelItem = model as ICrmModelItem;
			if (crmModelItem != null)
			{
				UpdateItem(crmModelItem.Item);
			}

			var crmModelList = model as ICrmModelList;
			if (crmModelList?.List != null)
			{
				foreach (var item in crmModelList.List)
				{
					UpdateItem(item);
				}
			}

			var crmModel = model as CrmModel;
			if (crmModel != null)
			{
				if (userService.Value.CurrentUser != null)
				{
					crmModel.User = userService.Value.CurrentUser;
				}

				var site = siteService.Value.CurrentSite;
				crmModel.Site = siteViewModelFactory(site);
				foreach (var modelItem in crmModel.GetModelItems())
				{
					UpdateItem(modelItem);
				}
			}
		}
		protected virtual void UpdateItem(object item)
		{
			var withLookupManager = item as IWithLookupManager;
			if (withLookupManager != null && withLookupManager.LookupManager == null)
			{
				withLookupManager.LookupManager = lookupManager.Value;
			}

			var withResourceManager = item as IWithResourceManager;
			if (withResourceManager != null && withResourceManager.ResourceManager == null)
			{
				withResourceManager.ResourceManager = resourceManager.Value;
			}
		}
	}
}
