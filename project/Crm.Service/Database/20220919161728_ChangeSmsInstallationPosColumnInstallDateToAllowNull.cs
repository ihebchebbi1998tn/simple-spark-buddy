namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using System.Data;

	[Migration(20220919161728)]
	public class ChangeSmsInstallationPosColumnInstallDateToAllowNull : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[SMS].[InstallationPos]", "InstallDate"))
			{
				Database.ChangeColumn("[SMS].[InstallationPos]", new Column("InstallDate", DbType.DateTime, ColumnProperty.Null));
			}

		}
	}
}
