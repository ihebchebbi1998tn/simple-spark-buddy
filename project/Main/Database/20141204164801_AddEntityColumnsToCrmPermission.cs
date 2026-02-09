namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20141204164801)]
	public class AddEntityColumnsToCrmPermission : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Permission]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[Permission]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[Permission]", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Permission]", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Permission]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}