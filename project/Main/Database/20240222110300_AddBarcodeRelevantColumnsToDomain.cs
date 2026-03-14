namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240222110300)]
	public class AddBarcodeRelevantColumnsToDomain : Migration
	{
		public override void Up()
		{
			const string TableName = "dbo.Domain";

			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodeShowOnServiceReport",
					DbType.Boolean,
					ColumnProperty.NotNull,
					false));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodeShowOnChecklistPdf",
					DbType.Boolean,
					ColumnProperty.NotNull,
					false));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodePrintOnly",
					DbType.Boolean,
					ColumnProperty.NotNull,
					false));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodePrefix",
					DbType.String,
					256,
					ColumnProperty.Null));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodeRoot",
					DbType.String,
					256,
					ColumnProperty.Null));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodeSuffix",
					DbType.String,
					256,
					ColumnProperty.Null));
			Database.AddColumnIfNotExisting(TableName,
				new Column("ReportBarcodeFormat",
					DbType.String,
					256,
					ColumnProperty.NotNull,
					"'CODE39'"));
		}
	}
}
