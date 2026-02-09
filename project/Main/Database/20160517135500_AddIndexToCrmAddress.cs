namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160517135500)]
	public class AddIndexToCrmAddress : Migration
	{
		public override void Up()
		{
			const string query = @"IF NOT EXISTS (SELECT name FROM sysindexes WHERE name = 'IX_Address_IsActive_IsCompanyStandardAddress')" +
			                     "CREATE NONCLUSTERED INDEX [IX_Address_IsActive_IsCompanyStandardAddress] ON [CRM].[Address] ([IsActive], [IsCompanyStandardAddress]) INCLUDE ([AddressId], [CompanyKey])";

			Database.ExecuteNonQuery(query);
		}
	}
}
