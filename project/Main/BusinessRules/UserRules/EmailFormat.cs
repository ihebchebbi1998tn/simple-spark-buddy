namespace Main.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class EmailFormat : EmailRule<User>
	{
		public EmailFormat()
		{
			Init(u => u.Email, "EMail");
		}
	}
}
