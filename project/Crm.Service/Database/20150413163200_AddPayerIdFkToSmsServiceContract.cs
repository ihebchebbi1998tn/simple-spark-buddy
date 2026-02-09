namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	
	[Migration(20150413163200)]
	public class AddPayerIdFkToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContract_Payer'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sc SET sc.[PayerId] = NULL FROM [SMS].[ServiceContract] sc LEFT OUTER JOIN [CRM].[Contact] c on sc.[PayerId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContract_Payer", "[SMS].[ServiceContract]", "PayerId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}