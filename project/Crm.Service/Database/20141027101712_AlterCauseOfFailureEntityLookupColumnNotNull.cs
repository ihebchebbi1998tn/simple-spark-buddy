namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141027101712)]
	public class AlterCauseOfFailureEntityLookupColumnNotNull : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("LU.CauseOfFailure", "CreateUser"))
			{
				Database.ExecuteNonQuery(@"UPDATE LU.CauseOfFailure SET CreateUser = COALESCE(CreateUser, 'Initial')");
				Database.ExecuteNonQuery("ALTER TABLE LU.CauseOfFailure ALTER COLUMN CreateUser nvarchar(100) NOT NULL");
			}
			if (Database.ColumnExists("LU.CauseOfFailure", "ModifyUser"))
			{
				Database.ExecuteNonQuery(@"UPDATE LU.CauseOfFailure SET ModifyUser = COALESCE(ModifyUser, 'Initial')");
				Database.ExecuteNonQuery("ALTER TABLE LU.CauseOfFailure ALTER COLUMN ModifyUser nvarchar(100) NOT NULL");
			}
			if (Database.ColumnExists("LU.CauseOfFailure", "CreateDate"))
			{
				Database.ExecuteNonQuery(@"UPDATE LU.CauseOfFailure SET CreateDate = COALESCE(CreateDate, getutcdate())");
				Database.ExecuteNonQuery("ALTER TABLE LU.CauseOfFailure ALTER COLUMN CreateDate datetime NOT NULL");
			}
			if (Database.ColumnExists("LU.CauseOfFailure", "ModifyDate"))
			{
				Database.ExecuteNonQuery(@"UPDATE LU.CauseOfFailure SET ModifyDate = COALESCE(ModifyDate, getutcdate())");
				Database.ExecuteNonQuery("ALTER TABLE LU.CauseOfFailure ALTER COLUMN ModifyDate datetime NOT NULL");
			}
			if (Database.ColumnExists("LU.CauseOfFailure", "IsActive"))
			{
				Database.ExecuteNonQuery(@"UPDATE LU.CauseOfFailure SET IsActive = COALESCE(IsActive, 1)");
				Database.ExecuteNonQuery("ALTER TABLE LU.CauseOfFailure ALTER COLUMN IsActive bit NOT NULL");
			}
		}
		public override void Down()
		{
		}
	}
}