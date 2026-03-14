namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164503)]
	public class AddPayerIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_Payer'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[PayerId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Contact] c ON soh.[PayerId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_Payer", "[SMS].[ServiceOrderHead]", "PayerId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}