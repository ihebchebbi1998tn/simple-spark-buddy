namespace Crm.Service.Database
{
	using Crm.Library.Data.MigratorDotNet.Framework;

	[Migration(20200317084200)]
	public class ChangeTimeEntryDescriptionLengthsToNvarcharMax : Migration
	{
		public override void Up()
		{
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN [Description] nvarchar(max) NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[ServiceOrderTimePostings] ALTER COLUMN [InternalRemark] nvarchar(max) NULL");
			Database.ExecuteNonQuery("ALTER TABLE [SMS].[TimeEntry] ALTER COLUMN [Description] nvarchar(max) NULL");
		}
	}
}