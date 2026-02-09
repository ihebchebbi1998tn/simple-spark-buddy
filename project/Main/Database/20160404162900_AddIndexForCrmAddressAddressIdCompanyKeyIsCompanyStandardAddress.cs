namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160404162900)]
	public class AddIndexForCrmAddressAddressIdCompanyKeyIsCompanyStandardAddress : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress'") == 1)
			{
				Database.ExecuteNonQuery("DROP INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]");
			}
			Database.ExecuteNonQuery("CREATE NONCLUSTERED INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address] ([AddressId] ASC, [CompanyKey] ASC, [IsCompanyStandardAddress] ASC) INCLUDE ([City], [CountryKey], [Latitude], [Longitude], [Name1], [Name2], [Name3], [Street], [ZipCode])");
		}
	}
}