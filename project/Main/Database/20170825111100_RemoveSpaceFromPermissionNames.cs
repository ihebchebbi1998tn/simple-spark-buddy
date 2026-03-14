namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170825111100)]
	public class RemoveSpaceFromPermissionNames : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [CRM].[Permission] SET [Name] = 'CreateServiceCase' WHERE [Name] = ' CreateServiceCase'");
			Database.ExecuteNonQuery("UPDATE [CRM].[Permission] SET [Name] = 'SkillAssign' WHERE [Name] = ' SkillAssign'");
		}
	}
}
