namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20220614164300)]
	public class ChangeNoteTypeIcons : Migration
	{
		public override void Up()
		{
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='assignment-check' WHERE Icon='\\f108'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='case' WHERE Icon='\\f12d'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='dns' WHERE Icon='\\f156'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='email' WHERE Icon='\\f15a'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='map' WHERE Icon='\\f196'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='money' WHERE Icon='\\f19a'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='wrench' WHERE Icon='\\f1ed'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='file' WHERE Icon='\\f223'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='comment-alt-text' WHERE Icon='\\f25b'");
		Database.ExecuteNonQuery("UPDATE LU.NoteType SET Icon='phone' WHERE Icon='\\f2be'");
		Database.ExecuteNonQuery("ALTER TABLE [LU].[NoteType] DROP CONSTRAINT [DF_NoteType_Icon]");
		Database.ExecuteNonQuery("ALTER TABLE [LU].[NoteType] ADD  CONSTRAINT [DF_NoteType_Icon]  DEFAULT ('comment-alt') FOR [Icon]");
		}
	}
}
