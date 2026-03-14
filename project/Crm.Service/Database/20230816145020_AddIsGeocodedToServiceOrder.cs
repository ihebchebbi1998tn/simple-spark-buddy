namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20230816145020)]
	public class AddIsGeocodedToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[SMS].[ServiceOrderHead]", new Column("IsGeocoded")
			{
				Type = DbType.Boolean,
				ColumnProperty = ColumnProperty.NotNull,
				DefaultValue = false
			});

			Database.ExecuteNonQuery("UPDATE [SMS].[ServiceOrderHead] SET IsGeocoded = 'true' WHERE Latitude IS NOT NULL AND Longitude IS NOT NULL");
		}
	}
}
