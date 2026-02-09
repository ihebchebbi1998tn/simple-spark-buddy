namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20170717152300)]
	public class AddEntityColumnsToCrmUsergroup : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "getutcdate()"));
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "getutcdate()"));
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[Usergroup]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}