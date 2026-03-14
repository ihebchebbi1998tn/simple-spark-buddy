namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;
	using Crm.Library.Data.MigratorDotNet.Migrator.Helper;
	using Crm.Model.Lookups;
	using Crm.Service.Model.Notes;

	[Migration(20240223095500)]
	public class AddServiceCaseInformationChangedNoteType : Migration {
		
		public override void Up()
		{
			if (Database.TableExists("LU.NoteType"))
			{
				Database.InsertLookupWithColorValue("NoteType", "ServiceCaseInformationChangedNote", "Servicefallinformationen geändert", "#9164a6", "de");
				Database.InsertLookupWithColorValue("NoteType", "ServiceCaseInformationChangedNote", "Service case information changed", "#9164a6", "en");
				Database.InsertLookupWithColorValue("NoteType", "ServiceCaseInformationChangedNote", "La información del caso de servicio cambió", "#9164a6", "es");
				Database.InsertLookupWithColorValue("NoteType", "ServiceCaseInformationChangedNote", "Les informations sur la demande de service ont été modifiées", "#9164a6", "fr");
				Database.InsertLookupWithColorValue("NoteType", "ServiceCaseInformationChangedNote", "A szervizeset adatai megváltoztak", "#9164a6", "hu");
				
				Database.ExecuteNonQuery(@"UPDATE LU.NoteType SET Icon = 'case' WHERE Value = 'ServiceCaseInformationChangedNote'");
			}
		}
	}
}
