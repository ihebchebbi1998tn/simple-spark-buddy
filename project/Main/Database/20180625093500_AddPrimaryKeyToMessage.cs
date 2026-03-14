namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20180625093500)]
	public class AddPrimaryKeyToMessage : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF NOT EXISTS (SELECT 1 FROM sys.objects WHERE [type] = 'PK' AND [parent_object_id] = OBJECT_ID('CRM.Message')) BEGIN
					ALTER TABLE [CRM].[Message] ADD CONSTRAINT [PK_Message] PRIMARY KEY ([MessageId])
				END;
			");
		}
	}
}
