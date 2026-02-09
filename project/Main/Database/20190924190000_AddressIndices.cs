namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190924190000)]
	public class AddressIndices : Migration
	{
		public override void Up()
		{
			//old index using CompanyKeyOld
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_CompanyKey_IsCompanyStandardAddress')
			BEGIN
				DROP INDEX [IX_Address_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
			END");
			//index containing almost the whole table
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress')
			BEGIN
				DROP INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
			END");
			//index containing only companykey
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_CompanyKey')
			BEGIN
				DROP INDEX [IX_Address_CompanyKey] ON [CRM].[Address]
			END");

			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_IsActive_IsCompanyStandardAddress')
			BEGIN
				DROP INDEX [IX_Address_IsActive_IsCompanyStandardAddress] ON [CRM].[Address]
			END");
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_CompanyKey_IsActive_IsCompanyStandardAddress')
			BEGIN
				CREATE NONCLUSTERED INDEX [IX_Address_CompanyKey_IsActive_IsCompanyStandardAddress] ON [CRM].[Address]
				(
					[CompanyKey] ASC,
					[IsActive] ASC,
					[IsCompanyStandardAddress] ASC
					-- please include AuthDataId (from multitenant plugin) when changing this
				)
				INCLUDE ([ModifyDate],[AddressId])
			END");
		}
	}
}
