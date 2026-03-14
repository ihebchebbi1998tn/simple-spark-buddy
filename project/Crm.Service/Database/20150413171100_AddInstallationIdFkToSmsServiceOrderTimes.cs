namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413171100)]
	public class AddInstallationIdFkToSmsServiceOrderTimes : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderTimes_Installation'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE sot SET sot.[InstallationId] = NULL FROM [SMS].[ServiceOrderTimes] sot LEFT OUTER JOIN [CRM].[Contact] c ON sot.[InstallationId] = c.[ContactId] WHERE c.[ContactId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderTimes_Installation", "[SMS].[ServiceOrderTimes]", "InstallationId", "[CRM].[Contact]", "ContactId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}