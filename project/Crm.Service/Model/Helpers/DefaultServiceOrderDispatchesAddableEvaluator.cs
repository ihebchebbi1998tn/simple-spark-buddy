namespace Crm.Service.Model.Helpers
{
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model.Lookup;

	public class DefaultServiceOrderDispatchesAddableEvaluator : IServiceOrderDispatchesAddableEvaluator
	{
		private readonly IUserService userService;
		private readonly ILookupManager lookupManager;
		private readonly IAuthorizationManager authorizationManager;

		public virtual bool Evaluate(ServiceOrderHead order, string username)
		{
			var user = userService.GetUser(username);

			if (order == null || user == null)
			{
				return false;
			}

			var status = lookupManager.Get<ServiceOrderStatus>(order.StatusKey);
			var canDispatchesBeAdded = (status.BelongsToScheduling() || status.BelongsToInProgress()) && authorizationManager.IsAuthorizedForAction(user, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Create);

			return canDispatchesBeAdded;
		}

		public DefaultServiceOrderDispatchesAddableEvaluator(IUserService userService, ILookupManager lookupManager, IAuthorizationManager authorizationManager)
		{
			this.userService = userService;
			this.lookupManager = lookupManager;
			this.authorizationManager = authorizationManager;
		}
	}
}
