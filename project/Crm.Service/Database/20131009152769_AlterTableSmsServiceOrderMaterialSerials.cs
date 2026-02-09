namespace Crm.Service.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20131009152769)]
	public class AlterTableSmsServiceOrderMaterialSerials : Migration
	{
		public override void Up()
		{
			Database.RemoveColumnIfExisting("SMS.ServiceOrderMaterialSerials", "Attribute01");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderMaterialSerials", "Attribute02");
			Database.RemoveColumnIfExisting("SMS.ServiceOrderMaterialSerials", "Attribute03");
			if (Database.ColumnExists("SMS.ServiceOrderMaterialSerials", "SortOrder"))
			{
				Database.RemoveDefaultConstraint("SMS.ServiceOrderMaterialSerials", "SortOrder");
				Database.RemoveColumn("SMS.ServiceOrderMaterialSerials", "SortOrder");
			}
			if (Database.ColumnExists("SMS.ServiceOrderMaterialSerials", "Favorite"))
			{
				Database.RemoveDefaultConstraint("SMS.ServiceOrderMaterialSerials", "Favorite");
				Database.RemoveColumn("SMS.ServiceOrderMaterialSerials", "Favorite");
			}
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterialSerials", new Column("PreviousSerialNo", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("SMS.ServiceOrderMaterialSerials", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}