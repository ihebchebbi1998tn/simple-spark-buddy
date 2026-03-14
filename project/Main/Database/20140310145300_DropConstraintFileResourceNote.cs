namespace Main.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20140310145300)]
	public class DropConstraintFileResourceNote : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery(@"IF  EXISTS (SELECT * FROM sys.objects WHERE name = 'FK_FileResource_Note')
				ALTER TABLE [CRM].[FileResource] DROP CONSTRAINT [FK_FileResource_Note]");
		}
		public override void Down()
		{
		}
	}
}
