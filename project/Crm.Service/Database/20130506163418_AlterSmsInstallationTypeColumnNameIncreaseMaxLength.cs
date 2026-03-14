namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130506163418)]
	public class AlterSmsInstallationTypeColumnNameIncreaseMaxLength : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationType] ALTER COLUMN [Name] NVARCHAR(250) NOT NULL");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}