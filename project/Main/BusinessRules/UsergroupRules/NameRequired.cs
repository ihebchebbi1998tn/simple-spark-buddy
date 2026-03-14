namespace Main.BusinessRules.UsergroupRules
{
	using Crm.Library.Model;
	using Crm.Library.Validation.BaseRules;

	public class NameRequired : RequiredRule<Usergroup>
	{
		public NameRequired()
		{
			Init(u => u.Name);
		}
	}
}
