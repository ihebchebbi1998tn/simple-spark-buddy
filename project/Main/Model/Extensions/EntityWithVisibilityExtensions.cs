namespace Main.Model.Extensions
{
	using System.Linq;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;

	public static class EntityWithVisibilityExtensions
	{
		public static IQueryable<T> VisibleTo<T>(this IQueryable<T> entities, IAuthorizationManager authorizationManager, User user)
			where T : IEntityWithVisibility
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Visibility, PermissionName.SkipCheck) || !entities.Any())
			{
				return entities;
			}

			var userGroupIds = user.Usergroups.Select(ug => ug.Id).ToList();
			entities = entities.Where(
				x => x.Visibility == Visibility.Everybody
				     || (x.Visibility == Visibility.OnlyMe && x.CreateUser == user.Id)
				     || (x.Visibility == Visibility.Users && x.VisibleToUserIds.Any(u => u == user.Id))
				     || (x.Visibility == Visibility.UserGroups && x.VisibleToUsergroupIds.Any(u => userGroupIds.Contains(u))));

			return entities;
		}
	}
}
