namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200127100500)]
	public class UpdateUserFirstAndLastNameToNotNull : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("UPDATE [CRM].[User] SET [FirstName] = '', [ModifyDate] = GETUTCDATE(), [ModifyUser] = 'Migration_20200127100500' WHERE [FirstName] IS NULL");
			var firstNameLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'User' AND
								COLUMN_NAME = 'FirstName'");
			Database.ExecuteNonQuery($"ALTER TABLE [CRM].[User] ALTER COLUMN [FirstName] NVARCHAR({firstNameLength}) NOT NULL");
			
			Database.ExecuteNonQuery("UPDATE [CRM].[User] SET [LastName] = '', [ModifyDate] = GETUTCDATE(), [ModifyUser] = 'Migration_20200127100500' WHERE [LastName] IS NULL");
			var lastNameLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'User' AND
								COLUMN_NAME = 'LastName'");
			Database.ExecuteNonQuery($"ALTER TABLE [CRM].[User] ALTER COLUMN [LastName] NVARCHAR({lastNameLength}) NOT NULL");

		}
	}
}
