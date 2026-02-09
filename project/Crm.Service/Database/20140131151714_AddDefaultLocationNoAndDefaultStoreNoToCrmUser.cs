namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20140131151714)]
	public class AddDefaultLocationNoAndDefaultStoreNoToCrmUser : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.[User]", new Column("DefaultStoreNo", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("CRM.[User]", new Column("DefaultLocationNo", DbType.String, 50, ColumnProperty.Null));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}