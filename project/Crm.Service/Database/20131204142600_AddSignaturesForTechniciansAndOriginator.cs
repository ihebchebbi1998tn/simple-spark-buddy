namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20131204142600)]
	public class AddSignaturesForTechniciansAndOriginator : Migration
	{
		public override void Up()
		{
			if (!Database.ColumnExists("SMS.ServiceOrderDispatch", "SignatureTechnician"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD SignatureTechnician nvarchar(max) NULL");
			}
			if (!Database.ColumnExists("SMS.ServiceOrderDispatch", "SignatureTechnicianName"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD SignatureTechnicianName nvarchar(256) NULL");
			}
			if (!Database.ColumnExists("SMS.ServiceOrderDispatch", "SignatureOriginator"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD SignatureOriginator nvarchar(max) NULL");
			}
			if (!Database.ColumnExists("SMS.ServiceOrderDispatch", "SignatureOriginatorName"))
			{
				Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderDispatch] ADD SignatureOriginatorName nvarchar(256) NULL");
			}

		}

		public override void Down()
		{
		}
	}
}