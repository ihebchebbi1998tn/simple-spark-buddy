namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170102182500)]
	public class AddEntityColumnsToCrmContactTags : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.ContactTags", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("CRM.ContactTags", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("CRM.ContactTags", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("CRM.ContactTags", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("CRM.ContactTags", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}