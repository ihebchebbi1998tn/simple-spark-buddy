namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190703102459)]
	public class AlterCostCenterCreateAndModifyDateToDateTime : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"	ALTER TABLE[LU].[CostCenter] DROP CONSTRAINT[DF_LUCostCenter_CreateDate]
						ALTER TABLE [LU].[CostCenter] DROP CONSTRAINT [DF_LUCostCenter_ModifyDate]");

			Database.ExecuteNonQuery(
				@"ALTER TABLE [LU].[CostCenter] ALTER COLUMN [CreateDate] DATETIME NOT NULL
						ALTER TABLE [LU].[CostCenter] ALTER COLUMN [ModifyDate] DATETIME NOT NULL");

			Database.ExecuteNonQuery(
				@"	ALTER TABLE [LU].[CostCenter] ADD  CONSTRAINT [DF_LUCostCenter_CreateDate]  DEFAULT (getutcdate()) FOR [CreateDate]
						ALTER TABLE [LU].[CostCenter] ADD  CONSTRAINT [DF_LUCostCenter_ModifyDate]  DEFAULT (getutcdate()) FOR [ModifyDate]");
		}
	}
}