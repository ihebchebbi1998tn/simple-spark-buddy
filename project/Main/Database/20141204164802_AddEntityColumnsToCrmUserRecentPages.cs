namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20141204164802)]
	public class AddEntityColumnsToCrmUserRecentPages : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[UserRecentPages]", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[UserRecentPages]", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("[CRM].[UserRecentPages]", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[UserRecentPages]", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("[CRM].[UserRecentPages]", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}