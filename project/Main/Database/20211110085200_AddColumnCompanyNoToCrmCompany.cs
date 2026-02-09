namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20211110085200)]
	public class AddColumnCompanyNoToCrmCompany : Migration
	{
		public override void Up()
		{
			if (Database.TableExists("[CRM].[Company]"))
			{
				Database.AddColumnIfNotExisting("[CRM].[Company]", new Column("CompanyNo", DbType.String, 20, ColumnProperty.Null));
			}

		}
	}
}
