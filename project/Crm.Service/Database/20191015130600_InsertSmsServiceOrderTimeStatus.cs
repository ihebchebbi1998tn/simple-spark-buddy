namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191015130600)]
	public class InsertSmsServiceOrderTimeStatus : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("TRUNCATE TABLE [SMS].[ServiceOrderTimeStatus]");
			Database.ExecuteNonQuery(
				@"INSERT INTO [SMS].[ServiceOrderTimeStatus]
				([Name], [Language], [Value], [SortOrder], [CreateUser], [ModifyUser], [Color])
				VALUES
				('Erstellt', 'de', 'Created', 0, 'Migration_20191015130600', 'Migration_20191015130600', '#9e9e9e'),
				('Created', 'en', 'Created', 0, 'Migration_20191015130600', 'Migration_20191015130600', '#9e9e9e'),
				('Établi', 'fr', 'Created', 0, 'Migration_20191015130600', 'Migration_20191015130600', '#9e9e9e'),
				('Alkotó', 'hu', 'Created', 0, 'Migration_20191015130600', 'Migration_20191015130600', '#9e9e9e'),
				('Gestartet', 'de', 'Started', 1, 'Migration_20191015130600', 'Migration_20191015130600', '#2196f3'),
				('Started', 'en', 'Started', 1, 'Migration_20191015130600', 'Migration_20191015130600', '#2196f3'),
				('Commencé', 'fr', 'Started', 1, 'Migration_20191015130600', 'Migration_20191015130600', '#2196f3'),
				('Lépések', 'hu', 'Started', 1, 'Migration_20191015130600', 'Migration_20191015130600', '#2196f3'),
				('Unterbrochen', 'de', 'Interrupted', 2, 'Migration_20191015130600', 'Migration_20191015130600', '#ff9800'),
				('Interrupted', 'en', 'Interrupted', 2, 'Migration_20191015130600', 'Migration_20191015130600', '#ff9800'),
				('Interrompu', 'fr', 'Interrupted', 2, 'Migration_20191015130600', 'Migration_20191015130600', '#ff9800'),
				('Megszakított', 'hu', 'Interrupted', 2, 'Migration_20191015130600', 'Migration_20191015130600', '#ff9800'),
				('Abgeschlossen', 'de', 'Finished', 3, 'Migration_20191015130600', 'Migration_20191015130600', '#4caf50'),
				('Finished', 'en', 'Finished', 3, 'Migration_20191015130600', 'Migration_20191015130600', '#4caf50'),
				('Terminé', 'fr', 'Finished', 3, 'Migration_20191015130600', 'Migration_20191015130600', '#4caf50'),
				('Befejezett', 'hu', 'Finished', 3, 'Migration_20191015130600', 'Migration_20191015130600', '#4caf50')
			");
		}
	}
}