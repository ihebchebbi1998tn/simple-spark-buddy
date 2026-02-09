namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180625095000)]
	public class AddForeignKeyDocumentAttributesFileResource : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE [parent_object_id] = OBJECT_ID('CRM.DocumentAttributes') AND [referenced_object_id] = OBJECT_ID('CRM.FileResource')) BEGIN
					ALTER TABLE CRM.DocumentAttributes
					ADD CONSTRAINT FK_DocumentAttributes_FileResource FOREIGN KEY (FileResourceKey) REFERENCES CRM.FileResource
				END;
			");
		}
	}
}
