namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240506152505)]
	public class AddTimeZoneToServiceOrderTimePostings : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("TimeZoneFrom", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderTimePostings]", new Column("TimeZoneTo", DbType.String, 50, ColumnProperty.Null));
		}
	}
}
