namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131031183234)]
	public class AddTableLuCauseOfFailure : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("LU.CauseOfFailure"))
			{
				Database.AddTable("LU.CauseOfFailure",
					new Column("CauseOfFailureId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Value", DbType.String, 20, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
					new Column("TenantKey", DbType.Int32, ColumnProperty.Null));
			}
		}
	}
}