namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20160629163300)]
	public class AddConstraintCommunicationContactKey : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("IF NOT EXISTS(SELECT * FROM sys.foreign_keys WHERE parent_object_id = OBJECT_ID(N'CRM.Communication') AND name = 'FK_Communication_ContactKey') " +
				"ALTER TABLE [CRM].[Communication] WITH CHECK ADD CONSTRAINT [FK_Communication_ContactKey] FOREIGN KEY([ContactKey]) REFERENCES[CRM].[Contact]([ContactId]) " +
				"ALTER TABLE [CRM].[Communication] CHECK CONSTRAINT [FK_Communication_ContactKey]");
		}
	}
}