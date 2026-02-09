namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220302140000)]
	public class AddStatisticsKeyToServiceOrder : Migration
	{
		public override void Up()
		{
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyProductType", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyMainAssembly", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeySubAssembly", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyAssemblyGroup", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyFaultImage", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyRemedy", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyCause", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyWeighting", DbType.String, 50, ColumnProperty.Null);
			Database.AddColumn("[SMS].[ServiceOrderHead]", "StatisticsKeyCauser", DbType.String, 50, ColumnProperty.Null);
		}
	}
}