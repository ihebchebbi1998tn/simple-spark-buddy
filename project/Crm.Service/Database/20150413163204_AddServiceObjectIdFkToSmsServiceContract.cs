namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413163204)]
	public class AddServiceObjectIdFkToSmsServiceContract : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceContract_ServiceObject'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sc SET sc.[ServiceObjectId] = NULL FROM [SMS].[ServiceContract] sc LEFT OUTER JOIN [CRM].[Contact] c on sc.[ServiceObjectId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceContract_ServiceObject", "[SMS].[ServiceContract]", "ServiceObjectId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}