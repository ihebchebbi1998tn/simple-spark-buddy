namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240201152200)]
	public class AddServiceOrderErrorTypeConfirmedNoteEntityTypeAndNoteType : Migration
	{
		public override void Up()
		{
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorTypeConfirmedNote", "Error confirmed", "#4caf50", "check-square", "en");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorTypeConfirmedNote", "Erreur confirmée", "#4caf50", "check-square", "fr");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorTypeConfirmedNote", "Hiba megerősítve", "#4caf50", "check-square", "hu");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorTypeConfirmedNote", "Error confirmado", "#4caf50", "check-square", "es");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorTypeConfirmedNote", "Fehler bestätig", "#4caf50", "check-square", "de");
		}
	}
}
