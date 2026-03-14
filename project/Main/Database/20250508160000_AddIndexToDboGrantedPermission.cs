namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250508160000)]
	public class AddIndexToDboGrantedPermission : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("dbo.GrantedPermission"))
			{
				Database.ExecuteNonQuery(@"CREATE NONCLUSTERED INDEX [IX_GrantedPermission_UserId]
					ON [dbo].[GrantedPermission] ([UserId])
					INCLUDE ([PermissionId],[AuthDomainId],[TargetDomainId],[TargetEntityAuthDataId],[Circle],[ReferenceCount])");
			}
		}
	}
}
