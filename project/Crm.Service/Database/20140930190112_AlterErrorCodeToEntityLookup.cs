namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140930190112)]
	public class AlterErrorCodeToEntityLookup : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ErrorCode", "CreateUser"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ErrorCode ADD CreateUser nvarchar(100)");				
			}
			if (!Database.ColumnExists("SMS.ErrorCode", "ModifyUser"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ErrorCode ADD ModifyUser nvarchar(100)");
			}
			if (!Database.ColumnExists("SMS.ErrorCode", "CreateDate"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ErrorCode ADD CreateDate datetime");
			}
			if (!Database.ColumnExists("SMS.ErrorCode", "ModifyDate"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ErrorCode ADD ModifyDate datetime");
			}
			if (!Database.ColumnExists("SMS.ErrorCode", "IsActive"))
			{
				Database.ExecuteNonQuery(@"ALTER TABLE SMS.ErrorCode ADD IsActive bit");
			}
		}
		public override void Down()
		{
		}
	}
}