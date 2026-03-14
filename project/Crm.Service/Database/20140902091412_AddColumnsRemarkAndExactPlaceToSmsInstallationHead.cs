namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140902091412)]
	public class AddColumnsRemarkAndExactPlaceToSmsInstallationHead : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("[SMS].[InstallationHead]", "Remark"))
			{
				Database.AddColumn("[SMS].[InstallationHead]", new Column("Remark", DbType.String, 60, ColumnProperty.Null));
			}
			if (!Database.ColumnExists("[SMS].[InstallationHead]", "ExactPlace"))
			{
				Database.AddColumn("[SMS].[InstallationHead]", new Column("ExactPlace", DbType.String, 36, ColumnProperty.Null));
			}
		}
		public override void Down()
		{
			if (!Database.ColumnExists("[SMS].[InstallationHead]", "Remark"))
			{
				Database.RemoveColumn("[SMS].[InstallationHead]", "Remark");
			}
			if (!Database.ColumnExists("[SMS].[InstallationHead]", "ExactPlace"))
			{
				Database.RemoveColumn("[SMS].[InstallationHead]", "ExactPlace");
			}
		}
	}
}