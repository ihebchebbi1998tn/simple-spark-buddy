namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220105140300)]
	public class ChangeTimePostingEntityUserColLength : Migration
	{
		public override void Up()
		{
			var timePostingModifyUserLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'SMS' AND
								TABLE_NAME = 'ServiceOrderTimePostings' AND
								COLUMN_NAME = 'ModifyUser'");

			var timePostingCreateUserLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'SMS' AND
								TABLE_NAME = 'ServiceOrderTimePostings' AND
								COLUMN_NAME = 'CreateUser'");

			if (timePostingModifyUserLength < 256)
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN [ModifyUser] NVARCHAR(256) NULL");
			if (timePostingCreateUserLength < 256)
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN [CreateUser] NVARCHAR(256) NULL");
		}
	}
}