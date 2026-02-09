namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200605141000)]
	public class AlterErrorCodeToString : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
					ALTER TABLE [SMS].[ServiceOrderDispatch]
					ALTER COLUMN [ErrorCode] nvarchar(50)");
		}
	}
}