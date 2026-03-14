namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240618163005)]
	public class AddTimeZoneToServiceOrderDispatch : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceOrderDispatch]", new Column("TimeZone", DbType.String, 50, ColumnProperty.Null));
		}
	}
}
