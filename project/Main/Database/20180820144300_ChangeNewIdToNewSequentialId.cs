namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180820144300)]
	public class ChangeNewIdToNewSequentialId : Migration
	{
		public override void Up()
		{
			Database.DropDefault("CRM", "BusinessRelationship", "BusinessRelationshipId");
			Database.ExecuteNonQuery("ALTER TABLE [CRM].[BusinessRelationship] ADD CONSTRAINT [DF_BusinessRelationship_BusinessRelationshipId] DEFAULT (newsequentialid()) FOR [BusinessRelationshipId]");

			if (Database.DefaultConstraintExists("[CRM].[User]", "DF_User_UserID"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [CRM].[User] DROP CONSTRAINT [DF_User_UserID]");
			}

			Database.ExecuteNonQuery("ALTER TABLE [CRM].[User] ADD CONSTRAINT [DF_User_UserID] DEFAULT (newsequentialid()) FOR [UserID]");
		}
	}
}
