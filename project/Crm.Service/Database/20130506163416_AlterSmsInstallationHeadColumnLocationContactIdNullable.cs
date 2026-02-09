namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130506163416)]
	public class AlterSmsInstallationHeadColumnLocationContactIdNullable : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[InstallationHead] ALTER COLUMN [LocationContactId] INT NULL");
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}