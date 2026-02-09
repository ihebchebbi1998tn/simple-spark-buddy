namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130506163417)]
	public class AlterSmsInstallationHeadColumnInstallationTypeNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [InstallationType] NVARCHAR(20) NULL");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}