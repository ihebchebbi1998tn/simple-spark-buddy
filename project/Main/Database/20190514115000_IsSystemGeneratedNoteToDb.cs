namespace Main.Database
{
	using System.Data;

	using Crm.Library.Data.MigratorDotNet.Framework;
	using Crm.Library.Data.MigratorDotNet.Migrator.Extensions;

	[Migration(20190514115000)]
	public class IsSystemGeneratedNoteToDb : Migration
	{
		public override void Up()
		{
			Database.AddColumnIfNotExisting("CRM.Note", new Column("IsSystemGenerated", DbType.Boolean, ColumnProperty.NotNull, true));
			Database.ExecuteNonQuery(@"
				UPDATE [CRM].[Note]
				SET IsSystemGenerated = 0
				WHERE NoteType IN ('UserNote', 'EmailNote', 'VisitReportTopicNote')
			");
		}
	}
}
