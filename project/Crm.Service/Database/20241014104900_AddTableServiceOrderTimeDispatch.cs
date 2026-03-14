namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	using ForeignKeyConstraint = Crm.Library.Data.MigratorDotNet.Framework.ForeignKeyConstraint;

	[Migration(20241014104900)]
	public class AddTableServiceOrderTimeDispatch : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[SMS].[ServiceOrderTimeDispatch]"))
			{
				return;
			}

			Database.AddTable(
				"[SMS].[ServiceOrderTimeDispatch]",
				new Column("ServiceOrderTimeDispatchId",
					DbType.Guid,
					ColumnProperty.PrimaryKey,
					"NEWSEQUENTIALID()"),
				new Column("CreateUser",
					DbType.String,
					ColumnProperty.NotNull,
					"'Setup'"),
				new Column("ModifyUser",
					DbType.String,
					ColumnProperty.NotNull,
					"'Setup'"),
				new Column("CreateDate",
					DbType.DateTime,
					ColumnProperty.NotNull,
					"GETUTCDATE()"),
				new Column("ModifyDate",
					DbType.DateTime,
					ColumnProperty.NotNull,
					"GETUTCDATE()"),
				new Column("IsActive",
					DbType.Boolean,
					ColumnProperty.NotNull,
					true),
				new Column("ServiceOrderDispatchId",
					DbType.Guid,
					ColumnProperty.NotNull),
				new Column("ServiceOrderTimeId",
					DbType.Guid,
					ColumnProperty.NotNull)
			);

			Database.AddForeignKey("FK_ServiceOrderTimeDispatch_ServiceOrderDispatch",
				"SMS.ServiceOrderTimeDispatch",
				"ServiceOrderDispatchId",
				"SMS.ServiceOrderDispatch",
				"DispatchId",
				ForeignKeyConstraint.Cascade);
			Database.AddForeignKey("FK_ServiceOrderTimeDispatch_ServiceOrderTime",
				"SMS.ServiceOrderTimeDispatch",
				"ServiceOrderTimeId",
				"SMS.ServiceOrderTimes",
				"id",
				ForeignKeyConstraint.Cascade);
		}
	}
}
