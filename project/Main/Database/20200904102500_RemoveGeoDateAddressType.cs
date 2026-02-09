namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200904102500)]
	public class RemoveGeoDateAddressType : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				DECLARE @val nvarchar(50);
				SET @val = (SELECT Value FROM [LU].[AddressType] WHERE Name = 'GeoDate' AND Language = 'en');
				IF @val IS NOT NULL
					BEGIN
						UPDATE [CRM].[Address] SET AddressTypeKey = '1' WHERE AddressTypeKey = @val;
						DELETE FROM [LU].[AddressType] WHERE Value = @val;
					END
			");
		}
	}
}
