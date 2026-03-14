namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20240202093400)]
	public class AddServiceOrderErrorCauseConfirmedNoteEntityTypeAndNoteType : Migration
	{
		public override void Up()
		{
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorCauseConfirmedNote", "Cause of error confirmed", "#4caf50", "check-square", "en");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorCauseConfirmedNote", "Cause de l''erreur confirmée", "#4caf50", "check-square", "fr");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorCauseConfirmedNote", "A hiba oka megerősítve", "#4caf50", "check-square", "hu");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorCauseConfirmedNote", "Causa del error confirmada", "#4caf50", "check-square", "es");
			Database.InsertLookupWithColorAndIconValues("NoteType", "ServiceOrderErrorCauseConfirmedNote", "Fehlerursache bestätigt", "#4caf50", "check-square", "de");
		}
	}
}
