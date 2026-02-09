namespace Main.Services
{
	using System;
	using System.Linq;
	using System.Reflection;

	using Crm.Library.BaseModel;
	using Crm.Library.BaseModel.Interfaces;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.Interfaces;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Services.Interfaces;

	public class DefaultVisibilityProvider : IVisibilityProvider
	{
		private static readonly MethodInfo FilterByVisibilityInfo = typeof(DefaultVisibilityProvider)
			.GetMethods(BindingFlags.Instance | BindingFlags.Public)
			.Single(x => x.Name == nameof(FilterByVisibility) && x.IsGenericMethod && x.GetParameters().Length == 1);

		private readonly IAuthorizationManager authorizationManager;
		private readonly IUserService userService;
		public DefaultVisibilityProvider(IAuthorizationManager authorizationManager, IUserService userService)
		{
			this.authorizationManager = authorizationManager;
			this.userService = userService;
		}
		public virtual IQueryable FilterByVisibility(IQueryable entities)
		{
			if (typeof(IEntityWithVisibility).IsAssignableFrom(entities.ElementType) == false)
			{
				throw new ArgumentException($"expecting element types of {nameof(IEntityWithVisibility)}", nameof(entities));
			}
			var method = FilterByVisibilityInfo.MakeGenericMethod(entities.ElementType);
			return (IQueryable)method.Invoke(this, new object[] { entities });
		}
		public virtual IQueryable<T> FilterByVisibility<T>(IQueryable<T> entities) where T : IEntityWithVisibility
		{
			return FilterByVisibility(entities, userService.CurrentUser);
		}
		public virtual IQueryable<T> FilterByVisibility<T>(IQueryable<T> entities, User user) where T : IEntityWithVisibility
		{
			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Visibility, PermissionName.SkipCheck))
			{
				return entities;
			}

			var userGroupIds = user.Usergroups.Select(ug => ug.Id).ToList();
			entities = entities.Where(
				x => x.Visibility == Visibility.Everybody
					|| x.Visibility == Visibility.OnlyMe && x.CreateUser == user.Id
					|| x.Visibility == Visibility.Users && x.VisibleToUserIds.Any(u => u == user.Id)
					|| x.Visibility == Visibility.UserGroups && x.VisibleToUsergroupIds.Any(u => userGroupIds.Contains(u)));

			return entities;
		}
		public virtual IEntityWithVisibility SetVisibility(IEntityWithVisibility contact)
		{
			contact.Visibility = Visibility.Everybody;
			contact.VisibleToUserIds.Clear();
			contact.VisibleToUsergroupIds.Clear();
			return contact;
		}
		public virtual bool IsVisible(IEntityWithVisibility contact, User user)
		{
			if (contact == null)
			{
				return true;
			}

			if (authorizationManager.IsAuthorizedForAction(user, PermissionGroup.Visibility, PermissionName.SkipCheck))
			{
				return true;
			}

			if (contact.Visibility == Visibility.Everybody)
			{
				return true;
			}

			if ((contact.Visibility == Visibility.OnlyMe))
			{
				if (contact.CreateUser == user.Id)
				{
					return true;
				}
				return false;
			}

			// Todo: Combining    Visibility.UserGroups and Visibility.Users in all locations of visibility definition
			var userGroupIds = user.Usergroups.Select(ug => ug.Id);
			// ReSharper disable once ConvertClosureToMethodGroup
			if (contact.VisibleToUsergroupIds.Any(u => userGroupIds.Contains(u)))
			{
				return true;
			}

			return contact.VisibleToUserIds.Any(u => u == user.Id);
		}
	}
}
