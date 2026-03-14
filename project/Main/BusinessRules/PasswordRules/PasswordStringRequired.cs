namespace Main.BusinessRules.PasswordRules
{
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation.BaseRules;

	public class PasswordStringRequired : RequiredRule<string>
	{
		private readonly IAuthenticationService authenticationService;
		protected override bool IsIgnoredFor(string password)
		{
			return !authenticationService.PasswordChangeSupported;
		}

		public PasswordStringRequired(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
			Init(p => p, "Password");
		}
	}
}
