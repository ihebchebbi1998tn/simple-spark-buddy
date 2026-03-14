namespace Crm.Service.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210625132500)]
	public class AddStatisticsKeyLookups : Migration
	{
		public override void Up()
		{
			if(!Database.TableExists("LU.StatisticsKeyProductType"))
			{
				Database.AddTable(
					"LU.StatisticsKeyProductType",
					new Column("StatisticsKeyProductTypeId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyMainAssembly"))
			{
				Database.AddTable(
					"LU.StatisticsKeyMainAssembly",
					new Column("StatisticsKeyMainAssemblyId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeySubAssembly"))
			{
				Database.AddTable(
					"LU.StatisticsKeySubAssembly",
					new Column("StatisticsKeySubAssemblyId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("MainAssemblyKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyAssemblyGroup"))
			{
				Database.AddTable(
					"LU.StatisticsKeyAssemblyGroup",
					new Column("StatisticsKeyAssemblyGroupId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("MainAssemblyKey", DbType.String, 50, ColumnProperty.Null),
					new Column("SubAssemblyKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyFaultImage"))
			{
				Database.AddTable(
					"LU.StatisticsKeyFaultImage",
					new Column("StatisticsKeyFaultImageId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("AssemblyGroupKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyRemedy"))
			{
				Database.AddTable(
					"LU.StatisticsKeyRemedy",
					new Column("StatisticsKeyRemedyId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyCause"))
			{
				Database.AddTable(
					"LU.StatisticsKeyCause",
					new Column("StatisticsKeyCauseId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyWeighting"))
			{
				Database.AddTable(
					"LU.StatisticsKeyWeighting",
					new Column("StatisticsKeyWeightingId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

			if (!Database.TableExists("LU.StatisticsKeyCauser"))
			{
				Database.AddTable(
					"LU.StatisticsKeyCauser",
					new Column("StatisticsKeyCauserId", DbType.Int32, ColumnProperty.PrimaryKeyWithIdentity),
					new Column("ProductTypeKey", DbType.String, 50, ColumnProperty.Null),
					new Column("Code", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Value", DbType.String, 50, ColumnProperty.NotNull),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.String, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.NotNull, 0),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull, "'Setup'"),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, true)
				);
			}

		}
	}
}