namespace Crm.Service.Team.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240926131200)]

	public class AddTeamPluginToLuLicenseModule : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, CreateUser, ModifyUser) VALUES ('FLD00060', 'Serviceteams', 'de', 'FLD00010', 'Migration_20240926131200', 'Migration_20240926131200')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, CreateUser, ModifyUser) VALUES ('FLD00060', 'Service teams', 'en', 'FLD00010', 'Migration_20240926131200', 'Migration_20240926131200')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, CreateUser, ModifyUser) VALUES ('FLD00060', 'Equipos de servicio', 'es', 'FLD00010', 'Migration_20240926131200', 'Migration_20240926131200')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, CreateUser, ModifyUser) VALUES ('FLD00060', 'Équipes de service', 'fr', 'FLD00010', 'Migration_20240926131200', 'Migration_20240926131200')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, CreateUser, ModifyUser) VALUES ('FLD00060', 'Szervizcsapatok', 'hu', 'FLD00010', 'Migration_20240926131200', 'Migration_20240926131200')");
		}
	}
}
