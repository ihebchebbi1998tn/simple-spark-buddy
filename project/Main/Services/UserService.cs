namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.RegularExpressions;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Helper;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Unicore;

	using LMobile.Unicore;

	using log4net;

	using Main.Model.Extensions;

	using NHibernate;
	using NHibernate.Linq;

	using User = Crm.Library.Model.User;

	public class UserService : IUserService
	{
		// Members
		private const int TokenLength = 15;
		private readonly ILog logger;
		private readonly IRepositoryWithTypedId<User, string> userRepository;
		private readonly ICurrentPrincipalResolver currentPrincipalResolver;
		private readonly IAccessControlManager accessControlManager;
		private readonly Lazy<IUserStore> userStore;
		private readonly IEntityTypeManager entityTypeManager;
		private readonly Lazy<IGrantedRoleStore> grantedRoleStore;
		private readonly IAppSettingsProvider appSettingsProvider;

		private User currentUser;

		public virtual User CurrentUser
		{
			get
			{
				var username = currentPrincipalResolver.PrincipalName;
				if (currentUser != null && currentUser.Id != username)
				{
					logger.Debug("Cached CurrentUser in UserService does not match current principal name");
					currentUser = null;
				}

				if (currentUser == null && username != null)
				{
					currentUser = userRepository.Session.Get<User>(username);
				}
				return currentUser;
			}
		}

		public virtual User GetUser(string id)
		{
			return id == currentPrincipalResolver.PrincipalName ? CurrentUser : GetUsersQuery().GetById(id);
		}
		public virtual List<PermissionSchemaRole> GetPermissionSchemaRolesByUser(Guid id)
		{
			var roles = new List<PermissionSchemaRole>();
			var grantedRoleKeys = grantedRoleStore.Value.ListByUser(id).Select(x => x.RoleId);
			foreach (var grantedRoleKey in grantedRoleKeys)
			{
				roles.Add(accessControlManager.FindPermissionSchemaRole(grantedRoleKey));
			}
			return roles;
		}
		public virtual User GetUser(Guid id)
		{
			return id == currentPrincipalResolver.PrincipalId ? CurrentUser : GetUsersQuery().GetByUserId(id);
		}
		[Obsolete("Use GetBy... extension for IEnumerable<User> instead", false)]
		public virtual User GetUserByGeneralToken(string token)
		{
			return GetUsersQuery().GetByGeneralToken(token);
		}
		public virtual string GetDisplayName(string id)
		{
			var user = GetUser(id);
			return user != null ? user.DisplayName : id;
		}
		public virtual IQueryable<User> GetUsersQuery()
		{
			var queryable = userRepository.GetAll();
			if (queryable is NhQueryable<User>)
			{
				queryable = queryable.WithOptions(o =>
				{
					o.SetCacheable(true);
					o.SetCacheMode(CacheMode.Normal);
				});
			}
			return queryable;
		}
		public virtual IQueryable<User> GetActiveUsersQuery()
		{
			return GetUsersQuery().Where(u => !u.Discharged);
		}
		public virtual IList<User> GetUsers()
		{
			return GetUsersQuery().ToList();
		}
		public virtual IList<User> GetActiveUsers()
		{
			return GetActiveUsersQuery().ToList();
		}
		public virtual IList<User> GetUsersByUsergroupId(Guid userGroupId)
		{
			return GetUsersQuery().Where(u => u.Usergroups.Any(ug => ug.Id == userGroupId)).ToList();
		}

		public virtual void SaveUser(User user)
		{
			if (user.UserId == default(Guid))
			{
				user.UserId = Guid.NewGuid();
				user.Id = CalculateUniqueUsername(user);
				user.GeneralToken ??= CreateGeneralToken();
			}

			if (user.Discharged)
			{
				if (user.DischargeDate == null || user.DischargeDate <= DateTime.UtcNow)
				{
					user.DischargeDate = DateTime.UtcNow;
				}
				else
				{
					user.Discharged = false;
				}
			}

			var isFirstUser = !GetActiveUsersQuery().Any();
			userRepository.SaveOrUpdate(user);
			userRepository.Session?.Flush();

			if (userStore.Value.FindByUserName(user.Id) == null)
			{
				user.AuthData = userRepository.Get(user.Id)?.AuthData;
				var unicoreUserType = typeof(LMobile.Unicore.User);
				var unicoreUserEntityType = entityTypeManager.Find(unicoreUserType.FullName);
				if (unicoreUserEntityType == null)
				{
					throw new InvalidOperationException($"entity type {unicoreUserType.FullName} does not exist");
				}
				var unicoreUser = new LMobile.Unicore.User
				{
					AuthDataId = user.AuthDataId,
					EntityTypeId = unicoreUserEntityType.UId,
					LockoutEnabled = true,
					UserName = user.Id,
					UId = user.UserId
				};
				userStore.Value.Insert(unicoreUser);
				userRepository.Session?.Flush();
			}

			if (isFirstUser)
			{
				var permissionSchema = accessControlManager.FindPermissionSchema(UnicoreDefaults.DefaultPermissionSchema);
				var administratorRole = accessControlManager.FindPermissionSchemaRole(permissionSchema.UId, RoleName.Administrator);
				accessControlManager.AssignPermissionSchemaRole(administratorRole, user.UserId, UnicoreDefaults.CommonDomainId, UnicoreDefaults.CommonDomainId);
				user.LicensedAt = DateTime.UtcNow;
			}
		}
		public virtual void DeleteUser(string username)
		{
			User user = userRepository.Get(username);
			if (user != null)
			{
				userRepository.Delete(user);
			}
		}

		public virtual void UpdateLastLogin(User user)
		{
			if (!user.LastLoginDate.HasValue || DateTime.UtcNow - user.LastLoginDate.Value > TimeSpan.FromMinutes(1))
			{
				user.LastLoginDate = DateTime.UtcNow;
				userRepository.SaveOrUpdate(user);
			}
		}
		public virtual void UpdateStatus(User user, DateTime? statusDate = null)
		{
			var currentUser = GetUser(user.Id);
			currentUser.Avatar = user.Avatar ?? currentUser.Avatar;
			currentUser.Latitude = user.Latitude ?? currentUser.Latitude;
			currentUser.Longitude = user.Longitude ?? currentUser.Longitude;
			currentUser.StatusKey = user.StatusKey ?? currentUser.StatusKey;
			currentUser.StatusMessage = user.StatusMessage ?? currentUser.StatusMessage;
			currentUser.LastStatusUpdate = statusDate.HasValue ? statusDate.Value : DateTime.UtcNow;
			userRepository.SaveOrUpdate(currentUser);
		}

		protected virtual string CalculateUniqueUsername(User user)
		{
			// TODO Function has to be refactored for sure
			// Database allows a length of 60, but a "." and digit will be added to the end
			const int allowedDbLength = 57;
			var username = user.Id;
			if (string.IsNullOrWhiteSpace(username))
			{
				username = string.Format("{0}.{1}", ReplaceIllegalCharacters(user.FirstName).ToLower(), ReplaceIllegalCharacters(user.LastName).ToLower());
			}
			if (username.Length > allowedDbLength)
			{
				username = username.Substring(0, allowedDbLength);
			}
			username = CheckIfUsernameExists(username);

			return username;
		}
		protected virtual string ReplaceIllegalCharacters(string str)
		{
			const string pattern = "[^a-zA-Z0-9]";
			var replacement = string.Empty;
			var sanitized = Regex.Replace(str, pattern, replacement);
			return sanitized;
		}
		protected virtual string CheckIfUsernameExists(string username)
		{
			var localUsername1 = username;
			var localUsername2 = username;
			var identicalNames = userRepository.GetAll()
				.Where(x => x.Id.StartsWith(localUsername1))
				.Select(x => x.Id)
				.ToList()
				.Where(x => x.Length == localUsername2.Length
							|| x.LastIndexOf(".", StringComparison.Ordinal) == localUsername2.Length)
				.ToList();
			var identicalNamesCount = identicalNames.Count();
			if (identicalNamesCount > 0)
			{
				var usernameNumbers = identicalNames.Where(n => n.Length > username.Length).Select(n => n.Split('.').Last().ParseAsInt()).ToList();
				var newNumber = usernameNumbers.Any() ? usernameNumbers.Max() + 1 : 1;
				username += "." + newNumber;
				return username;
			}
			return username;
		}

		protected virtual string CreateGeneralToken()
		{
			string token;

			do
			{
				token = Id.Create(TokenLength);
			}
			while (GetUsersQuery().Any(x => x.GeneralToken == token));

			return token;
		}
		public virtual string ResetGeneralToken(string email)
		{
			var user = GetUsersQuery().GetByEmail(email);
			if (user == null)
			{
				return null;
			}

			var newToken = CreateGeneralToken();
			user.GeneralToken = newToken;
			userRepository.SaveOrUpdate(user);
			return newToken;
		}
		public UserService(ICurrentPrincipalResolver currentPrincipalResolver,
			IRepositoryWithTypedId<User, string> userRepository,
			ILog logger,
			IAccessControlManager accessControlManager,
			Lazy<IUserStore> userStore,
			IEntityTypeManager entityTypeManager,
			Lazy<IGrantedRoleStore> grantedRoleStore,
			IAppSettingsProvider appSettingsProvider)
		{
			this.currentPrincipalResolver = currentPrincipalResolver;
			this.userRepository = userRepository;
			this.logger = logger;
			this.accessControlManager = accessControlManager;
			this.userStore = userStore;
			this.entityTypeManager = entityTypeManager;
			this.grantedRoleStore = grantedRoleStore;
			this.appSettingsProvider = appSettingsProvider;
		}
		public virtual void PurgeCache()
		{
			logger.Info("Purging UserCache");
			userRepository.Session.SessionFactory.ClearSecondLevelCache();
		}
	}
}
