namespace Crm.Service.Database
{
	using System;

	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20130507163618)]
	public class AlterSmsServiceNotificationsDropFavoriteAndSortOrder : Migration
	{
		public override void Up()
		{
			if (Database.ColumnExists("[SMS].[ServiceNotifications]", "SortOrder"))
			{
				Database.ExecuteNonQuery("EXEC DropDefault 'SMS', 'ServiceNotifications', 'SortOrder'");
				Database.RemoveColumn("[SMS].[ServiceNotifications]", "SortOrder");
			}
			if (Database.ColumnExists("[SMS].[ServiceNotifications]", "Favorite"))
			{
				Database.ExecuteNonQuery("EXEC DropDefault 'SMS', 'ServiceNotifications', 'Favorite'");
				Database.RemoveColumn("[SMS].[ServiceNotifications]", "Favorite");
			}
		}
		public override void Down()
		{
			throw new NotImplementedException();
		}
	}
}