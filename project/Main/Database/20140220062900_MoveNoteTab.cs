namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140220062900)]
	public class MoveNoteTab : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("update [CRM].[Permission] set [PGroup] = 'Note' where PGroup='NotesTab' and Name ='OpenPerson'");
		}
	}
}