namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164504)]
	public class AddPayerAddressIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_PayerAddress'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[PayerAddressId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Address] a ON soh.[PayerAddressId] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_PayerAddress", "[SMS].[ServiceOrderHead]", "PayerAddressId", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}