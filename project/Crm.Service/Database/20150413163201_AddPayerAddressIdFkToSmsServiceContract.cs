namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413163201)]
	public class AddPayerAddressIdFkToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContract_PayerAddress'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sc SET sc.[PayerAddressId] = NULL FROM [SMS].[ServiceContract] sc LEFT OUTER JOIN [CRM].[Address] a on sc.[PayerAddressId] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_ServiceContract_PayerAddress", "[SMS].[ServiceContract]", "PayerAddressId", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}