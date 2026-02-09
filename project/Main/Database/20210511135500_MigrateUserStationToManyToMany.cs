namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20210511135500)]
	public class MigrateUserStationToManyToMany : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				INSERT INTO [CRM].[UserStation] (UserKey, StationKey)
				SELECT UserID as UserKey, StationKey
				FROM [CRM].[User]
				WHERE StationKey IS NOT NULL");
			Database.RemoveColumn("[CRM].[User]", "StationKey");
		}
	}
}
