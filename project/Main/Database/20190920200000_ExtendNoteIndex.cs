namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20190920200000)]
	public class ExtendNoteIndex : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Note]') AND name = N'IX_Note_IsActive_Plugin')
			BEGIN
				DROP INDEX [IX_Note_IsActive_Plugin] ON [CRM].[Note]
			END");
			Database.ExecuteNonQuery(@"IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[CRM].[Note]') AND name = N'IX_Note_IsActive_Plugin')
			BEGIN
				CREATE NONCLUSTERED INDEX [IX_Note_IsActive_Plugin] ON [CRM].[Note]
				(
					[IsActive] ASC,
					[Plugin] ASC
					-- please include AuthDataId (from multitenant plugin) when changing this
				)
				INCLUDE ([ModifyDate],[ElementKey],[NoteId])
			END");
		}
	}
}
