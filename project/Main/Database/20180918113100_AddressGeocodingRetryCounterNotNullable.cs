namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180918113100)]
	public class AddressGeocodingRetryCounterNotNullable : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [CRM].[Address] WHERE GeocodingRetryCounter is null") > 0)
			{
				Database.ExecuteNonQuery(
					@"UPDATE [CRM].[Address]
					SET GeocodingRetryCounter = 0, ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20180918113100'
					WHERE GeocodingRetryCounter is null");
			}

			Database.ExecuteNonQuery(
				@"ALTER TABLE [CRM].[Address] ADD DEFAULT 0 FOR GeocodingRetryCounter
				ALTER TABLE [CRM].[Address] ALTER COLUMN GeocodingRetryCounter INT NOT NULL");
		}
	}
}
