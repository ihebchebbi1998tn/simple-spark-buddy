namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404162901)]
	public class AddIndexForCrmAddressCompanyKey : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_CompanyKey'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Address_CompanyKey] ON [CRM].[Address]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Address_CompanyKey] ON [CRM].[Address] ([CompanyKey] ASC)");
		}
	}
}