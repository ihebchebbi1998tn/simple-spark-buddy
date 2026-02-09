namespace Main.BusinessRules.PasswordRules
{
	using Crm.Library.Model;
	using Crm.Library.Services.Interfaces;
	using Crm.Library.Validation.BaseRules;

	public class PasswordRequired : RequiredRule<User>
	{
		private readonly IAuthenticationService authenticationService;
		protected override bool IsIgnoredFor(User user)
		{
			return !authenticationService.PasswordChangeSupported;
		}

		public PasswordRequired(IAuthenticationService authenticationService)
		{
			this.authenticationService = authenticationService;
			Init(u => u.Password);
		}
	}
}
