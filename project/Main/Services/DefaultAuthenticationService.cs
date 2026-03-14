namespace Main.Services
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using System.Text;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;

	using Main.Model.Extensions;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Http;

	using IAuthenticationService = Crm.Library.Services.Interfaces.IAuthenticationService;

	public class DefaultAuthenticationService : IAuthenticationService
	{
		private readonly IUserService userService;
		private readonly IHttpContextAccessor httpContextAccessor;
		private readonly IRepositoryWithTypedId<Device, Guid> deviceRepository;

		public virtual int MinPasswordLength
		{
			get { return 6; }
		}

		public virtual string LoginTypeKey
		{
			get { return "Email"; }
		}

		public virtual bool PasswordChangeSupported
		{
			get { return true; }
		}

		public virtual bool PasswordResetSupported
		{
			get { return true; }
		}

		public virtual User GetUser(string login)
		{
			if (string.IsNullOrWhiteSpace(login) == false)
			{
				return userService.GetUsersQuery().GetByEmail(login);
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

		public virtual bool Validate(string email, string password, out User user)
		{
			user = GetUser(email);
			return Validate(user, password);
		}
		public virtual bool Validate(User user, string password)
		{
			return user != null && user.Password == EncryptPassword(user.Email, password);
		}
		protected virtual string EncryptPassword(string email, string password)
		{
			var sha256 = SHA256.Create();
			var encryptedPassword = Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(password + email.ToLower())));

			return encryptedPassword;
		}

		public virtual bool ChangePassword(User user, string oldPassword, string newPassword)
		{
			if (user == null || !Validate(user, oldPassword))
			{
				return false;
			}

			ResetPassword(user, newPassword);

			return true;
		}
		public virtual void ResetPassword(User user, string newPassword)
		{
			if (user == null)
			{
				return;
			}

			var encryptedNewPassword = EncryptPassword(user.Email, newPassword);
			user.Password = encryptedNewPassword;
			userService.SaveUser(user);
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

		public DefaultAuthenticationService(
			IUserService userService, 
			IHttpContextAccessor httpContextAccessor, 
			IRepositoryWithTypedId<Device, Guid> deviceRepository)
		{
			this.userService = userService;
			this.httpContextAccessor = httpContextAccessor;
			this.deviceRepository = deviceRepository;
		}
	}
}
