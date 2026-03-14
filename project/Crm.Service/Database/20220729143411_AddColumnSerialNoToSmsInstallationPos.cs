namespace Crm.Database.Modify
{
	using System.Text;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220729143411)]
	public class AddColumnSerialNoToSmsInstallationPos : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[InstallationPos]") && !Database.ColumnExists("[SMS].[InstallationPos]", "SerialNo"))
			{
				StringBuilder query = new StringBuilder();
				query.Append("ALTER TABLE [SMS].[InstallationPos] ADD SerialNo nvarchar(250) ");
				query.Append("ALTER TABLE [SMS].[InstallationPos] ADD IsSerial bit NOT NULL CONSTRAINT DF_InstallationPos_IsSerial DEFAULT (0)");
				Database.ExecuteNonQuery(query.ToString());
			}
		}
	}
}
