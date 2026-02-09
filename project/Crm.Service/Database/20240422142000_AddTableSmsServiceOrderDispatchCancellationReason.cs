namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240422142000)]
	public class AddTableSmsServiceOrderDispatchCancellationReason : Migration
	{
		public override void Up()
		{
			const string TableName = "Sms.ServiceOrderDispatchCancellationReason";
			if (!Database.TableExists(TableName))
			{
				Database.AddTable(TableName,
					new Column("ServiceOrderDispatchCancellationReasonId",
						DbType.Guid,
						ColumnProperty.PrimaryKey,
						"NEWSEQUENTIALID()"),
					new Column("Value",
						DbType.String,
						50,
						ColumnProperty.NotNull),
					new Column("Name",
						DbType.String,
						ColumnProperty.NotNull),
					new Column("Language",
						DbType.String,
						2,
						ColumnProperty.NotNull),
					new Column("Favorite",
						DbType.Boolean,
						ColumnProperty.NotNull),
					new Column("SortOrder",
						DbType.Int32,
						ColumnProperty.NotNull),
					new Column("CreateDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"),
					new Column("CreateUser",
						DbType.String,
						256,
						ColumnProperty.NotNull),
					new Column("ModifyDate",
						DbType.DateTime,
						ColumnProperty.NotNull,
						"GETUTCDATE()"
					),
					new Column("ModifyUser",
						DbType.String,
						256,
						ColumnProperty.NotNull),
					new Column("IsActive",
						DbType.Boolean,
						ColumnProperty.NotNull,
						true));
			}
		}
	}
}
