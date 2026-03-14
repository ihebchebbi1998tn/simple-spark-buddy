namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20130905153952)]
	public class AddEntityColumnsToLuCostCenter : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("LU.CostCenter", new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("LU.CostCenter", new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"));
			Database.AddColumnIfNotExisting("LU.CostCenter", new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.CostCenter", new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull, "'Setup'"));
			Database.AddColumnIfNotExisting("LU.CostCenter", new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
		}
		public override void Down()
		{
			Database.RemoveColumnIfExisting("LU.CostCenter", "CreateDate");
			Database.RemoveColumnIfExisting("LU.CostCenter", "ModifyDate");
			Database.RemoveColumnIfExisting("LU.CostCenter", "CreateUser");
			Database.RemoveColumnIfExisting("LU.CostCenter", "ModifyUser");
			Database.RemoveColumnIfExisting("LU.CostCenter", "IsActive");
		}
	}
}