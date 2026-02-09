namespace Main.Database
{
	using System;
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180305174700)]
	public class AddAdditionalDomainExtensions : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("DefaultLanguageKey", DbType.String, 50, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("DefaultLocale", DbType.String, 20, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("Host", DbType.String, 100, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("MaterialLogo", DbType.Binary, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("MaterialTheme", DbType.String, 30, ColumnProperty.NotNull, "'bluegray'"));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("ReportFooter", DbType.String, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("ReportLogo", DbType.Binary, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("SiteLogo", DbType.Binary, Int32.MaxValue, ColumnProperty.Null));
			Database.AddColumnIfNotExisting("[dbo].[Domain]", new Column("Theme", DbType.String, 30, ColumnProperty.NotNull, "'standard'"));
		}
	}
}
