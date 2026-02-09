namespace Main.BusinessRules.RoleRules
{
	using Crm.Library.Validation.BaseRules;

	using LMobile.Unicore;

	public class NameRequired : RequiredRule<PermissionSchemaRole>
	{
		public NameRequired()
		{
			Init(r => r.Name);
		}
	}
}
