namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180918114300)]
	public class ServiceOrderGeocodingRetryCounterNotNullable : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM [SMS].[ServiceOrderHead] WHERE GeocodingRetryCounter is null") > 0)
			{
				Database.ExecuteNonQuery(
					@"BEGIN TRANSACTION

						UPDATE [CRM].[Contact]
						SET ModifyDate = GETUTCDATE(), ModifyUser = 'Migration_20180918114300'
						WHERE ContactId in (SELECT ContactKey FROM [SMS].[ServiceOrderHead] WHERE GeocodingRetryCounter is null) 

						UPDATE [SMS].[ServiceOrderHead]
						SET GeocodingRetryCounter = 0
						WHERE GeocodingRetryCounter is null
 
					COMMIT;");
			}

			Database.ExecuteNonQuery(
				@"ALTER TABLE [SMS].[ServiceOrderHead] ADD DEFAULT 0 FOR GeocodingRetryCounter
				ALTER TABLE [SMS].[ServiceOrderHead] ALTER COLUMN GeocodingRetryCounter INT NOT NULL");
		}
	}
}