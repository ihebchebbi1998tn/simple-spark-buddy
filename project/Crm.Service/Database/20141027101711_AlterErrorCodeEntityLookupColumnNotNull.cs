namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141027101711)]
	public class AlterErrorCodeEntityLookupColumnNotNull : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("SMS.ErrorCode", "CreateUser"))
			{
				Database.ExecuteNonQuery(@"UPDATE SMS.ErrorCode SET CreateUser = COALESCE(CreateUser, 'Initial')");
				Database.ExecuteNonQuery("ALTER TABLE SMS.ErrorCode ALTER COLUMN CreateUser nvarchar(100) NOT NULL");
			}
			if (Database.ColumnExists("SMS.ErrorCode", "ModifyUser"))
			{
				Database.ExecuteNonQuery(@"UPDATE SMS.ErrorCode SET ModifyUser = COALESCE(ModifyUser, 'Initial')");
				Database.ExecuteNonQuery("ALTER TABLE SMS.ErrorCode ALTER COLUMN ModifyUser nvarchar(100) NOT NULL");
			}
			if (Database.ColumnExists("SMS.ErrorCode", "CreateDate"))
			{
				Database.ExecuteNonQuery(@"UPDATE SMS.ErrorCode SET CreateDate = COALESCE(CreateDate, getutcdate())");
				Database.ExecuteNonQuery("ALTER TABLE SMS.ErrorCode ALTER COLUMN CreateDate datetime NOT NULL");
			}
			if (Database.ColumnExists("SMS.ErrorCode", "ModifyDate"))
			{
				Database.ExecuteNonQuery(@"UPDATE SMS.ErrorCode SET ModifyDate = COALESCE(ModifyDate, getutcdate())");
				Database.ExecuteNonQuery("ALTER TABLE SMS.ErrorCode ALTER COLUMN ModifyDate datetime NOT NULL");
			}
			if (Database.ColumnExists("SMS.ErrorCode", "IsActive"))
			{
				Database.ExecuteNonQuery(@"UPDATE SMS.ErrorCode SET IsActive = COALESCE(IsActive, 1)");
				Database.ExecuteNonQuery("ALTER TABLE SMS.ErrorCode ALTER COLUMN IsActive bit NOT NULL");
			}
		}
		public override void Down()
		{
		}
	}
}