namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20161230145700)]
	public class AddEntityColumnsToLuSourceType : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.SourceType", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("LU.SourceType", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("LU.SourceType", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.SourceType", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.SourceType", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
	}
}