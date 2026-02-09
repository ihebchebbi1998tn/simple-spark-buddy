namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140930094912)]
	public class AlterComponentAndErrorCodeLookupsToString : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
											IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'DF_Components_Value')
											BEGIN ALTER TABLE [SMS].[Components] DROP CONSTRAINT [DF_Components_Value] END

											IF EXISTS (SELECT 1 FROM SYS.OBJECTS WHERE NAME = 'DF_ErrorCode_Value')
											BEGIN ALTER TABLE [SMS].[ErrorCode] DROP CONSTRAINT [DF_ErrorCode_Value] END

											ALTER TABLE SMS.Components ALTER COLUMN Value NVARCHAR(20)
											ALTER TABLE SMS.ErrorCode ALTER COLUMN Value NVARCHAR(100)
											ALTER TABLE SMS.ServiceOrderHead ALTER COLUMN ErrorCode NVARCHAR(100)
											ALTER TABLE SMS.ServiceOrderHead ALTER COLUMN Component NVARCHAR(20)
											");
		}
		public override void Down()
		{
		}
	}
}