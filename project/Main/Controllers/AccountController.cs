namespace Main.Controllers
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Reflection;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	using Crm.Library.Data.Domain.DataInterfaces;
	using Crm.Library.Data.NHibernateProvider;
	using Crm.Library.Extensions;
	using Crm.Library.Extensions.IIdentity;
	using Crm.Library.Globalization.Resource;
	using Crm.Library.Helper;
	using Crm.Library.MobileViewEngine;
	using Crm.Library.Model;
	using Crm.Library.Model.Authorization;
	using Crm.Library.Model.Authorization.PermissionIntegration;
	using Crm.Library.Model.Site;
	using Crm.Library.Modularization;
	using Crm.Library.Modularization.Interfaces;
	using Crm.Library.Services;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation;

	using log4net;

	using Main.Extensions;
	using Main.Model;
	using Main.Model.Extensions;
	using Main.Model.Lookups;
	using Main.Services.Interfaces;
	using Main.ViewModels;

	using Microsoft.AspNetCore.Authentication;
	using Microsoft.AspNetCore.Authentication.Cookies;
	using Microsoft.AspNetCore.Authorization;
	using Microsoft.AspNetCore.Http;
	using Microsoft.AspNetCore.Mvc;

	using Zxcvbn;

	using IAuthenticationService = Crm.Library.Services.Interfaces.IAuthenticationService;
	using IRedirectProvider = Main.Services.Interfaces.IRedirectProvider;
	using Task = System.Threading.Tasks.Task;

	[Authorize]
	public class AccountController : Controller
	{
		private readonly IAuthenticationService authenticationService;
		private readonly IUserService userService;
		private readonly ILog logger;
		private readonly IBrowserCapabilities browserCapabilities;
		private readonly IEnumerable<IRedirectProvider> redirectProviders;
		private readonly Site site;
		private readonly IAppSettingsProvider appSettingsProvider;
		private readonly IEnumerable<ILoginValidator> loginValidators;
		private readonly Func<User> userFactory;
		private readonly Func<Message> messageFactory;
		private readonly IRepositoryWithTypedId<Message, Guid> messageRepository;
		private readonly IRuleValidationService ruleValidationService;
		private readonly IResourceManager resourceManager;
		private readonly IRepositoryWithTypedId<Device, Guid> deviceRepository;
		private readonly IPluginProvider pluginProvider;
		private readonly IRepositoryWithTypedId<PasswordResetToken, Guid> passwordResetTokenRepository;
		private readonly Func<PasswordResetToken> passwordResetTokenFactory;
		private readonly ITotpService totpService;
		private readonly ISessionProvider sessionProvider;

		public virtual ActionResult AccessDenied()
		{
			return RedirectToAction("Forbidden", "Error");
		}

		[RenderAction("AccountInfoEdit", Priority = 70)]
		public virtual ActionResult AccountInfoEditEmail()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 70)]
		public virtual ActionResult AccountInfoViewEmail()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 50)]
		public virtual ActionResult AccountInfoEditLanguage()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 50)]
		public virtual ActionResult AccountInfoViewLanguage()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 100)]
		public virtual ActionResult AccountInfoEditName()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 100)]
		public virtual ActionResult AccountInfoViewName()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoEdit", Priority = 60)]
		public virtual ActionResult AccountInfoEditPassword()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 60)]
		public virtual ActionResult AccountInfoViewPassword()
		{
			return PartialView();
		}

		[RenderAction("AccountInfoView", Priority = 55)]
		public virtual ActionResult AccountInfoViewMultiFactorAuthentication()
		{
			return PartialView();
		}

		public virtual ActionResult SetUpTotpAuthenticator() => PartialView();

		// Methods
		[AllowAnonymous]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Login()
		{
			if (!userService.GetActiveUsersQuery().Any())
			{
				return RedirectToAction("Register");
			}

			var viewModel = GetLoginViewModel();

			if (TempData["PasswordResetLinkSent"] != null && (bool)TempData["PasswordResetLinkSent"])
			{
				viewModel.NewPasswordSent = true;
			}

			if (Request.Query["noTenantAvailable"] == "true")
			{
				viewModel.NoTenantAvailable = true;
			}

			return View("Login", viewModel);
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult Login(string email, string password, bool? rememberMe, string returnUrl, bool? needRedirect, string newPassword, string newPassword2)
		{
			User user = null;

			rememberMe = rememberMe ?? false;
			email = email.Trim();

			var ruleViolations = new List<RuleViolation>();

			if (password.IsNullOrEmpty() || !authenticationService.Validate(email,
					password,
					out user))
			{
				ruleViolations.Add(new RuleViolation("InvalidLoginData"));
			}

			ruleViolations.AddRange(loginValidators.SelectMany(x => x.GetRuleViolations(user)));

			if (ruleViolations.Count > 0)
			{
				return RedirectToLogin(ruleViolations, rememberMe);
			}

			var cookieOptions = new CookieOptions
			{
				Expires = (bool)rememberMe ? DateTime.Now.AddYears(50) : DateTime.Now.AddDays(-1),
				Path = "/"
			};
			Response.Cookies.Append("Login",
				"pre-populated",
				cookieOptions);
			Debug.Assert(user != null, "user != null");

			if (user.PasswordChangeRequired)
			{
				var model = GetLoginViewModel();
				model.PasswordChangeRequired = true;

				if (newPassword.IsNullOrEmpty())
				{
					return View(model);
				}

				if (!newPassword.Equals(newPassword2, StringComparison.Ordinal))
				{
					model.RuleViolations.Add(new RuleViolation("PasswordsDoNotMatch"));
					return View(model);
				}

				var passwordStrengthResult = Core.EvaluatePassword(newPassword);
				if (passwordStrengthResult.Score < site.GetExtension<DomainExtension>().MinPasswordStrength)
				{
					// return new JsonResult(new { ErrorMessageKey = "RuleViolation.PasswordTooWeak", passwordStrengthResult.Feedback, passwordStrengthResult.Score });
					model.RuleViolations.Add(new RuleViolation("PasswordTooWeak"));
					return View(model);
				}

				user.PasswordChangeRequired = false;
				authenticationService.ResetPassword(user, newPassword);
			}

			if (IsMultiFactorAuthenticationRequired(user))
			{
				return RedirectToLoginMFATotp(user,
					rememberMe,
					returnUrl,
					needRedirect);
			}

			return SignIn(user,
				(bool)rememberMe,
				returnUrl,
				needRedirect);
		}

		protected virtual ActionResult SignIn(User user, bool rememberMe, string returnUrl, bool? needRedirect)
		{
			authenticationService.SignIn(user, rememberMe);

			var loginMessage = new StringBuilder();
			loginMessage.AppendFormatLine("User {0} successfully logged in.", user.Id.HideSensitiveDataFromName());
			loginMessage.AppendLine(String.Empty);
			loginMessage.AppendLine(GetUserData());
			logger.InfoFormat(loginMessage.ToString());

			userService.UpdateLastLogin(user);

			var availableClients = redirectProviders.Select(x => x.AvailableClients(user)).Where(x => x != null).Distinct().ToList();
			if (availableClients.Count == 0)
			{
				logger.Error($"User {user.Id.HideSensitiveDataFromName()} has no access to any client. Please contact your system administrator.");
				return new RedirectResult("~/Home/ClientSelection");
			}

			if (needRedirect.HasValue && needRedirect.Value)
			{
				if (availableClients.Count > 1)
				{
					return new RedirectResult("~/Main/Home/ClientSelection");
				}

				switch (availableClients.ElementAt(0))
				{
					case PermissionName.Backend:
						return new RedirectResult("~/Main/Dashboard/Index");
					case PermissionName.MaterialClientOnline:
						return new RedirectResult("~/Main/Home/MaterialIndex");
					case PermissionName.MaterialClientOffline:
						return new RedirectResult("~/Main/Home/MaterialIndex");
					case "VideoClient":
						return new RedirectResult("~/Main.VideoCall/VideoCall/Index");
					default:
						return new RedirectResult("~/Main/Home/ClientSelection");
				}
			}

			if (!String.IsNullOrEmpty(returnUrl))
			{
				if (returnUrl == HttpContext.Request.PathBase && !returnUrl.EndsWith("/"))
				{
					returnUrl += '/';
				}

				if (!Url.IsLocalUrl(returnUrl))
				{
					returnUrl = "/";
				}

				return new RedirectResult(returnUrl);
			}

			var redirects = redirectProviders.Select(x => x.RedirectAfterLogin(user,
				browserCapabilities,
				returnUrl)).Where(x => x != null).Distinct().ToArray();
			if (redirects.Length == 0)
			{
				return new RedirectToRouteResult("Dashboard", null);
			}

			if (redirects.Length == 1)
			{
				return redirects.First();
			}

			return new RedirectResult("~/Main/Home/ClientSelection");
		}

		public virtual ActionResult RedirectToLogin(IList<RuleViolation> ruleViolations, bool? rememberMe)
		{
			var model = GetLoginViewModel();
			model.AddRuleViolations(ruleViolations);
			ViewData["rememberMe"] = rememberMe;
			return View("Login", model);
		}

		public virtual ActionResult RedirectToLoginMFATotp(User user, bool? rememberMe, string returnUrl, bool? needRedirect)
		{
			var model = new LoginMfaTotpViewModel();
			model.User = user;
			ViewData["rememberMe"] = rememberMe;
			TempData["UserId"] = user.UserId;

			if (user.TotpAuthorizationKey == null)
			{
				var newAuthorizationKey = totpService.GenerateNewAuthorizationKey();
				model.NewAuthorizationKey = newAuthorizationKey;
				model.NewAuthorizationUri = totpService.ComposeAuthorizationUri(newAuthorizationKey, user.Email);
			}

			return View("LoginMFATotp", model);
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual IActionResult ValidateTotp(string token, string newAuthorizationKey)
		{
			if (string.IsNullOrEmpty(token))
			{
				return new StatusCodeResult(StatusCodes.Status403Forbidden);
			}

			var userId = TempData["UserId"] as Guid?;

			if (!userId.HasValue)
			{
				return new StatusCodeResult(StatusCodes.Status403Forbidden);
			}

			var user = userService.GetUsersQuery().GetByUserId(userId.Value);
			if (user == null)
			{
				return new StatusCodeResult(StatusCodes.Status403Forbidden);
			}
			else if (user.TotpAuthorizationKey == null && string.IsNullOrEmpty(newAuthorizationKey))
			{
				return new StatusCodeResult(StatusCodes.Status403Forbidden);
			}

			var totpAuthorizationKey = newAuthorizationKey;
			if (string.IsNullOrEmpty(newAuthorizationKey))
			{
				try
				{
					totpAuthorizationKey = totpService.DecryptAuthorizationKey(user.UserId, user.TotpAuthorizationKey);
				}
				catch (Exception ex)
				{
					logger.Error("Totp validation error.", ex);

					return new StatusCodeResult(StatusCodes.Status403Forbidden);
				}
			}

			TempData["UserId"] = userId;

			var tokenIsValid = totpService.ValidateToken(totpAuthorizationKey, token);

			if (tokenIsValid)
			{
				return Json(new { success = true });
			}
			else
			{
				return StatusCode(StatusCodes.Status401Unauthorized, new { message = resourceManager.GetTranslation("InvalidTotpToken") });
			}
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult LoginMFATotp(string newAuthorizationKey, bool rememberMe, string returnUrl, bool? needRedirect)
		{
			var userId = TempData["UserId"] as Guid?;

			var model = new LoginMfaTotpViewModel();

			if (userId.HasValue)
			{
				model.User = userService.GetUsersQuery().GetByUserId(userId.Value);
			}

			if (!userId.HasValue)
			{
				return RedirectToLogin(model.RuleViolations, rememberMe);
			}

			if (model.User.TotpAuthorizationKey == null)
			{
				if (string.IsNullOrEmpty(newAuthorizationKey))
				{
					return RedirectToLogin(model.RuleViolations, rememberMe);
				}

				var totpAuthorizationKey = newAuthorizationKey;
				model.User.TotpAuthorizationKey = totpService.EncryptAuthorizationKey(model.User.UserId, totpAuthorizationKey);
				userService.SaveUser(model.User);
			}

			return SignIn(model.User,
				rememberMe,
				returnUrl,
				needRedirect);
		}

		[AllowAnonymous]
		public virtual async Task OpenIdLogin(string returnUrl)
		{
			var authProperties = new AuthenticationProperties();
			if (returnUrl != null)
			{
				authProperties.RedirectUri = returnUrl;
			}
			else
			{
				authProperties.RedirectUri = Url.Home();
			}

			await HttpContext.ChallengeAsync("Auth0", authProperties);
		}

		[AllowAnonymous]
		public virtual RedirectResult OpenIdCallback()
		{
			return new RedirectResult("~/");
		}

		[AllowAnonymous]
		public virtual ActionResult OpenIdError(OpenIdError error)
		{
			return PartialView("OpenIdError", new OpenIdErrorViewModel { error = error });
		}

		[AllowAnonymous]
		public virtual async Task<ActionResult> Logout()
		{
			var fingerprint = HttpContext.Request.Cookies["fingerprint"];
			if (fingerprint.IsNotNullOrWhiteSpace() && userService.CurrentUser != null)
			{
				var device = deviceRepository.GetAll()
					.Where(x => x.Username == userService.CurrentUser.Id)
					.Where(x => x.Fingerprint == HttpContext.Request.Cookies["fingerprint"])
					.FirstOrDefault();

				if (device != null)
				{
					deviceRepository.Delete(device);
				}
			}

			authenticationService.SignOut();

			if (appSettingsProvider.GetValue(MainPlugin.Settings.System.OpenIdAuthentication.UseOpenIdAuthentication))
			{
				await HttpContext.SignOutAsync("Auth0");
				await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
				return new EmptyResult();
			}

			return new RedirectResult("~/Main/Account/Login" + Request.QueryString);
		}

		[AllowAnonymous]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Register()
		{
			if (userService.GetActiveUsersQuery().Any())
			{
				return RedirectToAction("Login");
			}

			var user = userFactory();
			user.DefaultLanguageKey = site.GetExtension<DomainExtension>().DefaultLanguageKey;
			var model = new UserEditViewModel
			{
				Item = user,
				PasswordResetSupported = authenticationService.PasswordResetSupported,
				MinPasswordLength = authenticationService.MinPasswordLength
			};

			return View("Register", model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult Register(User user, string confirmPassword)
		{
			if (userService.GetActiveUsersQuery().Any())
			{
				return Unauthorized();
			}

			if (user != null && user.Email.IsNotNullOrEmpty())
			{
				user.Email = user.Email.Trim();
			}

			var ruleViolations = new List<RuleViolation>();

			try
			{
				ruleViolations.AddRange(ruleValidationService.GetRuleViolations(user));
				if (ruleViolations.Count == 0)
				{
					authenticationService.ResetPassword(user, confirmPassword);
					userService.SaveUser(user);
				}
			}
			catch (Exception ex)
			{
				ruleViolations.Add(new RuleViolation(ex.Message).SetDisplayRegion("UnknownError"));
			}

			var model = new UserEditViewModel
			{
				Item = user,
				MinPasswordLength = authenticationService.MinPasswordLength,
				PasswordResetSupported = authenticationService.PasswordResetSupported
			};

			if (ruleViolations.Count > 0)
			{
				ruleViolations.Where(x => x.DisplayRegion.IsNullOrEmpty()).ForEach(v => v.DisplayRegion = v.PropertyName);
				model.AddRuleViolations(ruleViolations);
				return View("Register", model);
			}

			authenticationService.SignIn(user, false);
			userService.UpdateLastLogin(user);

			return RedirectToAction("Index", "Home");
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual ActionResult ResetGeneralToken(string email)
		{
			if (email.IsNotNullOrEmpty())
			{
				var newToken = userService.ResetGeneralToken(email);
				if (newToken != null)
					return Json(newToken);
			}

			return Json(new { errorMessage = resourceManager.GetTranslation("UserDoesNotExist") });
		}

		[AllowAnonymous]
		[HttpGet]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult ResetPassword()
		{
			return View("ResetPassword", new CrmModel());
		}

		public virtual ActionResult RedirectToResetPassword(IList<RuleViolation> ruleViolations)
		{
			var model = new CrmModel();
			model.AddRuleViolations(ruleViolations);
			return View("ResetPassword", model);
		}

		[AllowAnonymous]
		[HttpPost]
		[ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
		public virtual ActionResult ResetPassword(string email)
		{
			var ruleViolations = new List<RuleViolation>();

			if (ruleViolations.Count > 0)
			{
				return RedirectToResetPassword(ruleViolations);
			}

			var user = userService.GetUsers().GetByEmail(email);
			if (user != null)
			{
				var emailsInLastHour = passwordResetTokenRepository.GetAll().Count(x => x.CreateDate >= DateTime.Now.AddHours(-1) && x.Username == user.Id);
				var maxEmailsPerHour = appSettingsProvider.GetValue(MainPlugin.Settings.PasswordReset.MaxEmailsPerHour);
				if (maxEmailsPerHour <= 0 || emailsInLastHour < maxEmailsPerHour)
				{
					var passwordResetToken = passwordResetTokenFactory();
					var expirationInMinutes = appSettingsProvider.GetValue(MainPlugin.Settings.PasswordReset.ExpirationInMinutes);
					if (expirationInMinutes > 0)
					{
						passwordResetToken.ExpiryDate = DateTime.Now.AddMinutes(expirationInMinutes);
					}

					passwordResetToken.Id = Guid.NewGuid();
					passwordResetToken.Username = user.Id;
					passwordResetTokenRepository.SaveOrUpdate(passwordResetToken);
					var message = messageFactory();
					message.Recipients.Add(email);
					message.Subject = "Your new password";
					var hostUri = site.GetExtension<DomainExtension>().HostUri.ToString();
					var resetPasswordUrl = "Account/PasswordReset/" + passwordResetToken.Id;
					message.Body = $"<a href=\"{hostUri.AppendIfMissing("/")}{resetPasswordUrl}\">{resourceManager.GetTranslation("ResetPassword", user.DefaultLanguageKey)}</a>";
					message.IsBodyHtml = true;
					message.LanguageKey = user.DefaultLanguageKey;
					messageRepository.SaveOrUpdate(message);
				}
			}

			TempData["PasswordResetLinkSent"] = true;

			var returnUrl = Request.ExtractBackUrlForCurrentUrl();
			if (returnUrl.IsNullOrEmpty())
			{
				return RedirectToAction("Login");
			}

			return Redirect(returnUrl);
		}

		[AllowAnonymous]
		[HttpGet]
		public virtual ActionResult PasswordReset(Guid id)
		{
			var passwordResetToken = passwordResetTokenRepository.GetAll().FirstOrDefault(x => x.Id == id && (!x.ExpiryDate.HasValue || x.ExpiryDate.Value > DateTime.Now));
			if (passwordResetToken == null)
			{
				return NotFound();
			}

			var model = GetLoginViewModel();
			model.PasswordResetToken = id;
			return View("Login", model);
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult PasswordReset(Guid id, string newPassword, string newPassword2)
		{
			var passwordResetToken = passwordResetTokenRepository.GetAll().FirstOrDefault(x => x.Id == id && (!x.ExpiryDate.HasValue || x.ExpiryDate.Value > DateTime.Now));
			if (passwordResetToken == null)
			{
				return NotFound();
			}

			if (newPassword.IsNullOrEmpty() || !newPassword.Equals(newPassword2, StringComparison.Ordinal))
			{
				var model = GetLoginViewModel();
				model.PasswordResetToken = id;
				model.RuleViolations.Add(new RuleViolation("PasswordsDoNotMatch"));
				return View("Login", model);
			}

			var user = userService.GetUser(passwordResetToken.Username);
			authenticationService.ResetPassword(user, newPassword);
			authenticationService.SignOutDevices(user);
			userService.SaveUser(user);
			return new RedirectResult("~/Account/Login");
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		[HttpPost]
		public virtual ActionResult ChangePassword(string oldPassword, string newPassword, string newPassword2)
		{
			var errorMessages = new List<string>();
			if (oldPassword.IsNullOrEmpty() || !authenticationService.Validate(userService.CurrentUser, oldPassword))
			{
				var translatedError = resourceManager.GetTranslation("OldPasswordIsIncorrect");
				errorMessages.Add(translatedError);
				return Json(errorMessages);
			}

			if (!newPassword.IsNullOrEmpty() && !newPassword.Equals(newPassword2, StringComparison.Ordinal))
			{
				var translatedError = resourceManager.GetTranslation("PasswordsDoNotMatch");
				errorMessages.Add(translatedError);
				return Json(errorMessages);
			}

			var passwordStrengthResult = Core.EvaluatePassword(newPassword);
			if (passwordStrengthResult.Score < site.GetExtension<DomainExtension>().MinPasswordStrength)
			{
				return new JsonResult(new { ErrorMessageKey = "RuleViolation.PasswordTooWeak", passwordStrengthResult.Feedback, passwordStrengthResult.Score });
			}

			var user = userService.CurrentUser;

			var ruleViolations = ruleValidationService.GetRuleViolations(user);
			foreach (var ruleViolation in ruleViolations)
			{
				var translatedError = resourceManager.GetTranslation($"RuleViolation.{ruleViolation.RuleClass}")
					.WithArgs(resourceManager.GetTranslation(ruleViolation.PropertyName));
				errorMessages.Add(translatedError);
			}

			if (errorMessages.Count > 0)
			{
				return Json(errorMessages);
			}

			authenticationService.ResetPassword(user, newPassword.IsNullOrEmpty() ? oldPassword : newPassword);
			var fingerprint = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "Fingerprint")?.Value;
			authenticationService.SignOutDevices(user, fingerprint.IsNotNullOrWhiteSpace() ? fingerprint : null);
			userService.SaveUser(user);
			return Json(errorMessages);
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual ActionResult EvaluatePassword(string password)
		{
			var passwordStrengthResult = Core.EvaluatePassword(password);
			return new JsonResult(new { passwordStrengthResult.Feedback, passwordStrengthResult.Score, MinPasswordStrength = site.GetExtension<DomainExtension>().MinPasswordStrength });
		}

		[RequiredPermission(PermissionName.Settings, Group = MainPlugin.PermissionGroup.UserAccount)]
		public virtual JsonResult Autocomplete(string q, int limit)
		{
			var users = userService.GetUsers().Where(x => x.DisplayName.Contains(q, StringComparison.OrdinalIgnoreCase)).Take(limit).ToList();

			return Json(users);
		}

		[RequiredPermission(MainPlugin.PermissionName.UpdateStatus, Group = MainPlugin.PermissionGroup.UserAccount)]
		[HttpPost]
		public virtual ActionResult UpdateStatus(User user)
		{
			userService.UpdateStatus(user);
			return RedirectToRoute("Dashboard", new { pageNumber = "Page1" });
		}

		protected virtual string GetUserData()
		{
			if (HttpContext == null)
			{
				return String.Empty;
			}

			var builder = new StringBuilder();
			builder.AppendLine("Request Data");
			var user = HttpContext.User?.Identity.GetUser(userService);
			if (user != null)
			{
				builder.AppendLine("User: " + user.Id.HideSensitiveDataFromName());
			}

			builder.AppendLine("Culture: " + Thread.CurrentThread.CurrentCulture.Name);
			builder.AppendLine("UI Culture: " + Thread.CurrentThread.CurrentUICulture.Name);
			builder.AppendLine("Requested Url: " + HttpContext.Request.Path);
			builder.AppendLine("Referrer Url: " + HttpContext.Request.GetTypedHeaders().Referer);
			builder.AppendLine("Http Method: " + HttpContext.Request.Method);
			builder.AppendLine("Application Version: " + Assembly.GetAssembly(typeof(AccountController)).GetName().Version);
			builder.AppendLine();

			if (HttpContext.Request.Query.Count > 0)
			{
				builder.AppendLine("Query string");
				builder.AppendLine(HttpContext.Request.QueryString.Value);
			}

			builder.AppendLine("Browser Capabilities");
			builder.AppendLine("IP Address: " + HttpContext.Connection.RemoteIpAddress);
			builder.AppendLine("User-Agent: " + HttpContext.Request.UserAgent());
			builder.AppendLine("Identified as mobile device: " + browserCapabilities.IsMobileDevice);
			return builder.ToString().HideSensitiveDataFromUrl();
		}

		protected virtual string GetCordovaDeepLink()
		{
			var hostUri = site.GetExtension<DomainExtension>().HostUri;
			var scheme = hostUri.Scheme.Equals("http") ? "lmobile" : "lmobiles";
			var path = hostUri.PathAndQuery.AppendIfMissing("/") + "Main/Home/Index";
			var uriBuilder = hostUri.IsDefaultPort
				? new UriBuilder(scheme, hostUri.Host) { Path = path }
				: new UriBuilder(scheme,
					hostUri.Host,
					hostUri.Port,
					path);
			return uriBuilder.ToString();
		}

		protected virtual LoginViewModel GetLoginViewModel()
		{
			string avatar = null;
			string userDisplayName = null;
			var loginCookie = String.IsNullOrWhiteSpace(Request?.Cookies["Login"]) ? null : Request.Cookies["Login"];

			var viewModel = new LoginViewModel
			{
				Avatar = avatar,
				CanResetPassword = authenticationService.PasswordResetSupported,
				CordovaDeepLink = GetCordovaDeepLink(),
				LoginCookie = loginCookie,
				LoginType = authenticationService.LoginTypeKey,
				UserDisplayName = userDisplayName,
				UseOpenIdAuthentication = appSettingsProvider.GetValue(MainPlugin.Settings.System.OpenIdAuthentication.UseOpenIdAuthentication)
			};

			if (Request != null && Request.UserAgent() != null && HttpContext.IsCordovaApp())
			{
				viewModel.IsInCordovaApp = true;
			}

			viewModel.AppInstallUrlDescriptors = ComposeAppInstallUrlDescriptors(viewModel);

			return viewModel;
		}

		public virtual ActionResult UserProfile()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 50)]
		[RequiredPermission(nameof(Device), Group = PermissionGroup.WebApiRead)]
		public virtual ActionResult UserProfileDevicesTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 50)]
		[RequiredPermission(nameof(Device), Group = PermissionGroup.WebApiRead)]
		public virtual ActionResult UserProfileDevicesTabHeader()
		{
			return PartialView();
		}

		public virtual ActionResult RemoveDevice()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 90)]
		public virtual ActionResult UserProfileAccountInfoTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 90)]
		public virtual ActionResult UserProfileAccountInfoTabHeader()
		{
			return PartialView();
		}
		public virtual ActionResult UserAvatar()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTab", Priority = 20)]
		public virtual ActionResult UserProfileTokenTab()
		{
			return PartialView();
		}
		[RenderAction("UserProfileTabHeader", Priority = 20)]
		public virtual ActionResult UserProfileTokenTabHeader()
		{
			return PartialView();
		}

		[HttpGet]
		public virtual ActionResult GetTurnServerInfo()
		{
			const ushort expiry = 8400;
			const string secret = "2ZP12msSnxnvH3z8N1VUb3P6";
			var cipher = new HMACSHA1(Encoding.UTF8.GetBytes(secret));
			var text = (DateTimeOffset.Now.ToUnixTimeSeconds() + expiry).ToString();
			var result = Convert.ToBase64String(cipher.ComputeHash(Encoding.UTF8.GetBytes(text)));
			return new ContentResult { Content = $"{text}:{result}" };
		}

		protected virtual string GenerateXmppLoginToken(User user)
		{
			var cipher = new HMACSHA384(Encoding.ASCII.GetBytes(user.UserId.ToString("N")));
			var result = cipher.ComputeHash(Encoding.UTF8.GetBytes(user.GeneralToken));
			return String.Concat(Array.ConvertAll(result, x => x.ToString("x2")));
		}

		[HttpGet]
		public virtual ActionResult GetXmppToken() => new ContentResult { Content = GenerateXmppLoginToken(userService.CurrentUser) };

		public class XmppLoginData
		{
			public string username;
			public string token;
		}

		[AllowAnonymous]
		[HttpPost]
		public virtual StatusCodeResult ValidateUserXmppToken([FromBody] XmppLoginData data)
		{
			if (data.username.IsNullOrEmpty() || data.token.IsNullOrEmpty())
			{
				return new StatusCodeResult(403);
			}

			var user = userService.GetUser(data.username);
			if (user == null)
			{
				return new StatusCodeResult(403);
			}

			return GenerateXmppLoginToken(user) == data.token ? new StatusCodeResult(200) : new StatusCodeResult(403);
		}

		[HttpPost]
		public virtual ActionResult ValidatePassword(string password)
		{
			if (!authenticationService.Validate(userService.CurrentUser, password))
			{
				return new JsonResult(new { ErrorMessageKey = "InvalidPassword" });
			}

			return new EmptyResult();
		}

		[HttpGet]
		public virtual ActionResult GetNewTotpAuthorizationCredentialsForUser()
		{
			var authorizationKey = totpService.GenerateNewAuthorizationKey();
			return Json(new
			{
				authorizationKey,
				authorizationUri = totpService.ComposeAuthorizationUri(authorizationKey, userService.CurrentUser.Email)
			});
		}

		[HttpPost]
		public virtual ActionResult SetTotpAuthenticationForCurrentUser([FromBody] SetTotpAuthenticationForCurrentUserRequest request)
		{
			var messageList = new Dictionary<string, string>();

			if (request == null || request.AuthorizationKey == null)
			{
				return new StatusCodeResult(StatusCodes.Status403Forbidden);
			}

			var tokenIsValid = totpService.ValidateToken(request.AuthorizationKey, request.Token);
			if (tokenIsValid)
			{
				userService.CurrentUser.TotpAuthorizationKey = totpService.EncryptAuthorizationKey(userService.CurrentUser.UserId, request.AuthorizationKey);
				userService.SaveUser(userService.CurrentUser);
				messageList.Add("MESSAGE", resourceManager.GetTranslation("TotpAuthenticationSuccessfullySetUp"));
			}
			else
			{
				messageList.Add("ERROR", resourceManager.GetTranslation("InvalidTotpToken"));
			}

			return Json(messageList.ToArray());
		}

		public class SetTotpAuthenticationForCurrentUserRequest
		{
			public string AuthorizationKey { get; set; }
			public string Token { get; set; }
		}

		[HttpPost]
		public virtual ActionResult RemoveTotpAuthenticatorOfCurrentUser()
		{
			return RemoveTotpAuthenticator(userService.CurrentUser);
		}

		[HttpPost]
		[RequiredPermission(MainPlugin.PermissionName.ResetUserTotp, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult RemoveTotpAuthenticator(string userName)
		{
			var user = userService.GetUser(userName);

			return RemoveTotpAuthenticator(user);
		}

		protected virtual JsonResult RemoveTotpAuthenticator(User user)
		{
			var messageList = new Dictionary<string, string>();

			if (user == null)
			{
				messageList.Add("ERROR", resourceManager.GetTranslation("UserDoesNotExist"));
				return Json(messageList.ToArray());
			}

			if (user.TotpAuthorizationKey != null)
			{
				user.TotpAuthorizationKey = null;
				userService.SaveUser(user);
				totpService.RemoveHash(user.UserId);

				messageList.Add("MESSAGE", resourceManager.GetTranslation("TotpAuthenticationSuccessfullyRemoved"));
			}

			return Json(messageList.ToArray());
		}

		[HttpPost]
		[RequiredPermission(MainPlugin.PermissionName.ResetUserTotp, Group = PermissionGroup.UserAdmin)]
		public virtual ActionResult RemoveAllTotpAuthenticators()
		{
			var batchSize = 100;

			var usersWithTOTP = userService.GetUsersQuery()
				.Where(u => u.TotpAuthorizationKey != null);
			var userCounter = 0;

			var session = sessionProvider.GetSession();

			using (var transaction = session.BeginTransaction())
			{
				try
				{
					while (true)
					{
						var userBatch = usersWithTOTP.Skip(userCounter).Take(batchSize).ToList();
						if (userBatch.Count == 0)
						{
							break;
						}

						foreach (var user in userBatch)
						{
							user.TotpAuthorizationKey = null;
							userService.SaveUser(user);
						}

						userCounter += batchSize;
					}

					transaction.Commit();
				}
				catch
				{
					transaction.Rollback();
					return new StatusCodeResult(StatusCodes.Status500InternalServerError);
				}
			}

			totpService.ClearHashStore();

			return new StatusCodeResult(StatusCodes.Status200OK);
		}

		public virtual List<MobileAppInstallSourceDescriptor> ComposeAppInstallUrlDescriptors(LoginViewModel loginViewModel)
		{
			var result = new List<MobileAppInstallSourceDescriptor>();

			if (pluginProvider.ActivePluginDescriptors.Any(x => x.PluginName == "Crm.Service"))
			{
				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidServiceAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidServiceAppLink),
						Caption = "Service App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-service.webp",
						Platform = "Android"
					});
				}

				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosServiceAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosServiceAppLink),
						Caption = "Service App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-service.webp",
						Platform = "AppleIos"
					});
				}

				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10ServiceAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10ServiceAppLink),
						Caption = "Service App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-service.webp",
						Platform = "Windows10"
					});
				}
			}

			if (pluginProvider.ActivePluginDescriptors.Any(x => x.PluginName == "Crm.Order"))
			{
				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidSalesAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AndroidSalesAppLink),
						Caption = "Sales App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-sales.webp",
						Platform = "Android"
					});
				}

				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosSalesAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.AppleIosSalesAppLink),
						Caption = "Sales App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-sales.webp",
						Platform = "AppleIos"
					});
				}

				if (!string.IsNullOrEmpty(appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10SalesAppLink)))
				{
					result.Add(new MobileAppInstallSourceDescriptor()
					{
						Url = appSettingsProvider.GetValue(MainPlugin.Settings.Cordova.Windows10SalesAppLink),
						Caption = "Sales App",
						LogoPath = "~/Plugins/Main/Content/img/app-logo-sales.webp",
						Platform = "Windows10"
					});
				}
			}

			return result;
		}

		protected virtual bool IsMultiFactorAuthenticationRequired(User user)
		{
			var multiFactorAuthenticationModeKey = site.GetExtension<DomainExtension>().MultiFactorAuthenticationModeKey;
			if (user.TotpAuthorizationKey != null)
			{
				return true;
			}

			if (multiFactorAuthenticationModeKey == MultiFactorAuthenticationMode.MandatoryForAllUsersKey)
			{
				return true;
			}

			if (multiFactorAuthenticationModeKey == MultiFactorAuthenticationMode.MandatoryForSpecificUserGroupsKey
				&& user.Usergroups.Any(x => x.MultiFactorAuthenticationMandatory))
			{
				return true;
			}

			return false;
		}

		public AccountController(
			IAuthenticationService authenticationService,
			IUserService userService,
			IBrowserCapabilities browserCapabilities,
			IEnumerable<IRedirectProvider> redirectProviders,
			Site site,
			IAppSettingsProvider appSettingsProvider,
			IResourceManager resourceManager,
			IRuleValidationService ruleValidationService,
			ILog logger,
			IEnumerable<ILoginValidator> loginValidators,
			Func<User> userFactory,
			IRepositoryWithTypedId<Message, Guid> messageRepository,
			Func<Message> messageFactory,
			IRepositoryWithTypedId<Device, Guid> deviceRepository,
			IPluginProvider pluginProvider,
			IRepositoryWithTypedId<PasswordResetToken, Guid> passwordResetTokenRepository,
			Func<PasswordResetToken> passwordResetTokenFactory,
			ITotpService totpService,
			ISessionProvider sessionProvider)
		{
			this.authenticationService = authenticationService;
			this.userService = userService;
			this.browserCapabilities = browserCapabilities;
			this.redirectProviders = redirectProviders;
			this.site = site;
			this.appSettingsProvider = appSettingsProvider;
			this.resourceManager = resourceManager;
			this.ruleValidationService = ruleValidationService;
			this.logger = logger;
			this.loginValidators = loginValidators;
			this.userFactory = userFactory;
			this.messageFactory = messageFactory;
			this.messageRepository = messageRepository;
			this.deviceRepository = deviceRepository;
			this.pluginProvider = pluginProvider;
			this.passwordResetTokenRepository = passwordResetTokenRepository;
			this.passwordResetTokenFactory = passwordResetTokenFactory;
			this.totpService = totpService;
			this.sessionProvider = sessionProvider;
		}
	}
}
