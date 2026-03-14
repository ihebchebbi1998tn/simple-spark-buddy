namespace Main.ViewModels
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Main.Model;

	public class LoginViewModel : CrmModel
	{
		public string Avatar { get; set; }
		public bool CanResetPassword { get; set; }
		public string LoginCookie { get; set; }
		public string LoginType { get; set; }
		public bool NewPasswordSent { get; set; }
		public bool NoTenantAvailable { get; set; }
		public bool IsInCordovaApp { get; set; }
		public bool PasswordChangeRequired { get; set; }
		public Guid? PasswordResetToken { get; set; }
		public bool UseOpenIdAuthentication { get; set; }
		public bool ShowCordovaDeepLink
		{
			get { return !String.IsNullOrWhiteSpace(CordovaDeepLink); }
		}

		public bool SuggestApp
		{
			get
			{
				return !IsInCordovaApp
					&& AppInstallUrlDescriptors?.Any() == true
					&& !RuleViolations.Any()
					&& !NewPasswordSent;
			}
		}

		public string CordovaDeepLink { get; set; }
		public string UserDisplayName { get; set; }

		public List<MobileAppInstallSourceDescriptor> AppInstallUrlDescriptors { get; set; }
	}
}
