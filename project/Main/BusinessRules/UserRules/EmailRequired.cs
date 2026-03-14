namespace Main.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class EmailRequired : RequiredRule<User>
	{
		public EmailRequired()
		{
			Init(u => u.Email, "EMail");
		}
	}
}
