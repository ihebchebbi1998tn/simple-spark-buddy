namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150414112602)]
	public class AddUserGroupKeyFkToCrmUserUserGroup : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_UserUserGroup_UserGroup'") == 0)
			{
				Database.ExecuteNonQuery("DELETE uug FROM [CRM].[UserUserGroup] uug LEFT OUTER JOIN [CRM].[UserGroup] ug ON uug.[UserGroupKey] = ug.[UserGroupId] WHERE ug.[UserGroupId] IS NULL");
				Database.AddForeignKey("FK_UserUserGroup_UserGroup", "[CRM].[UserUserGroup]", "UserGroupKey", "[CRM].[UserGroup]", "UserGroupId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}