namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413175000)]
	public class AddCompanyKeyFkToCrmAddress : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Address_Contact'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE a SET a.[CompanyKey] = NULL FROM [CRM].[Address] a LEFT OUTER JOIN [CRM].[Contact] c ON a.[CompanyKey] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_Address_Contact", "[CRM].[Address]", "CompanyKey", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.Cascade);
			}
		}
	}
}