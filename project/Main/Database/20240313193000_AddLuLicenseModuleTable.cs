namespace Main.Database
{
	using System.Data;
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20240313193000)]
	public class AddLuLicenseModuleTable : Migration
	{
		public override void Up()
		{
			if (!Database.TableExists("[LU].[LicenseModule]"))
			{
				Database.AddTable(
					"[LU].[LicenseModule]",
					new Column($"LicenseModuleId", DbType.Guid, ColumnProperty.PrimaryKey, "NEWSEQUENTIALID()"),
					new Column("Name", DbType.String, ColumnProperty.NotNull),
					new Column("Language", DbType.StringFixedLength, 2, ColumnProperty.NotNull),
					new Column("Favorite", DbType.Boolean, ColumnProperty.NotNull, false),
					new Column("SortOrder", DbType.Int32, ColumnProperty.Null),
					new Column("Value", DbType.String, ColumnProperty.NotNull),
					new Column("ParentModuleId", DbType.String, ColumnProperty.Null),
					new Column("CreateDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("ModifyDate", DbType.DateTime, ColumnProperty.NotNull, "GETUTCDATE()"),
					new Column("CreateUser", DbType.String, ColumnProperty.NotNull),
					new Column("ModifyUser", DbType.String, ColumnProperty.NotNull),
					new Column("IsActive", DbType.Boolean, ColumnProperty.NotNull, 1)
				);
			}
			
			AddLicenseModule("FLD00010", "Crm.Service plugin", "Crm.Service plugin", "Crm.Service plugin", "Crm.Service plugin");
			AddLicenseModule("FLD00020", "Main.Multitenant plugin", "Main.Multitenant plugin", "Main.Multitenant plugin", "Main.Multitenant plugin");
			AddLicenseModule("FLD00030", "Sms.Einsatzplanung.Connector plugin", "Sms.Einsatzplanung.Connector plugin", "Sms.Einsatzplanung.Connector plugin", "Sms.Einsatzplanung.Connector plugin");
			AddLicenseModule("FLD00050", "Sms.Scheduler plugin", "Sms.Scheduler plugin", "Sms.Scheduler plugin", "Sms.Scheduler plugin");
			AddLicenseModule("FLD01010", "Scheduler user", "Scheduler-Benutzer", "Utilisateur du planificateur", "Usuario programador", "FLD00050");
			AddLicenseModule("FLD02000", "Main.Replication plugin", "Main.Replication plugin", "Main.Replication plugin", "Main.Replication plugin");
			AddLicenseModule("FLD02050", "Integration.Exchange plugin", "Integration.Exchange plugin", "Integration.Exchange plugin", "Integration.Exchange plugin");
			AddLicenseModule("FLD03000", "Crm.Offline plugin", "Crm.Offline plugin", "Crm.Offline plugin", "Crm.Offline plugin");
			AddLicenseModule("FLD03010", "Sms.TimeManagement plugin", "Sms.TimeManagement plugin", "Sms.TimeManagement plugin", "Sms.TimeManagement plugin");
			AddLicenseModule("FLD03020", "Crm.PerDiem plugin", "Crm.PerDiem plugin", "Crm.PerDiem plugin", "Crm.PerDiem plugin");
			AddLicenseModule("FLD03040", "Crm.VisitReport plugin", "Crm.VisitReport plugin", "Crm.VisitReport plugin", "Crm.VisitReport plugin");
			AddLicenseModule("FLD03050", "Visit reports", "Besuchsberichte", "Rapports de visite", "Fecha de iniciación", "FLD03040");
			AddLicenseModule("FLD03060", "Crm.Project plugin", "Crm.Project plugin", "Crm.Project plugin", "Crm.Project plugin");
			AddLicenseModule("FLD03070", "Crm.ErpExtension plugin", "Crm.ErpExtension plugin", "Crm.ErpExtension plugin", "Crm.ErpExtension plugin");
			AddLicenseModule("FLD03080", "Crm.Campaigns plugin", "Crm.Campaigns plugin", "Crm.Campaigns plugin", "Crm.Campaigns plugin");
			AddLicenseModule("FLD03090", "Crm.Configurator plugin", "Crm.Configurator plugin", "Crm.Configurator plugin", "Crm.Configurator plugin");
			AddLicenseModule("FLD03130", "Crm.AttributeForms plugin", "Crm.AttributeForms plugin", "Crm.AttributeForms plugin", "Crm.AttributeForms plugin");
			AddLicenseModule("FLD03140", "Service cases", "Servicefälle", "Demandes de service ", "Casos de servicio", "FLD00010");
			AddLicenseModule("FLD03150", "Service contracts", "Serviceverträge", "Contrats de service", "Contratos de servicio", "FLD00010");
			AddLicenseModule("FLD03160", "Service objects", "Serviceobjekte", "Objets de service", "Objectos de servicio", "FLD00010");
			AddLicenseModule("FLD03170", "Sms.Checklists plugin", "Sms.Checklists plugin", "Sms.Checklists plugin", "Sms.Checklists plugin");
			AddLicenseModule("FLD03180", "Ad hoc service orders", "Ad-hoc-Serviceauftrag", "Ad hoc commande de service", "Orden de servicio ad hoc", "FLD00010");
			AddLicenseModule("FLD03190", "Replenishment order", "Nachbestückungsauftrag", "Commande de réapprovisionnement", "Pedido de reposición", "FLD00010");
			AddLicenseModule("FLD03210", "Main.VideoCall plugin", "Main.VideoCall plugin", "Main.VideoCall plugin", "Main.VideoCall plugin");
			AddLicenseModule("FLD03230", "Main.Flow plugin", "Main.Flow plugin", "Main.Flow plugin", "Main.Flow plugin");
			AddLicenseModule("FLD05030", "User", "Benutzer", "Utilisateur", "Usuario");
			AddLicenseModule("FLD09050", "Main.Localization plugin", "Main.Localization plugin", "Main.Localization plugin", "Main.Localization plugin");
		}
		private void AddLicenseModule(string moduleId, string enText, string deText, string frText, string esText, string parentModuleId = null)
		{
			parentModuleId = parentModuleId != null ? $"'{parentModuleId}'" : "NULL";
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{moduleId}', '{enText}', 'en', {parentModuleId},0, 0, 'Migration_20240313193000', 'Migration_20240313193000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{moduleId}', '{deText}', 'de', {parentModuleId},0, 0, 'Migration_20240313193000', 'Migration_20240313193000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{moduleId}', '{frText}', 'fr', {parentModuleId},0, 0, 'Migration_20240313193000', 'Migration_20240313193000')");
			Database.ExecuteNonQuery($"INSERT INTO [LU].[LicenseModule] (Value, Name, Language, ParentModuleId, Favorite, SortOrder, CreateUser, ModifyUser) VALUES ('{moduleId}', '{esText}', 'es', {parentModuleId},0, 0, 'Migration_20240313193000', 'Migration_20240313193000')");
		}
	}
}
