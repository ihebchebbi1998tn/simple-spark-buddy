namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240809122000)]
	public class AddNewFieldsToSmsServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("TravelTypeKey", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("DriverTypeKey", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("DistanceUnitKey", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("Distance", DbType.Int32, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("LocationFrom", DbType.String, 256, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("LocationTo", DbType.String, 256, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderTimePostings", new Column("LicensePlate", DbType.String, 50, ColumnProperty.Null));
		}
	}
}
