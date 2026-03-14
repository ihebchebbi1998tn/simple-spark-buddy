namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130806114125)]
	public class AddColumnTechnicianInformationToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[InstallationHead]", new Column("TechnicianInformation", DbType.String, 4000, ColumnProperty.Null));
		}
	}
}