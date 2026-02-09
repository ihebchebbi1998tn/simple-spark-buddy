namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210630152000)]
	public class AddStatisticsKeyToServiceCase : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyProductType", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyMainAssembly", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeySubAssembly", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyAssemblyGroup", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyFaultImage", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyRemedy", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyCause", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyWeighting", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceNotifications]", "StatisticsKeyCauser", DbType.String, 50, ColumnProperty.Null);
		}
	}
}