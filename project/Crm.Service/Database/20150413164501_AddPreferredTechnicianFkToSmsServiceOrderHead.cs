namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164501)]
	public class AddPreferredTechnicianFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_PreferredTechnician'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[PreferredTechnician] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[User] u ON soh.[PreferredTechnician] = u.[Username] WHERE u.[Username] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_PreferredTechnician", "[SMS].[ServiceOrderHead]", "PreferredTechnician", "[CRM].[User]", "Username", ForeignKeyConstraint.NoAction);
			}
		}
	}
}