namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20191002112300)]
	public class UpdateDefaultServiceCaseStatuses : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(
				@"IF NOT EXISTS (SELECT * FROM SMS.ServiceNotificationStatus WHERE VALUE NOT IN (0,2,4,5,6))
BEGIN
	UPDATE SMS.ServiceNotificationStatus SET SortOrder = 0, Groups = 'Preparation', SettableStatuses = '2,4,6' WHERE VALUE = 0
	UPDATE SMS.ServiceNotificationStatus SET SortOrder = 2, Groups = 'Preparation', SettableStatuses = '4,5,6' WHERE VALUE = 2
	UPDATE SMS.ServiceNotificationStatus SET SortOrder = 4, Groups = 'InProgress', SettableStatuses = '2,6' WHERE VALUE = 4
	UPDATE SMS.ServiceNotificationStatus SET SortOrder = 5, Groups = 'InProgress', SettableStatuses = '2,4,6' WHERE VALUE = 5
	UPDATE SMS.ServiceNotificationStatus SET SortOrder = 6, Groups = 'Closed', SettableStatuses = '5' WHERE VALUE = 6
END");
		}
	}
}