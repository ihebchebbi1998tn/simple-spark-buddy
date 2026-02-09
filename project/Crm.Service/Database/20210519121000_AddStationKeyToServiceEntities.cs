namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
    using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
    using System.Data;

	[Migration(20210519121000)]
	public class AddStationKeyToServiceEntities : Migration
	{
		public override void Up()
		{
			var stationColumn = new Column("StationKey", DbType.Guid, ColumnProperty.Null);
			var tables = new[]
			{
				"[SMS].[ServiceNotifications]",
				"[SMS].[InstallationHead]"
			};
			foreach(var table in tables)
			{
				var shortName = table.Split('[')[2].Split(']')[0];
				Database.AddColumnIfNotExisting(table, stationColumn);
				Database.AddForeignKey($"FK_{shortName}_Station", table, "StationKey", "[CRM].[Station]", "StationId");
			}
		}
	}
}