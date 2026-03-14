namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20231020150000)]
	public class AddInstallationTypeKeyToLuStatisticsKeyFaultImage : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[LU].[StatisticsKeyFaultImage]", "InstallationTypeKey"))
			{
				Database.AddColumn("[LU].[StatisticsKeyFaultImage]", "InstallationTypeKey", DbType.String, 500, ColumnProperty.Null);
			}
		}
	}
}
