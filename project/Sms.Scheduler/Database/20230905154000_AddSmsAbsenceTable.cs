namespace Sms.Scheduler.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;

	using Sms.Scheduler.Model;

	[Migration(20230905154000)]
	public class AddSmsAbsenceTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[SMS].[Absence]"))
			{
				var helper = new UnicoreMigrationHelper(Database);
				Database.AddTable("[SMS].[Absence]",
					new Column("[AbsenceId]", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("[TimeEntryTypeKey]", DbType.String, ColumnProperty.NotNull),
					new Column("[From]", DbType.DateTime, ColumnProperty.NotNull),
					new Column("[To]", DbType.DateTime, ColumnProperty.NotNull),
					new Column("[ResponsibleUser]", DbType.String, ColumnProperty.NotNull),
					new Column("[Description]", DbType.String, 4000, ColumnProperty.Null),
					new Column("[CreateDate]", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("[ModifyDate]", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("[CreateUser]", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("[ModifyUser]", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("[IsActive]", DbType.Boolean, ColumnProperty.NotNull, true)
				);
				helper.AddOrUpdateEntityAuthDataColumn<Absence>("SMS", "Absence");
			}
		}
	}
}

