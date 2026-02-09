namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20170620110000)]
	public class ChangeTypeTimeEntryIndexPermission : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				UPDATE [CRM].[Permission]
				SET Type = 0
					, ModifyUser = 'Migration_20170620110000'
					, ModifyDate = GETUTCDATE()
				WHERE PGroup = 'TimeEntry' AND Name = 'Index'");
		}
	}
}