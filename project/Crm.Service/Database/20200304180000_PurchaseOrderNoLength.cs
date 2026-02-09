namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200304180000)]
	public class PurchaseOrderNoLength : Migration
	{
		public override void Up()
		{
			var length = (int)Database.ExecuteScalar(
				@"	SELECT CHARACTER_MAXIMUM_LENGTH
						FROM INFORMATION_SCHEMA.COLUMNS
						WHERE TABLE_SCHEMA = 'SMS' AND
							TABLE_NAME = 'ServiceOrderHead' AND
							COLUMN_NAME = 'PurchaseOrderNo'");
			if (length < 256)
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN [PurchaseOrderNo] NVARCHAR(256) NULL");
			}
		}
	}
}