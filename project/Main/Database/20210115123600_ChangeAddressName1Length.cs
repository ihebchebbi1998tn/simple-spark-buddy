namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210115123600)]
	public class ChangeAddressName1Length : Migration
	{
		public override void Up()
		{
			var contactNameLength = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'Contact' AND
								COLUMN_NAME = 'Name'");

			var addressName1Length = (int)Database.ExecuteScalar(
				@"SELECT CHARACTER_MAXIMUM_LENGTH 
								FROM INFORMATION_SCHEMA.COLUMNS
								WHERE TABLE_SCHEMA = 'CRM' AND
								TABLE_NAME = 'Address' AND
								COLUMN_NAME = 'Name1'");

			if(contactNameLength == 450 && addressName1Length < 450)
			{
				Database.ExecuteNonQuery(@"
				IF EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress')
					BEGIN
						DROP INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
					END

				ALTER TABLE [CRM].[Address] ALTER COLUMN [Name1] NVARCHAR(450)

				IF NOT EXISTS (SELECT * FROM sys.indexes WHERE NAME = 'IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress')
					BEGIN
						CREATE NONCLUSTERED INDEX [IX_Address_AddressId_CompanyKey_IsCompanyStandardAddress] ON [CRM].[Address]
						(
							[AddressId] ASC,
							[CompanyKey] ASC,
							[IsCompanyStandardAddress] ASC
						)
						INCLUDE ([Name1],
							[Name2],
							[Name3],
							[City],
							[CountryKey],
							[ZipCode],
							[Street],
							[Latitude],
							[Longitude]) ON [PRIMARY]
					END
			");
			}
		}
	}
}
