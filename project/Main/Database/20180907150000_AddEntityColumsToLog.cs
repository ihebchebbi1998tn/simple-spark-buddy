namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180907150000)]
	public class AddEntityColumsToLog : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Log", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("CRM.Log", new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'System'"));
			Database.AddColumnIfNotExisting("CRM.Log", new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'System'"));
			Database.AddColumnIfNotExisting("CRM.Log", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}
