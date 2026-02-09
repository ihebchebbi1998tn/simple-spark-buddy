namespace Crm.Service.Team.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240417145500)]

	public class AddIsServiceTeamToCrmUsergroup : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("CRM.Usergroup", "IsServiceTeam"))
			{
				Database.AddColumn("CRM.Usergroup", new Column("IsServiceTeam", DbType.Boolean, ColumnProperty.NotNull, false));
				Database.ExecuteNonQuery("UPDATE CRM.Usergroup SET IsServiceTeam = 1 WHERE MainResourceId IS NOT NULL");
				Database.ExecuteNonQuery("UPDATE CRM.Usergroup SET IsServiceTeam = 1 FROM CRM.Usergroup JOIN CRM.ArticleUserGroupRelationship ON CRM.Usergroup.UsergroupId = CRM.ArticleUserGroupRelationship.UserGroupKey");
				Database.ExecuteNonQuery("UPDATE CRM.Usergroup SET IsServiceTeam = 1 FROM CRM.Usergroup JOIN SMS.ServiceOrderDispatch ON CRM.Usergroup.UsergroupId = SMS.ServiceOrderDispatch.TeamId");
			}
		}
	}
}
