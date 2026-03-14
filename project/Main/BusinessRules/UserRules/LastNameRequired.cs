namespace Main.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class LastNameRequired : RequiredRule<User>
	{
		public LastNameRequired()
		{
			Init(u => u.LastName);
		}
	}
}
