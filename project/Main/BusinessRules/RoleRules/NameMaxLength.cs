namespace Main.BusinessRules.RoleRules
{
	using Crm.Library.Validation;
	using Crm.Library.Validation.BaseRules;

	using LMobile.Unicore;

	[Rule]
	public class NameMaxLength : MaxLengthRule<PermissionSchemaRole>
	{
		public NameMaxLength()
		{
			Init(r => r.Name, 256);
		}
	}
}
