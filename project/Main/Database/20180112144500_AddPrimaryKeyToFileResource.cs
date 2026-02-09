namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180112144500)]
	public class AddPrimaryKeyToFileResource : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.FileResource')) BEGIN
					ALTER TABLE [CRM].[FileResource] ADD CONSTRAINT [PK_FileResource] PRIMARY KEY ([Id])
				END;
			");
		}
	}
}