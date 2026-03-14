namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130802130141)]
	public class RenameCostUnitToCostCenter : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[CostCenter]"))
			{
				Database.AddTable("[LU].[CostCenter]",
								new Column("CostCenterId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
								new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
								new Column("Name", DbType.String, ColumnProperty.NotNull),
								new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
								new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull),
								new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull),
								new Column("CreateDate", DbType.String, 256, ColumnProperty.NotNull),
								new Column("CreateUser", DbType.String, 256, ColumnProperty.NotNull),
								new Column("ModifyDate", DbType.String, 256, ColumnProperty.NotNull),
								new Column("ModifyUser", DbType.String, 256, ColumnProperty.NotNull),
								new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true));
			}
			if (Database.TableExists("[SMS].[CostUnit]"))
			{
				Database.RemoveTable("[SMS].[CostUnit]");
			}
		}

		public override void Down()
		{
		}
	}
}