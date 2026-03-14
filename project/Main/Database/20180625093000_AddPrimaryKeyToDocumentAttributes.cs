namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180625093000)]
	public class AddPrimaryKeyToDocumentAttributes : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.DocumentAttributes')) BEGIN
					ALTER TABLE [CRM].[DocumentAttributes] ADD CONSTRAINT [PK_DocumentAttributes] PRIMARY KEY ([Id])
				END;
			");
		}
	}
}
