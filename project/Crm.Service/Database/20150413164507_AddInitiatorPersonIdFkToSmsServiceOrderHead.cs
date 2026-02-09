namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164507)]
	public class AddInitiatorPersonIdFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_InitiatorPerson'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[InitiatorPersonId] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Contact] c ON soh.[InitiatorPersonId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_InitiatorPerson", "[SMS].[ServiceOrderHead]", "InitiatorPersonId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}