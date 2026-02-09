namespace Main.BusinessRules.UserRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class DefaultLanguageKeyRequired : RequiredRule<User>
	{
		public DefaultLanguageKeyRequired()
		{
			Init(u => u.DefaultLanguageKey);
		}
	}
}
