namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413183800)]
	public class AddContactKeyFkToCrmTask : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Task_Contact'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE t SET t.[ContactKey] = NULL FROM [CRM].[Task] t LEFT OUTER JOIN [CRM].[Contact] c ON t.[ContactKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_Task_Contact", "[CRM].[Task]", "ContactKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}