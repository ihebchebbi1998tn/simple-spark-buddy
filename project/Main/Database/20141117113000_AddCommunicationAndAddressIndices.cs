namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20141117113000)]
	public class AddCommunicationAndAddressIndices : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Communication]') AND name = N'IX_Communication_GroupKey_ContactKey') " +
															 "BEGIN " +
															 "CREATE NONCLUSTERED INDEX IX_Communication_GroupKey_ContactKey " +
																"ON [CRM].[Communication] ([GroupKey],[ContactKey]) " +
															 "END");


			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Address]') AND name = N'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress') 
																	BEGIN
																	CREATE NONCLUSTERED INDEX IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress ON [CRM].[Address] 
																	(
																		[AddressId] ASC,
																		[CompanyKey] ASC,
																		[IsCompanyStandardAddress] ASC
																	)
																	INCLUDE ( [Name1],
																	[Name2],
																	[Name3],
																	[City],
																	[CountryKey],
																	[ZipCode],
																	[Street],
																	[Latitude],
																	[Longitude]) WITH (SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF) ON [PRIMARY]
																	END");
		}
	}
}