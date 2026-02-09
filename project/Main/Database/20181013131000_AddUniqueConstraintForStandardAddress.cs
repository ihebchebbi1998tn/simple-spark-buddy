namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20181013131000)]
	public class AddUniqueConstraintForStandardAddress : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) from sys.indexes WHERE name = N'IX_Address_OneStandardAddress'") > 0)
			{
				Database.ExecuteNonQuery("DROP INDEX IX_Address_OneStandardAddress ON [CRM].[Address]");
			}
			Database.ExecuteNonQuery("CREATE UNIQUE INDEX IX_Address_OneStandardAddress ON [CRM].[Address](CompanyKey) WHERE CompanyKey is not null AND IsCompanyStandardAddress = 1 AND IsActive = 1");
		}
	}
}
