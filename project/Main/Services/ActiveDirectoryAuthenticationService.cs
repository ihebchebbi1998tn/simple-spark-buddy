namespace Main.Services
{
	extern alias SystemConfigurationConfigurationManager;
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Helper;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using log4net;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Http;

	using Novell.Directory.Ldap;

	using IAuthenticationService = Crm.Library.Services.Interfaces.IAuthenticationService;

	public class ActiveDirectoryAuthenticationService : IAuthenticationService
	{
		private readonly IUserService userService;
		private readonly ILog logger;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IRepositoryWithTypedId<Device, Guid> deviceRepository;

		public virtual int MinPasswordLength
		{
			get
			{
				// If this value should be reliable, one must login to the AD and Query for different
				// password guidelines
				return 6;
			}
		}

		public virtual bool PasswordChangeSupported
		{
			get { return false; }
		}

		public virtual bool PasswordResetSupported
		{
			get { return false; }
		}

		public virtual User GetUser(string login)
		{
			if (string.IsNullOrWhiteSpace(login) == false)
			{
				return userService.GetUsersQuery().FirstOrDefault(x => x.AdName != null && x.AdName.ToLower() == login.ToLower());
			}
			return null;
		}

		public virtual void SignIn(User user, bool createPersistentCookie)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, user.GetIdentityString())
			};
			var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			httpContextAccessor.HttpContext.SignInAsync(new ClaimsPrincipal(claimsIdentity), new AuthenticationProperties { IsPersistent = createPersistentCookie });
		}
		public virtual void SignOut()
		{
			httpContextAccessor.HttpContext.Session.Clear();
			httpContextAccessor.HttpContext.Response.Cookies.Delete(Microsoft.AspNetCore.Session.SessionDefaults.CookieName);
			httpContextAccessor.HttpContext.SignOutAsync();
		}
		public virtual bool Validate(string login, string password, out User user)
		{
			user = GetUser(login);
			return Validate(user, password);
		}
		public virtual bool Validate(User user, string password)
		{
			if (user == null)
			{
				return false;
			}

			var username = user.AdName ?? user.Id;
			var connectionString = appSettingsProvider.GetValue(MainPlugin.Settings.System.ActiveDirectoryEndpoint);
			var uri = new Uri(connectionString);
			try
			{
				var useSsl = uri.Port != LdapConnection.DefaultPort;
				var dn = $"{username}@{uri.Host}";
				using (var connection = new LdapConnection { SecureSocketLayer = useSsl })
				{
					connection.Connect(uri.Host, uri.Port);
					connection.Bind(dn, password);
					if (connection.Bound)
					{
						return true;
					}
				}
			}
			catch (LdapException ex)
			{
				logger.Error("Validating user in active directory threw an error", ex);
			}
			return false;
		}
		public virtual string EncryptPassword(string login, string password)
		{
			throw new NotImplementedException("This is not part of the game. When using AD this will be executed elsewhere");
		}
		public virtual string LoginTypeKey
		{
			get { return "Username"; }
		}
		public virtual bool ChangePassword(User user, string oldPassword, string newPassword)
		{
			throw new NotSupportedException("Changing passwords is not supported for AD provider");
		}
		public virtual void ResetPassword(User user, string newPassword)
		{
			throw new NotSupportedException("Resetting passwords is not supported for AD provider");
		}
		public virtual void SignOutDevices(User user, string currentFingerprint = null)
		{
			var devices = deviceRepository.GetAll()
				.Where(device => device.Username == user.Id);

			if (currentFingerprint.IsNotNullOrEmpty())
			{
				devices = devices
					.Where(device => device.Fingerprint != currentFingerprint);
			}
				
			devices
				.ToList()
				.ForEach(deviceRepository.Delete);
		}

		public ActiveDirectoryAuthenticationService(IUserService userService,
			ILog logger,
			IHttpContextAccessor httpContextAccessor,
			IAppSettingsProvider appSettingsProvider,
			IRepositoryWithTypedId<Device, Guid> deviceRepository)
		{
			this.userService = userService;
			this.logger = logger;
			this.httpContextAccessor = httpContextAccessor;
			this.appSettingsProvider = appSettingsProvider;
			this.deviceRepository = deviceRepository;
		}
	}
}
