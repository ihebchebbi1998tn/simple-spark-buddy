namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180327113500)]
	public class RemoveAllowCreateUserFromCrmSite : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[Site]", "AllowCreateUser"))
			{
				Database.RemoveDefaultConstraint("[CRM].[Site]", "AllowCreateUser");
				Database.RemoveColumnIfExisting("[CRM].[Site]", "AllowCreateUser");
			}
		}
	}
}
