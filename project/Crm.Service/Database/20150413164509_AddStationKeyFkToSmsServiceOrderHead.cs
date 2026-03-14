namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20150413164509)]
	public class AddStationKeyFkToSmsServiceOrderHead : Migration
	{
		public override void Up()
		{
			if ((int)Database.ExecuteScalar("SELECT COUNT(*) FROM sys.foreign_keys WHERE name = 'FK_ServiceOrderHead_Station'") == 0)
			{
				Database.ExecuteNonQuery("UPDATE soh SET soh.[StationKey] = NULL FROM [SMS].[ServiceOrderHead] soh LEFT OUTER JOIN [CRM].[Station] s ON soh.[StationKey] = s.[StationId] WHERE s.[StationId] IS NULL");
				Database.AddForeignKey("FK_ServiceOrderHead_Station", "[SMS].[ServiceOrderHead]", "StationKey", "[CRM].[Station]", "StationId", ForeignKeyConstraint.NoAction);
			}
		}
	}
}