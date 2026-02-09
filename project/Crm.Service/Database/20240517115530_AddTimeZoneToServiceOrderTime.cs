namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240517115530)]
	public class AddTimeZoneToServiceOrderTime : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimes]", new Column("TimeZone", DbType.String, 50, ColumnProperty.Null));
		}
	}
}
