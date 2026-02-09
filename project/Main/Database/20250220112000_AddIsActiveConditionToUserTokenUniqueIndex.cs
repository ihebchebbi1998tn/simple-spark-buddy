namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20250220112000)]
	public class AddIsActiveConditionToUserTokenUniqueIndex : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[CRM].[User]", "GeneralToken"))
			{
				if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE NAME = 'IX_UQ_GeneralToken'") == 1)
				{
					Database.ExecuteNonQuery("DROP INDEX [IX_UQ_GeneralToken] ON [CRM].[User]");
				}

				Database.ExecuteNonQuery("CREATE UNIQUE INDEX [IX_UQ_GeneralToken] ON [CRM].[User] ([GeneralToken]) WHERE [Discharged] = 0 AND [IsActive] = 1 AND [GeneralToken] IS NOT NULL");
			}
		}
	}
}
