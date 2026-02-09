namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20200326152700)]
	public class UserRecentPagesAddIsMaterialColumn : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.UserRecentPages", new Column("IsMaterial", DbType.Boolean, ColumnProperty.NotNull, false));
			Database.AddColumnIfNotExisting("CRM.UserRecentPages", new Column("Category", DbType.String, ColumnProperty.Null));
		}
	}
}
