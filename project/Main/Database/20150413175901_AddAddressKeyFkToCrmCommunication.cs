namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413175901)]
	public class AddAddressKeyFkToCrmCommunication : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_Communication_Address'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE c SET c.[AddressKey] = NULL FROM [CRM].[Communication] c LEFT OUTER JOIN [CRM].[Address] a ON c.[AddressKey] = a.[AddressId] WHERE a.[AddressId] IS NULL");
				Database.AddForeignKey("FK_Communication_Address", "[CRM].[Communication]", "AddressKey", "[CRM].[Address]", "AddressId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}