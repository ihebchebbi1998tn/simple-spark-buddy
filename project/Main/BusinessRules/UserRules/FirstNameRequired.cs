namespace Main.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class FirstNameRequired : RequiredRule<User>
	{
		public FirstNameRequired()
		{
			Init(u => u.FirstName, "Firstname");
		}
	}
}
