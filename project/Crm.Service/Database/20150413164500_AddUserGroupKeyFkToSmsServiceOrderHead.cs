namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20150413164500)]
	public class AddUserGroupKeyFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_UserGroup'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[UserGroupKey] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[UserGroup] ug ON soh.[UserGroupKey] = ug.[UserGroupId] WHERE ug.[UserGroupId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_UserGroup", "[SMS].[ServiceOrderHead]", "UserGroupKey", "[CRM].[UserGroup]", "UserGroupId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}