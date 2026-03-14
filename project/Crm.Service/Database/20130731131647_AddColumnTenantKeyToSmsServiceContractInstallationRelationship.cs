namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130731131647)]
	public class AddColumnTenantKeyToSmsServiceContractInstallationRelationship : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[SMS].[ServiceContractInstallationRelationship]", new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}