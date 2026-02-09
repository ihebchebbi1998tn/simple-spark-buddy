namespace Crm.Service.Database
{

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231020160000)]
	public class AddMissingColumnsToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[InstallationHead]"))
			{
				Database.AddColumn("[SMS].[InstallationHead]", "StatisticsKeyProductTypeKey", System.Data.DbType.String, 500, ColumnProperty.Null);
			}
		}
	}
}
