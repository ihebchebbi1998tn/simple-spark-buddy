namespace Main.BusinessRules.RoleRules
{
	using Crm.Library.Extensions;
	using Crm.Library.Unicore;
	using Crm.Library.Validation;

	using LMobile.Unicore;

	public class NameUnique : Rule<PermissionSchemaRole>
	{
		private readonly IAccessControlManager accessControlManager;

		// Methods
		public override bool IsSatisfiedBy(PermissionSchemaRole role)
		{
			if (role.Name.IsNullOrEmpty())
			{
				return true;
			}

			var roles = accessControlManager.ListPermissionSchemaRoles(UnicoreDefaults.DefaultPermissionSchema);

			foreach (var r in roles)
			{
				if (r.Name == role.Name && r.UId != role.UId)
				{
					return false;
				}
			}

			return true;
		}

		protected override RuleViolation CreateRuleViolation(PermissionSchemaRole role)
		{
			return RuleViolation(role, r => r.Name, "Role");
		}

		// Constructor
		public NameUnique(IAccessControlManager accessControlManager)
			: base(RuleClass.Unique)
		{
			this.accessControlManager = accessControlManager;
		}
	}
}
