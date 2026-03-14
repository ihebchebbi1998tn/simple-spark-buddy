namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164506)]
	public class AddInitiatorIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_Initiator'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[InitiatorId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Contact] c ON soh.[InitiatorId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_Initiator", "[SMS].[ServiceOrderHead]", "InitiatorId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}