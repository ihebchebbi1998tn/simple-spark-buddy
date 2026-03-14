namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20180220110000)]
	public class StationToGuid : Migration
	{
		public override void Up()
		{
			if (Database.GetColumnDataType("Crm", "Station", "StationId") == "int")
			{
				Database.ExecuteNonQuery(@"ALTER TABLE CRM.[User] DROP CONSTRAINT FK_User_Station");
				Database.ExecuteNonQuery(@"ALTER TABLE CRM.Station DROP CONSTRAINT PK_Station");
				Database.ExecuteNonQuery(@"ALTER TABLE CRM.Station ADD StationIdOld INT NULL");
				Database.ExecuteNonQuery(@"UPDATE CRM.Station SET StationIdOld = StationId");
				Database.ExecuteNonQuery(@"ALTER TABLE CRM.Station DROP COLUMN StationId");
				Database.ExecuteNonQuery(@"
					ALTER TABLE CRM.Station
					ADD StationId UNIQUEIDENTIFIER
					CONSTRAINT DF_Station_StationId DEFAULT(NEWSEQUENTIALID())
					CONSTRAINT PK_Station PRIMARY KEY");
				Database.ExecuteNonQuery(@"
					UPDATE CRM.Station
					SET ModifyDate = GETUTCDATE()
						,ModifyUser = 'Migration_20180220110000'");

				Database.ExecuteNonQuery(@"sp_rename 'CRM.User.StationKey', 'StationKeyOld', 'COLUMN'");
				Database.ExecuteNonQuery(@"
					ALTER TABLE CRM.[User]
					ADD StationKey UNIQUEIDENTIFIER NULL
					CONSTRAINT FK_User_Station FOREIGN KEY (StationKey) REFERENCES CRM.Station(StationId)");
				Database.ExecuteNonQuery(@"
					UPDATE CRM.[User]
					SET StationKey = StationId
						,ModifyDate = GETUTCDATE()
						,ModifyUser = 'Migration_20180220110000'
					FROM CRM.[User]
					JOIN CRM.Station ON StationIdOld = StationKeyOld");
				Database.RemoveColumn("CRM.[User]", "StationKeyOld");
			}
			Database.AddEntityBaseDefaultContraints("Crm", "Station");
		}
	}
}
