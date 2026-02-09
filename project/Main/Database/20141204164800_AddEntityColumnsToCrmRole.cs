namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20141204164800)]
	public class AddEntityColumnsToCrmRole : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Role]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[Role]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[Role]", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Role]", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Role]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}