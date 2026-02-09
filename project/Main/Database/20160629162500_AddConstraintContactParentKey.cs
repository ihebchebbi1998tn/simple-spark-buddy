namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160629162500)]
	public class AddConstraintContactParentKey : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"
				IF EXISTS(SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'CRM.Contact') AND name = 'FK_Contact_Parent')
				BEGIN
					ALTER TABLE [CRM].[Contact] DROP CONSTRAINT [FK_Contact_Parent]
				END

			  IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'CRM.Contact') AND name = 'FK_Contact_ParentKey')
				BEGIN
					ALTER TABLE[CRM].[Contact] WITH CHECK ADD CONSTRAINT[FK_Contact_ParentKey] FOREIGN KEY([ParentKey]) REFERENCES[CRM].[Contact] ([ContactId])
				END
					
				ALTER TABLE[CRM].[Contact] CHECK CONSTRAINT[FK_Contact_ParentKey]
			");
		}
	}
}