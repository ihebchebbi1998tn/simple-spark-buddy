namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190312174500)]
	public class AddSalesRepresentativeToCompany : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("[CRM].[Company]", new Column("SalesRepresentative", DbType.String, 256, ColumnProperty.Null));
		}
	}
}
