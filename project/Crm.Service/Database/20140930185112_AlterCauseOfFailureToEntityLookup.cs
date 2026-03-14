namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140930185112)]
	public class AlterCauseOfFailureToEntityLookup : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("LU.CauseOfFailure", "CreateUser"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE LU.CauseOfFailure ADD CreateUser nvarchar(100)");				
			}
			if (!Database.ColumnExists("LU.CauseOfFailure", "ModifyUser"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE LU.CauseOfFailure ADD ModifyUser nvarchar(100)");
			}
			if (!Database.ColumnExists("LU.CauseOfFailure", "CreateDate"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE LU.CauseOfFailure ADD CreateDate datetime");
			}
			if (!Database.ColumnExists("LU.CauseOfFailure", "ModifyDate"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE LU.CauseOfFailure ADD ModifyDate datetime");
			}
			if (!Database.ColumnExists("LU.CauseOfFailure", "IsActive"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE LU.CauseOfFailure ADD IsActive bit");
			}
		}
		public override void Down()
		{
		}
	}
}