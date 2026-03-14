namespace Crm.Service.Model.Helpers
{
	using Crm.Library.Globalization.Lookup;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Services.Interfaces;
	using Crm.Service.Model.Lookup;

	public class DefaultServiceOrderDispatchEditableByUserEvaluator : IServiceOrderDispatchEditableByUserEvaluator
	{
		private readonly IUserService userService;
		private readonly ILookupManager lookupManager;
		private readonly IAuthorizationManager authorizationManager;

		public virtual bool Evaluate(ServiceOrderDispatch dispatch, ServiceOrderHead order, string username)
		{
			var user = userService.GetUser(username);

			if (dispatch == null || order == null || user == null)
			{
				return false;
			}

			var dispatchStatus = lookupManager.Get<ServiceOrderDispatchStatus>(dispatch.StatusKey);
			var orderStatus = lookupManager.Get<ServiceOrderStatus>(order.StatusKey);
			var areDispatchesEditableForServiceOrderStatus = orderStatus.BelongsToScheduling() || orderStatus.BelongsToInProgress();
			var isDispatchEditableForCurrentUser = dispatch.DispatchedUser.Id == user.Id || authorizationManager.IsAuthorizedForAction(user, ServicePlugin.PermissionGroup.Dispatch, PermissionName.Edit);
			var isDispatchClosed = dispatchStatus.IsClosedNotComplete() || dispatchStatus.IsClosedComplete();

			return areDispatchesEditableForServiceOrderStatus && isDispatchEditableForCurrentUser && !isDispatchClosed;
		}

		public DefaultServiceOrderDispatchEditableByUserEvaluator(IUserService userService, ILookupManager lookupManager, IAuthorizationManager authorizationManager)
		{
			this.userService = userService;
			this.lookupManager = lookupManager;
			this.authorizationManager = authorizationManager;
		}
	}
}
